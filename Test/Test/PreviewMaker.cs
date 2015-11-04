using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class PreviewMaker:FFmpegTool
    {
        public PreviewMaker(string ffmegExecutableFolder)
            : base(ffmegExecutableFolder)
        {         
        }

        public void GetPreview(string videoPath, string outputPath)
        {
            string param = String.Format(" -i {0} -ss 00:00:1.0 -vframes 1 {1}", videoPath, outputPath);
            Execute(ffmpegPath, param);
        }       
    }
}
