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
using FFmpegTool;
using FFmpegTool = FFmpegTool.FFmpegTool;

namespace Test
{
    class Program
    {
        private const string FFMPEG_FOLDER = @"D:\Downloads\ffmpeg-20151028-git-dd36749-win64-shared\bin";

        static void Main(string[] args)
        {
            string videoPath = @"D:\Downloads\input.flv";
            double videoStart = 10;
            double videoDuration = 15.5;
            string audioPath = @"D:\Downloads\input.mp3";
            string outputPath = @"D:\Downloads\output.mp4";
            
            CoubMaker maker = new CoubMaker();
            maker.MakeCoub(videoPath, videoStart, videoDuration, audioPath, outputPath);
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
            string param = String.Format(@"-ss {1} -i {0} -t {2} -c:v copy -c:a copy -y {3}", videoPath, start, duration, outputPath);
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
