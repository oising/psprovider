function ClearItem {
    [cmdletbinding()]
    param(
		[string]$path
    )

    $psprovider.writeverbose("ClearItem")
    # ...
}
function CopyItem {
    [cmdletbinding()]
    param(
		[string]$path, 
		[string]$copyPath, 
		[bool]$recurse
    )

    $psprovider.writeverbose("CopyItem")
    # ...
}
function GetChildItems {
    [cmdletbinding()]
    param(
		[string]$path, 
		[bool]$recurse
    )

    $psprovider.writeverbose("GetChildItems")
    # ...
}
function GetChildNames {
    [cmdletbinding()]
    param(
		[string]$path, 
		[Management.Automation.ReturnContainers]$returnContainers
    )

    $psprovider.writeverbose("GetChildNames")
    # ...
}
function GetItem {
    [cmdletbinding()]
    param(
		[string]$path
    )

    $psprovider.writeverbose("GetItem")
    # ...
}
function HasChildItems {
    [cmdletbinding()]
	[outputtype('bool')]
    param(
		[string]$path
    )

    $psprovider.writeverbose("HasChildItems")
    # ...
}
function InvokeDefaultAction {
    [cmdletbinding()]
    param(
		[string]$path
    )

    $psprovider.writeverbose("InvokeDefaultAction")
    # ...
}
function IsItemContainer {
    [cmdletbinding()]
	[outputtype('bool')]
    param(
		[string]$path
    )

    $psprovider.writeverbose("IsItemContainer")
    # ...
}
function IsValidPath {
    [cmdletbinding()]
	[outputtype('bool')]
    param(
		[string]$path
    )

    $psprovider.writeverbose("IsValidPath")
    # ...
}
function ItemExists {
    [cmdletbinding()]
	[outputtype('bool')]
    param(
		[string]$path
    )

    $psprovider.writeverbose("ItemExists")
    # ...
}
function MoveItem {
    [cmdletbinding()]
    param(
		[string]$path, 
		[string]$destination
    )

    $psprovider.writeverbose("MoveItem")
    # ...
}
function NewDrive {
    [cmdletbinding()]
	[outputtype('Management.Automation.PSDriveInfo')]
    param(
		[Management.Automation.PSDriveInfo]$drive
    )

    $psprovider.writeverbose("NewDrive")
    # ...
}
function NewItem {
    [cmdletbinding()]
    param(
		[string]$path, 
		[string]$itemTypeName, 
		[Object]$newItemValue
    )

    $psprovider.writeverbose("NewItem")
    # ...
}
function RemoveItem {
    [cmdletbinding()]
    param(
		[string]$path, 
		[bool]$recurse
    )

    $psprovider.writeverbose("RemoveItem")
    # ...
}
function RenameItem {
    [cmdletbinding()]
    param(
		[string]$path, 
		[string]$newName
    )

    $psprovider.writeverbose("RenameItem")
    # ...
}
function SetItem {
    [cmdletbinding()]
    param(
		[string]$path, 
		[Object]$value
    )

    $psprovider.writeverbose("SetItem")
    # ...
}
