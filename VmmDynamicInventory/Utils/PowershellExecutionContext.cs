using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management.Automation;
using System.Configuration;

using VmmDynamicInventory.Models;
namespace VmmDynamicInventory.Utils
{
    public static class PowershellExecutionContext
    {
        
        public static Dictionary<String, Object> ListVms()
        {

            string vmmServerName = ConfigurationManager.AppSettings["vmm:vmmServer"];
            string ansibleGroupBy = ConfigurationManager.AppSettings["vmm:AnsibleGroupProperty"];
            String strAnsibleShowHostsWithoutGroup = ConfigurationManager.AppSettings["vmm:AnsibleShowHostsWithoutGroup"];
            String ansibleHostProperty = ConfigurationManager.AppSettings["vmm:AnsibleHostProperty"];
            bool ansibleShowHostsWithoutGroup = System.Convert.ToBoolean(strAnsibleShowHostsWithoutGroup);

            var localFolder = HttpContext.Current.Server.MapPath("/");
            String listVmsString = localFolder + @"Powershell\ListVms.ps1";
            string scriptText = System.IO.File.ReadAllText(listVmsString);
            scriptText = scriptText.Replace("#SCVMMSERVER", vmmServerName);

            var ansibleHostList = new Dictionary<String, Object>();

            PSDataCollection<PSObject> outputDataCollection = new PSDataCollection<PSObject>();
            using (PowerShell powerShell = PowerShell.Create())
            {
                powerShell.AddScript(scriptText);
                
                IAsyncResult psInvoke = powerShell.BeginInvoke<PSObject, PSObject>(null, outputDataCollection);
                var result = powerShell.EndInvoke(psInvoke);
            }

            List<Vm> vmList = new List<Vm>();
            foreach (var vm in outputDataCollection)
            {
                string vmName = vm.Properties["Name"].Value.ToString();

                String ansibleTag = null;
                
                if ((vm.Properties[ansibleGroupBy].Value != null))
                {
                    ansibleTag = vm.Properties[ansibleGroupBy].Value.ToString();
                }
                else
                {
                    ansibleTag = "no_group";
                }

                //since we cast to string, empty values may show up as "(none)"
                if (ansibleTag == "(none)")
                {
                    ansibleTag = "no_group";
                }
                
                String hostGroupPath = vm.Properties["HostGroupPath"].Value.ToString();

                String ansibleHostName = vm.Properties[ansibleHostProperty].Value.ToString();

                Vm thisVM = new Vm();
                thisVM.VmName = vmName;
                thisVM.AnsibleGroup = ansibleTag;
                thisVM.HostGroupPath = hostGroupPath;
                thisVM.AnsibleHostName = ansibleHostName;
                
                System.Diagnostics.Debug.WriteLine(string.Format("VM {0} is in Ansigle group {1}", thisVM.AnsibleHostName, thisVM.AnsibleGroup));
                if ((ansibleShowHostsWithoutGroup == true) ||(ansibleTag != "no_group"))
                {
                    vmList.Add(thisVM);
                }
                
            }

            

            var AnsibleGroups = vmList.Select(x => x.AnsibleGroup).Distinct();
            foreach (var AnsibleGroup in AnsibleGroups)
            {
                var thisGroupVMs = vmList.Where(x => x.AnsibleGroup == AnsibleGroup);
                var hostList = new List<Object>();
                if (hostList.Count > 0)
                {
                    foreach (var vm in thisGroupVMs)
                    {
                        hostList.Add(vm.AnsibleHostName);
                    }

                    var cloudList = new Dictionary<String, Object>();
                    cloudList.Add("hosts", hostList);
                    ansibleHostList.Add(AnsibleGroup.ToString(), cloudList);
                }
                


            }

            return ansibleHostList;
        }
    }
}