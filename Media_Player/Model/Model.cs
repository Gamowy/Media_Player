using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.Model
{
    public class Model
    {
        public MediaControl MediaControl { get; set; }
        public Model() 
        { 
            MediaControl=new MediaControl();
        }

    }
}
