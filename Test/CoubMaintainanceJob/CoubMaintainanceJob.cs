using System;
using Adam.Core.Maintenance;
using Adam.Core;
using Adam.Core.Records;
using Adam.Core.Orders;
using FFmpegTool;

namespace CoubMaintainanceJob
{
    public class CoubMaintainanceJob : ManualMaintenanceJob 
    {

        public CoubMaintainanceJob(Application app)
            : base(app)
        {
        }


        protected override void OnExecute()
        {
            CoubMaker maker = new CoubMaker(@"D:\Downloads\ffmpeg-20151028-git-dd36749-win64-shared\bin");
            Record record = new Record(App);
            foreach (CoubMaintainanceTarget target in Targets)
            {
                string tempFile = App.GetTemporaryFile("mp4");
                maker.MakeCoub(target.VideoPath, target.VideoStart, target.VideoDuration, target.AudioPath, tempFile);
                record.AddNew();
                record.Classifications.Add(new Adam.Core.Classifications.ClassificationPath("/Training/Helena.Kolodko"));
                // Add preview
                record.Files.AddFile(tempFile);
                record.Save();
            }
        }

        public override bool IsRetryingSupported
        {
            get { return false; }
        }

        protected override void OnDeserialize(System.Xml.XmlReader reader)
        {
        }

        protected override MaintenanceTarget OnDeserializeTarget(System.Xml.XmlReader reader)
        {
            string audioPath = "";
            string videoPath = "";
            double start = 0;
            double duration = 0;
            reader.Read();
            reader.Read();
            reader.Read();
		    audioPath = reader.Value.Trim();
            reader.Read();
            reader.Read();
            reader.Read();
            videoPath = reader.Value.Trim();
            reader.Read();
            reader.Read();
            reader.Read();
            Double.TryParse(reader.Value.Trim(), out start);
            reader.Read();
            reader.Read();
            reader.Read();
            Double.TryParse(reader.Value.Trim(), out duration);
            return new CoubMaintainanceTarget(audioPath, videoPath, start, duration);
        }

        protected override void OnSerialize(System.Xml.XmlWriter writer)
        {
        }

        protected override void OnSerializeTarget(MaintenanceTarget target, System.Xml.XmlWriter writer)
        {
            CoubMaintainanceTarget t = (CoubMaintainanceTarget)target;
            writer.WriteElementString("audioPath", t.AudioPath);
            writer.WriteElementString("videoPath", t.VideoPath);
            writer.WriteElementString("videoStart", t.VideoStart.ToString());
            writer.WriteElementString("videoDuration", t.VideoDuration.ToString());
        }

        protected override Type TargetType
        {
            get { return typeof(CoubMaintainanceTarget); }
        }
    }
}
