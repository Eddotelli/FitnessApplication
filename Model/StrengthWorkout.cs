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
        public StrengthWorkout(string Name, int Repetitions, DateTime Date, string TypeInput, TimeSpan Duration, int CaloriesBurned, string Notes) : base(Name, Date, TypeInput, Duration, CaloriesBurned, Notes)
        {           
            this.Repetitions = Repetitions;
        }

        // ------------------------------ Metoder ------------------------------ //
        public override int CalculateCaloriesBurned()
        {
            // Exempelberäkning (anpassa enligt behov)
            return (int)(Duration.TotalMinutes * 8); // Exempel: 8 kalorier per minut
        }
    }
}
