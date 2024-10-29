using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public abstract class Workout
    {
        // Lista med träningspass-typer för ComboBox
        public static List<string> AvailableWorkoutTypes => new List<string> { "Strength", "Cardio" };

        // ------------------------------ Egenskaper ------------------------------ //
        public string Name { get; set; }
        public DateTime Date {  get; set; }
        public string TypeInput { get; set; }
        public TimeSpan Duration {  get; set; }
        public int CaloriesBurned {  get; set; }
        public string Notes {  get; set; }

        // ------------------------------ Konstruktor ------------------------------ //
        public Workout(string Name, DateTime Date, string TypeInput, TimeSpan Duration, int CaloriesBurned, string Notes)
        {
            this.Name = Name;
            this.Date = Date;
            this.TypeInput = TypeInput;
            this.Duration = Duration;
            this.CaloriesBurned = CaloriesBurned;
            this.Notes = Notes;
        }
        // ------------------------------ Metoder ------------------------------ //
        public abstract int CalculateCaloriesBurned();
    }
}
