#
# ListVms.ps1
#

Get-SCVMMServer -ComputerName "#SCVMMSERVER" | Out-Null
get-scvirtualmachine