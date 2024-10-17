using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class StrengthWorkout : Workout
    {
        // ---------- Egenskaper ---------- //
        public int Repetitions {  get; set; }


        // ---------- Konstruktor ---------- //
        public StrengthWorkout(int Repetitions, string DateTime, string Type, string Duration, int CaloriesBurned, string Notes) : base(DateTime, Type, Duration, CaloriesBurned, Notes)
        {
            this.Repetitions = Repetitions;
            this.DateTime = DateTime;
            this.Type = Type;
            this.Duration = Duration;
            this.CaloriesBurned = CaloriesBurned;
            this.Notes = Notes;

        }


        // ------------------------------ Metoder ------------------------------ //
        public override int CalculateCaloriesBurned()
        {
            
        }
    }
}
