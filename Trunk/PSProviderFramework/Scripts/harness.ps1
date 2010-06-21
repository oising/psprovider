ipmo c:\projects\PowerShell\PSProvider\PSProviderFramework\bin\Debug\PSProviderFramework.dll

if (Get-PSDrive ps -ea 0) { remove-psdrive ps  }

new-psdrive ps containerscriptprovider -root / -moduleinfo $(new-module -name test {

	$data = @{
		a = "a" | select foo,bar
		b = "b" | select foo,bar
		c = "c" | select foo,bar
		d = "d" | select foo,bar
	}

	function getitem($path) {
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

	function itemexists($path) {

		if ($path) {

			$psprovider.writeverbose("item exists $path")
			$data.containskey($path)

		} else {

			# root always exists
			$true
		}
	}

	function getchildnames($path, $returnContainers) {
		
		$psprovider.writeverbose("getchildnames '$path' $returnContainers")
		
		if ($path) {
			if ($data[$path]) {
                $psprovider.writeitemobject($path, $path, $false)
            }
		} else {
			$data.keys | % { $psprovider.writeitemobject($_, [string]$_, $false) }
		}
	}

	function getchilditems($path, $recurse) {

		$psprovider.writeverbose("getchildnames '$path' $returnContainers")

		if ($path) {
			$psprovider.writeitemobject($data[$path], $_, $false)
		} else {
			$data.keys | % {
				$psprovider.writeitemobject($data[$_], $_, $false)
			}
		}
	}

	function clearitem($path) {
		$psprovider.writeverbose("clearitem '$path'")
	}
})

dir ps:\a