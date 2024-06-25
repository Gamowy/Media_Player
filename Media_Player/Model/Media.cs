using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.Model
{
    public class Media
    {
        public string Title { get; set; }

        public string Location { get; set; }

        public Media(string location) 
        {
            this.Title = Path.GetFileNameWithoutExtension(location);
            this.Location = location;
        }
        
    }
}
