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
        private const string FFMPEG_FOLDER = @"C:\Program Files\ffmpeg\bin";

        static void Main(string[] args)
        {
            string videoPath = @"D:\Downloads\input.avi";
            double videoStart = 10;
            double videoDuration = 15.5;
            string audioPath = @"D:\Downloads\input.mp3";
            string outputPath = @"D:\Downloads\output.mp4";
            
            CoubMaker maker = new CoubMaker(FFMPEG_FOLDER);
            maker.MakeCoub(videoPath, videoStart, videoDuration, audioPath, outputPath);
            Console.ReadLine();
        }

    }
}
