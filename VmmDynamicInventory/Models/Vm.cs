using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VmmDynamicInventory.Models
{
    public class Vm
    {
        
        public String VmName { get; set; }
        public String ComputerName { get; set; }
        public String HostGroupPath { get; set; }
        public bool IsHighlyAvailable { get; set; }
        public String Cloud { get; set; }
        public String AnsibleGroup { get; set; }
        public String AnsibleHostName { get; set; }
    }
}