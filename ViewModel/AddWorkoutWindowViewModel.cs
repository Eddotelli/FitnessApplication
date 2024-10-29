using FitTrack.Model;
using FitTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitTrack.ViewModel
{
    public class AddWorkoutWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att hantera gemensam lista mellan olika fönster. //
        private UserManager userManager;

        // Bunden till UserManager's WorkoutsInfo direkt för synkronisering med andra fönster. //
        public ObservableCollection<Workout> WorkoutsInfo => userManager.WorkoutsInfo;

        // Alternativ för ComboBox i kolumnen "Type". //
        public ObservableCollection<string> WorkoutTypes { get; set; }


        // ------------------------------ Egenskaper ------------------------------ //
        public string Name { get; set; }
        public string WorkoutTypeComboBox { get; set; } // Används för att välja mellan olika typer av Workout (t.ex. Strength eller Cardio). //
        public TimeSpan Duration { get; set; }
        public int CaloriesBurned { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public int Repetitions {  get; set; } // Endast relevant för StrengthWorkout. //
        public int Distance {  get; set; } // // Endast relevant för CardioWorkout. //

        // Privata egenskaper som original information sparas(kopieras) till. //
        private string originalName;
        private string originalWorkoutTypeComboBox;
        private TimeSpan originalDuration;
        private int originalCaloriesBurned;
        private string originalNotes;
        private DateTime originalDate;
        //private int originalRepetitions { get; set; }
        //private int originalDistance { get; set; } 

        // Egenskap för vald träningspass. //
        // Privat fält som lagrar den valda träningspasset (via datagrid?). //
        private Workout selectedItem;
        public Workout SelectedItem
        {
            get { return selectedItem; } // Returnerar värdet av selectedItem. //
            set
            {
                if (selectedItem != value) 
                {
                    selectedItem = value; // Sätter värdet av selectedItem till det nya värdet. //

                    // Spara ursprungliga värden när ett nytt objekt väljs. //
                    if (selectedItem != null) 
                    {
                        originalName = SelectedItem.Name;
                        originalWorkoutTypeComboBox = SelectedItem.TypeInput;
                        originalDuration = SelectedItem.Duration;
                        originalCaloriesBurned = SelectedItem.CaloriesBurned;
                        originalNotes = SelectedItem.Notes;
                        originalDate = SelectedItem.Date;
                    }

                    OnPropertyChanged(nameof(SelectedItem)); // Meddelar att SelectedItem har ändrats, så UI kan uppdateras. // 
                }      
            }
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand AddCommand => new RelayCommand(execute => AddWorkout());
        public RelayCommand CancelCommand => new RelayCommand(execute => CancelWorkout());
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveWorkout());
        public RelayCommand RemoveCommand => new RelayCommand(execute => RemoveWorkout());


        // ------------------------------ Konstruktor ------------------------------ //

        // Singleton-instansen av UserManager som hanterar gemensamma träningspass. //
        public AddWorkoutWindowViewModel()
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //

            // Lista över typer som visas i ComboBox för "Type"-kolumnen
            WorkoutTypes = new ObservableCollection<string>
            {
                "Strength", "Cardio"
            };
        }


        // ------------------------------ Metoder ------------------------------ //

        // Lägger till "tom" rad för att fylla i ett träningspass, som sedan läggs till i Lokala workout-listan. //
        public void AddWorkout()
        {
            // Kontrollera att användaren har valt ett WorkoutType. //
            if (string.IsNullOrEmpty(WorkoutTypeComboBox))
            {
                MessageBox.Show("Vänligen välj en typ av träningspass innan du lägger till det.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        
            Workout newWorkout;

            if (WorkoutTypeComboBox == "Strength")
            {
                // Om WorkoutTypeComboBox är "Strength" skapas en StrengthWorkout-instans. //
                newWorkout = new StrengthWorkout(Name, Repetitions, Date, "Strength", Duration, CaloriesBurned, Notes);
            }
            else
            {
                // Om WorkoutTypeComboBox inte är "Strength" (dvs. är "Cardio"), skapas en CardioWorkout-instans. //
                newWorkout = new CardioWorkout(Name, Distance, Date, "Cardio", Duration, CaloriesBurned, Notes);
            }

            UserManager.Instance.WorkoutsInfo.Add(newWorkout);
        }

        public void SaveWorkout()
        {
            MessageBox.Show("Träningspassen har sparats!", "Sparat", MessageBoxButton.OK, MessageBoxImage.Information);

            OnPropertyChanged(nameof(WorkoutsInfo)); // Uppdatera UI vid sparande
        }

        public void CancelWorkout() // <------------- Återkom och se över denna knappen!!
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to cancel and reset?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) 
            {
                if(result == MessageBoxResult.Yes && SelectedItem != null)
                {
                    SelectedItem.Name = "";
                    SelectedItem.TypeInput = "";
                    SelectedItem.Duration = TimeSpan.Zero;
                    SelectedItem.CaloriesBurned = 0;
                    SelectedItem.Notes = "";
                    SelectedItem.Date = DateTime.Now;
                }

            }
            else if (result == MessageBoxResult.No) 
            {
                // ------> Behövs det någon logik här? <------ //
            }   
        }

        // Tar bort vald träningspass från listan. //
        public void RemoveWorkout()
        {
            if (SelectedItem != null)
            {
                UserManager.Instance.WorkoutsInfo.Remove(SelectedItem);
                SelectedItem = null;
            }
        }
    }
}
