using Adam.Core.Maintenance;
using System;

namespace CoubMaintainanceJob
{
    public class CoubMaintainanceTarget : MaintenanceTarget
    {

        public CoubMaintainanceTarget(string audio, string video, double videoStart, double videoDuration)
        {
            VideoPath = video;
            AudioPath = audio;
            VideoDuration = videoDuration;
            VideoStart = videoStart;
        }

        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string AudioPath { get; private set; }
        public string VideoPath { get; private set; }
        public double VideoStart { get; private set; }
        public double VideoDuration { get; private set; }


        public override long Impact
        {
            get { return 50; }
        }
    }
}
