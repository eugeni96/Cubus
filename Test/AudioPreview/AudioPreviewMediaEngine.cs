using System;
using Adam.Core;
using Adam.Core.MediaEngines;
using Adam.Tools.MediaEngines;
using Adam.Tools.ExceptionHandler;
using System.IO;
using Adam.Core.Records;

namespace AudioPreview
{
    public class AudioPreviewMediaEngine : MediaEngine
    {
        public AudioPreviewMediaEngine(Application app)
            : base(app)
        {
            
        }

        public override string Id
        {
            get { return "AudioPreviewMediaEngine"; }
        }

        public override bool Run(MediaAction action)
        {
            if (action == null)
            {
                throw ExceptionManager.CreateArgumentNullException("action");
            }
            if (action.Id == AddAudioPreviewAction.ActionId)
            {
                CreatePreview((AddAudioPreviewAction)action);
                return true;
            }
            else
                return false;

        }

        private void CreatePreview(AddAudioPreviewAction action)
        {
            string previewPath = App.GetTemporaryFile("jpg");
            string path = App.GetSetting(".ffmpegInstallationPath").ToString();
            try
            {
                PreviewMaker maker = new PreviewMaker(path);
                maker.GetAlbumCover(action.AudioPath, previewPath);
                if (System.IO.File.Exists(previewPath))
                     action.AlbumCovers.Add(previewPath);

                previewPath = Path.ChangeExtension(previewPath, ".mp3");
                maker.GetAudioPreview(action.AudioPath, 30, previewPath);
                if (System.IO.File.Exists(previewPath))
                    action.AudioPreviews.Add(previewPath);
            }
            catch (ArgumentException e) 
            { }          
        }
    }
}
