using System;

namespace CubusEngine
{
    public class PreviewMaker : FFmpegTool
    {
        public PreviewMaker(string ffmpegExecutableFolder) : base(ffmpegExecutableFolder) { }
        public void GetPreview(string videoPath, string outputPath)
        {
            string param = String.Format(" -i \"{0}\" -ss 00:00:2.0 -vframes 1 \"{1}\"", videoPath, outputPath);
            Execute(ffmpegPath, param);
        }
    }
}
