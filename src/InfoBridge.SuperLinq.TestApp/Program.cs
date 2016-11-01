using InfoBridge.SuperLinq.Core;
using InfoBridge.SuperLinq.Core.Models;
using SuperOffice;
using SuperOffice.Configuration;
using SuperOffice.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Settings.DefaultExecutionContext = (action) => action.DateTimeToUTC = true;
                ConfigFile.Services.ApplicationToken = ConfigurationManager.AppSettings["SoAppToken"];
                ConfigFile.Services.RemoteBaseURL = ConfigurationManager.AppSettings["SoBaseUrl"];
                using (SoSession.Authenticate(ConfigurationManager.AppSettings["SoUser"], ConfigurationManager.AppSettings["SoPass"]))
                {
                    var r = new Queryable<Person>().Where(x => x.ContactId == 2).ToList();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.InnerException is SoServerException)
                {
                    var serverEx = (SoServerException)ex.InnerException?.InnerException;
                    throw new Exception(serverEx.ExceptionInfo?.Message ?? serverEx.Message, serverEx);
                }
                throw;
            }
        }
    }
}
