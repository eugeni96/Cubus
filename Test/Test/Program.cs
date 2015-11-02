using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string videoPath = @"C:\ffmpeg\bin\1.mp4";
            double videoStart = 0;
            double videoDuration = 15;
            string audioPath = @"C:\ffmpeg\bin\1.mp3";
            string outputPath = @"C:\ffmpeg\bin\3.mp4";
            string tempPath = @"C:\ffmpeg\bin\temp.mp4";
            string txtPath = @"C:\ffmpeg\bin\temp.txt";


            double audioDuration = GetDuration(audioPath);
            if (audioDuration > videoDuration)
            {
                int n = (int) (audioDuration/videoDuration);
                Cut(videoPath, videoStart, videoDuration, tempPath);
                string list = null;
                using (StreamWriter file = new StreamWriter(txtPath))
                {
                    for (int i = 0; i <= n; i++)
                    {
                        file.WriteLine("file '{0}'\n", tempPath);
                    }
                }
                ConcatAndReduce(txtPath,audioPath,outputPath);
            }
            Console.ReadLine();
        }

        private static void ConcatAndReduce(string txtPath, string audioPath, string outputPath)
        {
            string param = String.Format("-i {1} -f concat -i {0} -c copy {2}", txtPath, audioPath, outputPath);
            var inf = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe", param)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var prc = new Process();
            prc.StartInfo = inf;
            prc.Start();
        }
        private static double GetDuration(string audioPath)
        {
            string param = String.Format(@"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {0}", audioPath);
            var inf = new ProcessStartInfo(@"C:\ffmpeg\bin\ffprobe.exe", param)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var prc = new Process
            {
                StartInfo = inf
            };
            prc.Start();
            double duration=0.0;
            while (!prc.StandardOutput.EndOfStream) {
                duration=double.Parse(prc.StandardOutput.ReadLine(), CultureInfo.InvariantCulture);
            }
            return duration;
        }

        private static void Cut(string videoPath, double start, double duration,string outputPath)
        {
            string param = String.Format(@"-ss {1} -i {0} -t {2} -c:v copy -c:a copy {3}", videoPath, start, duration, outputPath);
            var inf = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe", param)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var prc = new Process
            {
                StartInfo = inf
            };
            prc.Start();
        }
    }
}
