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
        public CardioWorkout(int Distance, DateTime Date, string Type, TimeSpan Duration, int CaloriesBurned, string Notes) : base(Date, Type, Duration, CaloriesBurned, Notes)
        {
            this.Distance = Distance;
            this.Date = Date;
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
