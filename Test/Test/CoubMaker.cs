using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class CoubMaker
    {
        private string ffmpegPath;
        private string ffprobePath;
        private string tempPath = Path.GetTempPath();


        public CoubMaker(string ffmegExecutableFolder)
        {
            if (!Directory.Exists(ffmegExecutableFolder))
	            throw new ArgumentException("Provided folder doesn't exists or doesn't have required permission",
                    "ffmegExecutableFolder");
            ffmpegPath = Path.Combine(ffmegExecutableFolder, "ffmpeg.exe");
            if (!File.Exists(ffmpegPath))
	            throw new ArgumentException("Provided folder doesn't contain  file ffmpeg.exe or doesn't have required permission.",
                    "ffmegExecutableFolder");
            ffprobePath = Path.Combine(ffmegExecutableFolder, "ffprobe.exe");
            if (!File.Exists(ffprobePath))
                throw new ArgumentException("Provided folder doesn't contain  file ffprobe.exe or doesn't have required permission.", "ffmegExecutableFolder");
   
        }

        public void MakeCoub(string videoPath, string audioPath, string coubPath)
        {
            double videoDuration = GetDuration(videoPath);
            double audioDuration = GetDuration(audioPath);
            string listPath = Path.Combine(tempPath, "list.txt");

            if (audioDuration > videoDuration)
            {
                int cicles = (int)(audioDuration / videoDuration);
                using (StreamWriter file = new StreamWriter(listPath))
                {
                    for (int i = 0; i <= cicles; i++)
                    {
                        file.WriteLine("file '{0}'\n", videoPath);
                    }
                }
                ConcatAndReduce(listPath, audioPath, coubPath);
            }
        }

        private double GetDuration(string mediaPath)
        {
            string param = String.Format(@"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {0}", mediaPath);
            var prc = new Process
            {
                StartInfo = new ProcessStartInfo(ffprobePath, param)
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
            };
            prc.Start();
            double duration = 0.0;
            if (!prc.StandardOutput.EndOfStream)
            {
                duration = double.Parse(prc.StandardOutput.ReadLine(), CultureInfo.InvariantCulture);
            }
            return duration;
        }

        private void ConcatAndReduce(string txtPath, string audioPath,  string outputPath)
        {
            string param = String.Format("-i {1} -f concat -i {0} -c:v copy -c:a copy -y -shortest {2}",
                txtPath, audioPath, outputPath);
            var inf = new ProcessStartInfo(ffmpegPath, param)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            var prc = new Process();
            prc.StartInfo = inf;
            prc.Start();
            prc.WaitForExit();
        }
    }
}
