using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Adam.Core;
using Adam.Core.Fields;
using Adam.Core.Maintenance;
using Adam.Core.MediaEngines;
using Adam.Core.Search;
using Adam.Tools.MediaEngines;
using CubusEngine;
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

            //CustomIndexer.ClassificationInitializer.InitializeSpecifications(app);

            //CubusIndexer.AddIndexer(app);

            RunMaintenanceManager();



            app.LogOff();

        }
        public static void RunMaintenanceManager()
        {
            Application app = new Application();
            LogOnStatus status = app.LogOn("Training", "Eugeni.Tseitlin", "sp8hxj4v3a");
            if (status != LogOnStatus.LoggedOn)
                throw new ServerRegistrationException("Maintenance manager can't connect to Adam server. LogOn status = " + status);
            MaintenanceManager mm = new MaintenanceManager(app);
            mm.Priorities = "8";
            mm.TimeOut = TimeSpan.FromMinutes(3);
            mm.Execute();
            Console.WriteLine("Execute job");
            app.LogOff();
        }

        public static void CheckMediaEngine(Application app)
        {
            try
            {
                string path = Path.Combine(app.WorkingFolder, "test.mp4");
                CreatePreviewMoviesMediaAction createPreviewMovies = new CreatePreviewMoviesMediaAction(false, path,
                    "mp4");

                MediaManager manager = new MediaManager(app);
                manager.Run(
                    new IMediaEngine[] {new CubusMediaEngine(app)},
                    new MediaAction[] {createPreviewMovies});

                if (createPreviewMovies.IsSuccess)
                {
                    Console.WriteLine(createPreviewMovies.MoviePreviews[0].Path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
