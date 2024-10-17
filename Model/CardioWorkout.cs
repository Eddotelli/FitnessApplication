using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class CardioWorkout : Workout
    {
        // ---------- Egenskaper ---------- //
        public int Distance { get; set; }

        // ---------- Konstruktor ---------- //
        public CardioWorkout(int Distance, string DateTime, string Type, string Duration, int CaloriesBurned, string Notes) : base(DateTime, Type, Duration, CaloriesBurned, Notes)
        {
            this.Distance = Distance;
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
