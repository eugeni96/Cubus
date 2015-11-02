using Adam.Core;
using Adam.Core.Fields;
using Adam.Core.Indexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Adam.Core.Classifications;

namespace CustomIndexer
{
    public class CoubusIndexerEngine : IndexMaintenanceJob
    {
        public CoubusIndexerEngine(Application app)
            : base(app)
        {
        }


        protected override void OnCatalog(CatalogEventArgs e)
        {
            base.OnCatalog(e);
            // Добавить классификации в зависимости от расширения

            // пока что дикий хардкод с путями и расширениями
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
            }
            if (audioExtensions.Any(x => x.Equals(Path.GetExtension(e.Path))))
            {
                e.Record.Classifications.Add(new ClassificationPath("/Cubus/Audio"));
            }


            String metaFilePath = System.IO.Path.ChangeExtension(e.Path, ".cm");
            if (!System.IO.File.Exists(metaFilePath))
                return;
            Dictionary<String, bool> fields = new Dictionary<string, bool>();
            foreach (Field field in e.Record.Fields.MyLanguage)
                fields.Add(field.ValuePropertyName, false);
            var doc = new XmlDocument();
            doc.Load(metaFilePath);
            foreach (XmlNode node in doc.ChildNodes)
                if (fields.Keys.Contains(node.Name))
                    e.Record.Fields.GetField<TextField>(node.Name).SetValue(node.Value);
        }
    }
}
