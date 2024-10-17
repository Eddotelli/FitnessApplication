using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.ViewModel
{
    public class AddWorkoutWindowViewModel
    {
        // ---------- Egenskaper ---------- //
        public string WorkoutTypeComboBox{ get; set; }
        public TimeSpan DurationInput { get; set; }
        public int CaloriesBurnedInput { get; set; }
        public string NotesInput { get; set; }

        // ---------- Konstruktor ---------- //

        // ------------------------------ Metoder ------------------------------ //
        public void SaveWorkout()
        {

        }
    }
}
