using System;
using Adam.Core.Maintenance;
using Adam.Core;
using Adam.Core.Records;
using Adam.Core.Orders;
using FFmpegTool;
using Adam.Core.Fields;

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
            CoubMaker maker = new CoubMaker();
            string tempFile = App.GetTemporaryFile("mp4");            
            Record record = new Record(App);
            foreach (CoubMaintainanceTarget target in Targets)
            {
                maker.MakeCoub(target.VideoPath, target.VideoStart, target.VideoDuration, target.AudioPath, tempFile);                        
                AddRecord(record, target, tempFile);
            }
        }

        private static void AddRecord(Record record, CoubMaintainanceTarget target, string tempFile)
        {
            record.AddNew();
            record.Classifications.Add(new Adam.Core.Classifications.ClassificationPath("/Cubus/Coub"));
            record.Fields.GetField<TextField>("Name").SetValue(target.Name);
            record.Fields.GetField<TextField>("Author").SetValue(target.Author);
            record.Fields.GetField<TextField>("Description").SetValue(target.Description);
            record.Files.AddFile(tempFile);
            record.Save();
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
            double start;
            double duration;
            while (reader.Name != "target")
                reader.Read();

            audioPath = reader["audioPath"];
            videoPath = reader["videoPath"];
            Double.TryParse(reader["videoStart"], out start);
            Double.TryParse(reader["videoDuration"], out duration);
            var target = new CoubMaintainanceTarget(audioPath, videoPath, start, duration);             
            target.Name = reader["name"];
            target.Author = reader["author"];
            target.Description = reader["description"];
            return target;
        }

        protected override void OnSerialize(System.Xml.XmlWriter writer)
        {
        }

        protected override void OnSerializeTarget(MaintenanceTarget target, System.Xml.XmlWriter writer)
        {
            CoubMaintainanceTarget t = (CoubMaintainanceTarget)target;
            writer.WriteStartElement("target");
            writer.WriteAttributeString("author", t.Author);
            writer.WriteAttributeString("name", t.Name);
            writer.WriteAttributeString("description", t.Description);
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
