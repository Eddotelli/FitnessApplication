using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public abstract class Workout
    {
        // ---------- Egenskaper ---------- //
        public string DateTime {  get; set; }
        public string Type { get; set; }
        public string Duration {  get; set; }
        public int CaloriesBurned {  get; set; }
        public string Notes {  get; set; }

        // ---------- Konstruktor ---------- //
        public Workout(string DateTime, string Type, string Duration, int CaloriesBurned, string Notes)
        {
            this.DateTime = DateTime;
            this.Type = Type;
            this.Duration = Duration;
            this.CaloriesBurned = CaloriesBurned;
            this.Notes = Notes;
        }


        // ------------------------------ Metoder ------------------------------ //
        public abstract int CalculateCaloriesBurned();
    }
}
