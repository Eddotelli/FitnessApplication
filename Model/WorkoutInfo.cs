using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class WorkoutInfo
    {
        // ------------------------------ Egenskaper ------------------------------ //
        public string NameInput { get; set; }
        public string TypeInput { get; set; } // <--- Återkom och gör om till ComboBox.
        public TimeSpan DurationInput { get; set; }
        public int CaloriesBurnedInput { get; set; }
        public string NotesInput { get; set; }
        public DateTime DateInput { get; set; }
    }   
}
