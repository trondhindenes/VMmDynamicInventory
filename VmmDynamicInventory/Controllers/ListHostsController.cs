using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

using VmmDynamicInventory.Utils;
using VmmDynamicInventory.Models;
using System.Text;

namespace VmmDynamicInventory.Controllers
{
    public class ListHostsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var ansibleHosts =  PowershellExecutionContext.ListVms();
            String json = JsonConvert.SerializeObject(ansibleHosts);
            System.Diagnostics.Debug.WriteLine(json);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }
    }
}
