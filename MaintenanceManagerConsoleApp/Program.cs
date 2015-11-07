using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.Maintenance;

namespace MaintenanceManagerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Application();
            LogOnStatus status = app.LogOn("Training", "Eugeni.Tseitlin", "sp8hxj4v3a");
            if (status != LogOnStatus.LoggedOn)
                throw new UnauthorizedAccessException("Maintenance manager can't connect to Adam server. LogOn status = " + status);
            MaintenanceManager mm = new MaintenanceManager(app);
            mm.Priorities = "8";
            mm.TimeOut = TimeSpan.FromMinutes(3);
            mm.Execute();
            Console.WriteLine("Execute job");
            app.LogOff();
        }
    }
}
