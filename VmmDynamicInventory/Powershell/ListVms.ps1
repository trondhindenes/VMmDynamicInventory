#
# ListVms.ps1
#

import-module VirtualMachineManager
Get-SCVMMServer -ComputerName "#SCVMMSERVER" | Out-Null
$vms = get-scvirtualmachine
$vms | foreach {$_ | add-member -membertype NoteProperty -name IPv4Address -value $_.VirtualNetworkAdapters[0].IPv4Addresses[0]}
$vms