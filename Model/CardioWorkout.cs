using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class CardioWorkout : Workout
    {
        // ------------------------------ Egenskaper ------------------------------ //
        public int Distance { get; set; }

        // ------------------------------ Konstruktor ------------------------------ //
        public CardioWorkout(string Name, int Distance, DateTime Date, string TypeInput, TimeSpan Duration, int CaloriesBurned, string Notes) : base(Name, Date, TypeInput, Duration, CaloriesBurned, Notes)
        {          
            this.Distance = Distance;           
        }

        // ------------------------------ Metoder ------------------------------ //
        public override int CalculateCaloriesBurned()
        {
            Random slump = new Random();

            //Slumpar mellan 8-15 kalorier per minut. //
            return (int)Distance * (slump.Next(8, 16));
        }
    }
}
