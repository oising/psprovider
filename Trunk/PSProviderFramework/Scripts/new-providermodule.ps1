param(
    [switch]$ContainerOnly,
    [switch]$IncludeContent,
    [switch]$IncludeProperty,
    [switch]$IncludeSecurity
)
# generate skeleton module

$ta = [type]::gettype("System.Management.Automation.TypeAccelerators")
$provider = [psproviderframework.treescriptprovider]

function resolve-typeaccelerator($typename) {
    $accelerator = ($ta::get.getenumerator()|?{$_.value.fullname -eq $typename}).key
    if ($accelerator) { $accelerator } else {
        if ($typename.startswith("System.")) {
            $typename.substring(7)
        } else { $typename }
    }
}

function get-outputtype($m) {
    if ($m.returntype.name -ne "void") {
        "`n`t[outputtype('{0}')]" -f $(resolve-typeaccelerator $m.returntype.fullname)
    }
}
function get-parameters($m) {
    $p = $m.getparameters()
    $p | % {
        "`n`t`t[{0}]`${1}{2}" -f $(resolve-typeaccelerator $_.parametertype.fullname), `
            $_.name, $(if ($_.position -lt ($p.length - 1)) { "," })
    }
}

$provider.getmethods("nonpublic,instance,declaredonly") | ? {
    -not $_.isspecialname
} | ? {
    -not $_.isprivate
} | ? {
    $_.name -notlike "*dynamic*"
} | sort name | % {
@"
function {0} {{
    [cmdletbinding()]$(get-outputtype $_)
    param($(get-parameters $_)
    )

    `$psprovider.writeverbose("{0}")
    # ...
}}
"@ -f $_.name
}
