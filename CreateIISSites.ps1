Import-Module WebAdministration

$ScriptPath = split-path -parent $MyInvocation.MyCommand.Definition

Function CreateSite ($name,$path,[int]$port, $protocol = "http")
{
	$siteName = "iiS:\Sites\" + $name
	if (-Not (Test-Path $siteName))
	{
        $bindings = @{protocol=$protocol;bindingInformation=":" + $port + ":";IP="*"}
        
        $physicalPath = $ScriptPath + $path
        
        New-Item $siteName -bindings $bindings -physicalPath $physicalPath
	}
}

CreateSite "QRecruitment.IdentityServer" "\Quantium.Recruitment.IdentityServer" 44318 "https"
CreateSite "QRecruitment.ApiServices" "\Quantium.Recruitment.ApiServices" 60606