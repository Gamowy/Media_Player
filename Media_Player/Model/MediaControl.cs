using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.Model
{
    public class MediaControl
    {
        public bool IsPlaying { get; set; }
        public int Progress { get; set; }
        public int Duration { get; set; }
        public Uri? MediaUri { get; set; }
        public MediaControl() 
        {
            IsPlaying = false;
            //MediaUri = new Uri("E:/Video/Avseq04.mp4");
        }
        
    }
}
