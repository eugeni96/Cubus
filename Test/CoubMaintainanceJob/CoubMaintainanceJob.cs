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
            while (reader.Name != "target")
            {
                reader.Read();
            }

            audioPath = reader["audioPath"];
            videoPath = reader["videoPath"];
            Double.TryParse(reader["videoStart"], out start);
            Double.TryParse(reader["videoDuration"], out duration);
            return new CoubMaintainanceTarget(audioPath, videoPath, start, duration);
        }

        protected override void OnSerialize(System.Xml.XmlWriter writer)
        {
        }

        protected override void OnSerializeTarget(MaintenanceTarget target, System.Xml.XmlWriter writer)
        {
            CoubMaintainanceTarget t = (CoubMaintainanceTarget)target;
            writer.WriteStartElement("target");
            writer.WriteAttributeString("audioPath", t.AudioPath);
            writer.WriteAttributeString("videoPath", t.VideoPath);
            writer.WriteAttributeString("videoStart", t.VideoStart.ToString());
            writer.WriteAttributeString("videoDuration", t.VideoDuration.ToString());
            writer.WriteEndElement();
        }

        protected override Type TargetType
        {
            get { return typeof(CoubMaintainanceTarget); }
        }
    }
}
