using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.Classifications;

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
