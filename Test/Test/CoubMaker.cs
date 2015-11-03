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
                int loops = (int)(audioDuration / videoDuration);
                GenerateConcatinaionFile(videoPath, listPath, loops);
                ConcatVideoAndReduce(listPath, audioPath, coubPath);
            }
            else
            {
                int loops = (int)(videoDuration / audioDuration);
                GenerateConcatinaionFile(audioPath, listPath, loops);
                ConcatAudioAndReduce(listPath, videoPath, coubPath);
            }

        }

        private static void GenerateConcatinaionFile(string mediaPath, string listPath, int loops)
        {
            using (StreamWriter file = new StreamWriter(listPath))
            {
                for (int i = 0; i <= loops; i++)
                {
                    file.WriteLine("file '{0}'\n", mediaPath);
                }
            }
        }

        private double GetDuration(string mediaPath)
        {
            string param = String.Format(@"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {0}", mediaPath);
            StreamReader output = Execute(ffprobePath, param);
            double duration = 0.0;
            if (!output.EndOfStream)
            {
                duration = double.Parse(output.ReadLine(), CultureInfo.InvariantCulture);
            }
            return duration;
        }

        private void ConcatVideoAndReduce(string txtPath, string audioPath,  string outputPath)
        {
            string param = String.Format("-i {1} -f concat -i {0} -c:v copy -c:a copy -y -shortest {2}",
                txtPath, audioPath, outputPath);
            Execute(ffmpegPath, param);
        }

        private void ConcatAudioAndReduce(string txtPath, string videoPath, string outputPath)
        {
            string param = String.Format(" -f concat -i {0} -i {1} -c:v copy -c:a copy -y -shortest {2}",
                txtPath, videoPath, outputPath);
            Execute(ffmpegPath, param);
        }

        private StreamReader Execute(string programPath, string param)
        {
            var inf = new ProcessStartInfo(programPath, param)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            var prc = new Process();
            prc.StartInfo = inf;
            prc.Start();
            prc.WaitForExit();
            return prc.StandardOutput;
        }

        
    }
}
