using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.MediaEngines;
using Adam.Tools.ExceptionHandler;

namespace CubusEngine
{
    public class CubusMediaEngine : MediaEngine
    {
        private const string GdiPlusMediaEngineId = "GDI+";
        private const string FfmpegExecutableFolder = "C:\\Program Files\\FFMPEG\\bin";
        
        public CubusMediaEngine(Application app) : base(app)
        {
            
        }

        public override string Id
        {
            get
            {
                return GdiPlusMediaEngineId;
            }
        }

        public override bool Run(MediaAction action)
        {
            if (action == null)
            {
                // No media actions were passed...
                throw ExceptionManager.CreateArgumentNullException("action");
            }

            switch (action.Id)
            {
                case CreatePreviewMoviesMediaAction.ActionId:
                    CreatePreviewImages((CreatePreviewMoviesMediaAction)action);
                    return true;

                case ReadContentMediaAction.ActionId:
                    ReadContent((ReadContentMediaAction)action);
                    return true;

                default:
                    // The requested media action is not supported by this media engine.
                    return false;
            }

        }

        private void ReadContent(ReadContentMediaAction action)
        {
            action.Content = File.ReadAllText(action.Path);
        }
        
        private void CreatePreviewImages(CreatePreviewMoviesMediaAction action)
        {
            PreviewMaker previewMaker = new PreviewMaker(FfmpegExecutableFolder);
            string previewPath = App.GetTemporaryFile("jpg");
            previewMaker.GetPreview(action.Path, previewPath);
            action.ImagePreviews.Add(new Preview(previewPath, -1));
        }



    }
}
