using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class StrengthWorkout : Workout
    {
        // ------------------------------ Egenskaper ------------------------------ //
        public int Repetitions {  get; set; }


        // ------------------------------ Konstruktor ------------------------------ //
        public StrengthWorkout(int Repetitions, DateTime Date, string Type, TimeSpan Duration, int CaloriesBurned, string Notes) : base(Date, Type, Duration, CaloriesBurned, Notes)
        {
            this.Repetitions = Repetitions;
            this.Date = Date;
            this.Type = Type;
            this.Duration = Duration;
            this.CaloriesBurned = CaloriesBurned;
            this.Notes = Notes;

        }


        // ------------------------------ Metoder ------------------------------ //
        public override int CalculateCaloriesBurned()
        {
            return 0;
        }
    }
}
