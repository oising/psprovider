ipmo c:\projects\PowerShell\PSProvider\PSProviderFramework\bin\Debug\PSProviderFramework.dll

if (Get-PSDrive data -ea 0) { remove-psdrive data }
 
New-PSDrive data ContainerScriptProvider -Root / -ModuleInfo $(New-Module -Name test -Args @{ 
		
        a = "this is node a."
		b = "this is node b."
		c = "this is node c."
		d = "this is node d."
	
    } -ScriptBlock {
	
    param($data)
    
   	function GetContentReader($path) {
		$psprovider.writeverbose("getcontentreader '$path'")

        # initialize for read operation
		$item = $data[$path]
        $content = [char[]]$item
        $line = 0

        # count lines in item        
        $lines = & {
            $reader = new-object io.stringreader $item
            $n = 0
            # need explicit null check; empty line would be treated as $false
            while ($reader.readline() -ne $null) {
                $n++
            }
            $reader.close()
            $n
        }
        
		# should close around our locals, esp. $position
        # to support concurrent content readers. 
		& {
            # create a new content reader and return it
			New-ContentReader -OnRead {
				param([long]$count)
				
                # this implementation returns a string where $count represents number of lines to return
                # at a time; you may choose to return whatever you like, and treat $count in any way you feel
                # is appropriate. All that matters is that you return an array. Return an empty array to signify
                # the end of the stream.
				
                # yes, i could use stringbuilder here but i figure the algorithm is more general purpose for a sample
                # as this could be easily adopted for byte arrays.
                $remaining = $content.length - $position
				
				if ($remaining -gt 0) {
                
                    if ($count -gt $remaining) {
                        $len = $remaining
                    } else {
                        $len = $count
                    }
					$output = new-object char[] $len
                    
                    [array]::Copy($content, $position, $output, 0, $len)
                    $position += $len
                    
                    @($output -join "")

				} else {
                
					# end stream, return empty array
                    write-verbose "read: EOF" -verbose
                    @()
                }

			} -OnSeek {                
			    param([long]$offset, [io.seekorigin]$origin)
            	write-verbose "seek: $offset origin: $origin" -verbose
            
            } -OnClose {
                # perform any cleanup you like here.
				write-verbose "read: close!" -verbose
			}
		}.getnewclosure()
	}

    function GetContentWriter($path) {
        $psprovider.writeverbose("getcontentwriter '$path'")
                
        # initialize for write operation
		$item = $data[$path]
        $position = 0
        
        & {
            New-ContentWriter -OnWrite {
                param([collections.ilist]$content)
                
                write-verbose "write: $($content.length) element(s)." -verbose
                
                $content
                
            } -OnSeek {
                # seek must be implemented to support -Append, Add-Content etc
                param([long]$offset, [io.seekorigin]$origin)
                write-verbose "seek: $offset origin: $origin" -verbose
                
                switch ($origin) {
                    "end" {
                        $position = $item.length + $offset
                        write-verbose "seek: new position at $position" -verbose
                    }
                    default {
                        write-warning "unsupported seek."
                    }
                }
                
            } -OnClose {
                # perform any cleanup you like here.
				write-verbose "write: close!" -verbose            
            }
        }.getnewclosure()
    }
    
	function GetItem($path) {
		$psprovider.writeverbose("getitem '$path'")
		
        if ($path) {
            
			if ($data[$path]) {
				$psprovider.writeitemobject($data[$path], $path, $false)
			}

		} else {

			# root
			$psprovider.writeitemobject($data.values, "/", $true)
		}
	}

	function ItemExists($path) {

		if ($path) {

			$psprovider.writeverbose("item exists $path")
			$data.containskey($path)

		} else {

			# root always exists
			$true
		}
	}

	function GetChildNames($path, $returnContainers) {
		
		$psprovider.writeverbose("getchildnames '$path' $returnContainers")
		
		if ($path) {
			if ($data[$path]) {
                $psprovider.writeitemobject($path, $path, $false)
            }
		} else {
			$data.keys | % { $psprovider.writeitemobject($_, [string]$_, $false) }
		}
	}

	function GetChildItems($path, $recurse) {

		$psprovider.writeverbose("getchildnames '$path' $returnContainers")

		if ($path) {
			$psprovider.writeitemobject($data[$path], $_, $false)
		} else {
			$data.keys | % {
				$psprovider.writeitemobject($data[$_], $_, $false)
			}
		}
	}

	function ClearItem($path) {
		$psprovider.writeverbose("clearitem '$path'")
	}
})

gc data:\a -verbose -ReadCount 8