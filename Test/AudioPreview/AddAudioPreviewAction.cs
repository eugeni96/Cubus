using Adam.Core.MediaEngines;
using Adam.Core.Records;
using Adam.Tools.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPreview
{
    public class AddAudioPreviewAction : MediaAction, ICatalogAction
    {
        private string audioPath;
        private List<string> audioPreviews = new List<string>();
        private List<string> albumCovers = new List<string>();
        public AddAudioPreviewAction(CatalogActionData data)
            : base(data.IsCritical)
        {
            if (data == null)
                throw ExceptionManager.CreateArgumentNullException("data");
            audioPath = data.Path;
        }
        public AddAudioPreviewAction(string filepath, bool isCritical)
            : base(isCritical)
        {
            if (filepath == null)
                throw ExceptionManager.CreateArgumentNullException("filepath");
            audioPath = filepath;
        }

        public override string Id
        {
            get { return ActionId; }
        }

        public static string ActionId
        {
            get { return "AddAudioPreviewAction"; }
        }

        public string AudioPath { get { return audioPath; } }
        public List<string> AudioPreviews { get { return audioPreviews; } }
        public List<string> AlbumCovers { get { return albumCovers; } }

        public void UpdateFileVersion(FileVersion version, System.Xml.XmlWriter writer)
        {
            foreach (var cover in albumCovers)
                version.Previews.Add(cover);
            foreach (var preview in audioPreviews)
                version.AdditionalFiles.Add(preview);
        }

        
    }
}
