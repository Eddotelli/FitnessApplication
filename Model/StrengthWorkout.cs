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
            Random slump = new Random();

            //Slumpar mellan 8-15 kalorier per minut. //
            return Repetitions * slump.Next(6, 10);
        }
    }
}
