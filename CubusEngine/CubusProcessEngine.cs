﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Fields;
using Adam.Core.Indexer;

namespace CubusEngine
{
    public class CubusProcessEngine : IndexMaintenanceJob
    {
        public CubusProcessEngine(Application app)
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
            Dictionary<String, bool> definitionNames = new Dictionary<string, bool>();
            foreach (Field field in e.Record.Fields.MyLanguage)
                definitionNames.Add(field.Container.Definition.Name, false);
            var xmlMeta = new XmlDocument();
            xmlMeta.Load(metaFilePath);
            XmlNodeList nodes = xmlMeta.GetElementsByTagName("item").Item(0).ChildNodes;
            foreach (XmlNode node in nodes)
                if (definitionNames.Keys.Any(x => x.Equals(node.Name, StringComparison.OrdinalIgnoreCase)))
                    e.Record.Fields.GetField<TextField>(node.Name).SetValue(node.InnerText);
            System.IO.File.Delete(metaFilePath);
        }
    }
}
