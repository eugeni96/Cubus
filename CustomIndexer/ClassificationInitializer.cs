using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Fields;
using Adam.Core.Search;

namespace CustomIndexer
{
    public static class ClassificationInitializer
    {
        public static void InitializeSpecifications(Application app)
        {
            Classification classification = new Classification(app);
            if (classification.TryLoad(new ClassificationPath("/Cubus")) == TryLoadResult.NotFound)
            {
                
                classification.AddNew();
                classification.Name = "Cubus";
                classification.Save();

                List<String> fieldList = new List<string>() { "Name", "Author", "Dicription" };
                FieldDefinitionHelper fieldDefinitionHelper = new FieldDefinitionHelper(app);
                foreach (var field in fieldList)
                {
                    if (fieldDefinitionHelper.Exists(field))
                        continue;
                    TextFieldDefinition fieldCreator = new TextFieldDefinition(app);
                    fieldCreator.AddNew();
                    fieldCreator.Name = field;
                    fieldCreator.Save();
                }

                foreach (var field in fieldList)
                {
                    var guid = fieldDefinitionHelper.GetId("Creator");
                    if (guid != null)
                    {
                        Guid id = (Guid) guid;
                        classification.RegisteredFields.Add(id);
                    }
                }

                Classification video = new Classification(app);
                video.AddNew(classification.Id);
                video.Name = "Video";

                Classification audio = new Classification(app);
                audio.AddNew(classification.Id);
                audio.Name = "Audio";
                
                audio.Save();
                video.Save();
            }
        }
    }
}
