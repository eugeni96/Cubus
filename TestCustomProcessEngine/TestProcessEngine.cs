using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Fields;
using Adam.Core.Indexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestCustomProcessEngine
{
    public class TestProcessEngine : IndexMaintenanceJob
    {
        public TestProcessEngine(Application app)
            : base(app)
        {
        }

        protected override void OnCatalog(CatalogEventArgs e)
        {
            
            base.OnCatalog(e);

            List<string> videoExtensions = new List<string>()
            {
                ".mp4",
                ".avi",
                ".mkv"
            };
            List<string> audioExtensions = new List<string>()
            {
                ".flac",
                ".mp3",
                ".wav"
            };

            if (videoExtensions.Any(x => x.Equals(Path.GetExtension(e.Path))))
            {
                e.Record.Classifications.Add(new ClassificationPath("/Cubus/Video"));
                string ffmpegExecutableFolder = e.Record.App.GetSetting(".ffmpegInstallationPath").ToString();
                PreviewMaker previewMaker = new PreviewMaker(ffmpegExecutableFolder);
                string previewPath = Path.ChangeExtension(e.Path, ".jpg");
                previewMaker.GetPreview(e.Path, previewPath);
                e.Record.Files.Master.Versions.Latest.Previews.Add(previewPath);
            }
            if (audioExtensions.Any(x => x.Equals(Path.GetExtension(e.Path))))
            {
                e.Record.Classifications.Add(new ClassificationPath("/Cubus/Audio"));
            }

            String metaFilePath = System.IO.Path.ChangeExtension(e.Path, ".cm");
            if (!System.IO.File.Exists(metaFilePath))
                return;
            Dictionary<String, bool> definitionNames = new Dictionary<string, bool>();
            foreach (Field field in e.Record.Fields.MyLanguage)
                definitionNames.Add(field.Container.Definition.Name, false);
            var xmlMeta = new XmlDocument();
            xmlMeta.Load(metaFilePath);
            XmlNodeList nodes = xmlMeta.GetElementsByTagName("item").Item(0).ChildNodes;
            foreach (XmlNode node in nodes)
                if (definitionNames.Keys.Any(name => String.Equals(name, node.Name, StringComparison.OrdinalIgnoreCase)))
                    e.Record.Fields.GetField<TextField>(node.Name).SetValue(node.InnerText);
            System.IO.File.Delete(metaFilePath);
        }
    }
}
