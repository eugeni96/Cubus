using Adam.Core;
using Adam.Core.Fields;
using Adam.Core.Indexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
