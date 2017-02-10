using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace FFXIVSB.Models
{
    public class SquadMember
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Int32 Physical { get; set; }
        public Int32 Mental { get; set; }
        public Int32 Tactical { get; set; }

        [JsonIgnore]
        [NotMapped]
        public SolidColorBrush DisplayColor { get; set; }

        public SquadMember()
        {
            DisplayColor = new SolidColorBrush(Colors.White);
        }

        public SquadMember(string name = "", Int32 phy = 0, Int32 mtl = 0, Int32 tcl = 0)
        {
            Name = name;
            Physical = phy;
            Mental = mtl;
            Tactical = tcl;
            DisplayColor = new SolidColorBrush(Colors.White);
        }
    }
}
