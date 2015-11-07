using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Orders;
using Adam.Core.Records;
using Adam.Core.Search;
using Adam.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using CoubMaintainanceJob;
using Adam.Core.Maintenance;

namespace CoubJob
{
    class Program
    {
        static void Main(string[] args)
        {

            Application app = new Application();
            LogOnStatus status = app.LogOn("TRAINING", "Helena.Kolodko", "gfhjkm");
            Console.WriteLine(status);


            Record record = new Record(app);
            RecordCollection records = new RecordCollection(app);
            records.Load(new SearchExpression("File.Version.Extension = mp4"));
            record = records.First<Record>();
            string videoPath = record.Files.LatestMaster.Path;

            records.Load(new SearchExpression("File.Version.Extension = mp3"));
            record = records.First<Record>();
            string audioPath = record.Files.LatestMaster.Path;

            MaintenanceManager manager = new MaintenanceManager(app);
            CoubMaintainanceTarget target = new CoubMaintainanceTarget(audioPath, videoPath, 0, 8.8);
            target.Name = "Something";
            target.Author = "Someone";

            CoubMaintainanceJob.CoubMaintainanceJob job = new CoubMaintainanceJob.CoubMaintainanceJob(app);
            job.AddNew();
            job.Targets.Add(target);
            job.Save();
            manager.JobIds.Add(job.Id);
            manager.Execute();
            app.LogOff();
            Console.ReadLine();

        }
    }
}
