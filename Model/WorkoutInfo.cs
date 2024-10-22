using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class WorkoutInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public TimeSpan Duration { get; set; }
        public int Calories { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }   
}
