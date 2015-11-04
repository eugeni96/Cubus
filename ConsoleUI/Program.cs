using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.Fields;
using Adam.Core.Search;
using CustomIndexer;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Application();
            LogOnStatus status = app.LogOn("TRAINING", "Eugeni.Tseitlin", "sp8hxj4v3a");

            if (status != LogOnStatus.LoggedOn)
            {
                throw new UnauthorizedAccessException();
            }

            CustomIndexer.ClassificationInitializer.InitializeSpecifications(app);

            //CubusIndexer.AddIndexer(app);
        }
    }
}
