#
# ListVms.ps1
#

import-module VirtualMachineManager
Get-SCVMMServer -ComputerName "#SCVMMSERVER" | Out-Null
get-scvirtualmachine