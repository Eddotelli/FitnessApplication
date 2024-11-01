using FitTrack.Model;
using FitTrack.MVVM;
using FitTrack.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// Add these at the top to use Debug
using System.Diagnostics;

namespace FitTrack.ViewModel
{
    public class AddWorkoutWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att dela en gemensam lista och centraliserad datahantering mellan olika fönster. //
        private UserManager userManager;

        // Denna referens används för att kunna stänga eller kontrollera fönstret från ViewModel. //
        private readonly Window _addWorkoutWindow;

        // Bindning till inloggade användarens WorkoutsInfo via UserManager för att hålla dem synkroniserade. //
        public ObservableCollection<Workout> WorkoutsInfo => userManager.LoggedInUser?.UserWorkouts;

        // Alternativ för ComboBox i kolumnen "Type". //
        public ObservableCollection<string> WorkoutTypes { get; set; }

        // Temporär lista som ska visas i `ListBox` på AddWorkoutWindow. //
        public ObservableCollection<Workout> TempWorkouts { get; private set; } = new ObservableCollection<Workout>();


        // ------------------------------ Egenskaper ------------------------------ //

        // Fält och egenskap för att lagra och hantera värdet på Name. //
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                Console.WriteLine("Hej!!");
                OnPropertyChanged(nameof(Name));  
            }
        }

        // Fält och egenskap för att välja mellan olika typer av Workout (t.ex. Strength eller Cardio). //
        private string workoutTypeComboBox;
        public string WorkoutTypeComboBox
        {
            get { return workoutTypeComboBox; }
            set
            {
                workoutTypeComboBox = value;
                //CanExecuteButtons = !string.IsNullOrEmpty(workoutTypeComboBox);
                OnPropertyChanged(nameof(WorkoutTypeComboBox));
                Debug.WriteLine($"WorkoutTypeComboBox set to: {workoutTypeComboBox}");
                UpdateCaloriesBurned();
            }
        }

        // Fält och egenskap för att lagra och hantera värdet på Duration. //
        private TimeSpan duration = TimeSpan.FromSeconds(0);
        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        // Fält och egenskap för att lagra och hantera värdet på CaloriesBurned. //
        private int caloriesBurned;
        public int CaloriesBurned
        {
            get { return caloriesBurned; }
            set
            {
                caloriesBurned = value;
                OnPropertyChanged(nameof(CaloriesBurned));
            }
        }

        // Fält och egenskap för att lagra och hantera värdet på Notes. //
        private string notes;
        public string Notes 
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged(nameof(CaloriesBurned));
            }
        }

        // Fält och egenskap för att lagra och hantera värdet på Date. //
        private DateTime date = DateTime.Now;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));

                }
            }
        }

        // Endast relevant för StrengthWorkout. //
        private int repetitions;
        public int Repetitions
        {
            get { return repetitions; }
            set
            {
                repetitions = value;
                OnPropertyChanged(nameof(Repetitions));
                Debug.WriteLine($"Repetitions set to: {repetitions}");
                UpdateCaloriesBurned();
            }
        }

        // Endast relevant för CardioWorkout. // 
        private int distance;
        public int Distance
        {
            get { return distance; }
            set
            {
                distance = value;
                OnPropertyChanged(nameof(Distance));
                Debug.WriteLine($"Distance set to: {distance}");
                UpdateCaloriesBurned();
            }
        }

        // Privata egenskaper som original information sparas(kopieras) till. //
        private string originalName;
        private string originalWorkoutTypeComboBox;
        private TimeSpan originalDuration;
        private int originalCaloriesBurned;
        private string originalNotes;
        private DateTime originalDate;

        private int originalRepetitions { get; set; } // Endast relevant för StrengthWorkout. //
        private int originalDistance { get; set; } // Endast relevant för CardioWorkout. //

        // Privat egenskap som lagrar den valda träningspasset. //
        private Workout selectedItem;
        public Workout SelectedItem
        {
            // Returnerar värdet av selectedItem. //
            get { return selectedItem; }
            set
            {
                if (selectedItem != value) 
                {
                    // Sätter värdet av selectedItem till det nya värdet. //
                    selectedItem = value;

                    // Meddelar att SelectedItem har ändrats, så UI kan uppdateras. //
                    OnPropertyChanged(nameof(SelectedItem)); 
                }      
            }
        }



        // ------------------------------ Bool-egenskaper ------------------------------ //

        //// Bool-egenskap för att aktivera/inaktivera knapparna. //
        //private bool canExecuteButtons;
        //public bool CanExecuteButtons
        //{
        //    get { return canExecuteButtons; }
        //    set
        //    {
        //        canExecuteButtons = value;
        //        OnPropertyChanged(nameof(CanExecuteButtons));
        //    }
        //}

        // Bool-egenskap för att aktivera 'Save'-knappen när alla textboxar är ifyllda. //
        private bool canSaveWorkout;
        public bool CanSaveWorkout
        {
            get { return canSaveWorkout; }
            private set
            {
                if (canSaveWorkout != value)
                {
                    canSaveWorkout = value;
                    OnPropertyChanged(nameof(CanSaveWorkout));
                }
            }
        } 

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand AddCommand => new RelayCommand(execute => AddWorkout());
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveWorkout());
        public RelayCommand RemoveCommand => new RelayCommand(execute => RemoveWorkout());
        public RelayCommand PasteCommand => new RelayCommand(execute => PasteWorkout());
        public RelayCommand ExitCommand => new RelayCommand(execute => Exit());


        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som initierar AddWorkoutWindowViewModel. //
        public AddWorkoutWindowViewModel(Window addWorkoutWindow)
        {
            // Hämtar Singleton-instansen av UserManager för att säkerställa att samma användar- och datahantering delas över hela applikationen. //
            userManager = UserManager.Instance;

            // Detta gör att ViewModel kan stänga fönstret när användaren är klar. //
            _addWorkoutWindow = addWorkoutWindow;

            // Lista över typer som visas i ComboBox för "Type"-kolumnen. //
            WorkoutTypes = new ObservableCollection<string>
            {
                "Strength", "Cardio"
            };

            // Sätter CanExecuteKnappar till false för att inaktivera knapparna vid start. //
            //CanExecuteButtons = false;

            
        }


        // ------------------------------ Metoder ------------------------------ //

        // Lägger till träningspass i den temporära listan. //
        public void AddWorkout()
        {
            // Skapar ett nytt träningspass baserat på vald WorkoutTypeComboBox. //
            Workout newWorkout;
            if (WorkoutTypeComboBox == "Strength")
            {  
                // Om WorkoutTypeComboBox är "Strength" skapas en StrengthWorkout-instans. //
                newWorkout = new StrengthWorkout(Name, Repetitions, Date, "Strength", Duration, CaloriesBurned, Notes);
                CaloriesBurned = newWorkout.CalculateCaloriesBurned();
            }
            else
            {          
                // Om WorkoutTypeComboBox är "Cardio" skapas en CardioWorkout-instans. //
                newWorkout = new CardioWorkout(Name, Distance, Date, "Cardio", Duration, CaloriesBurned, Notes);
                UpdateCaloriesBurned();
            }

            // Lägg till träningspasset i den temporära listan. //
            TempWorkouts.Add(newWorkout);
            OnPropertyChanged(nameof(TempWorkouts));
        }

        // Vid spara, överför träningspass från temporära listan till användarens permanenta lista. //
        public void SaveWorkout()
        {
            // Om man försöker spara när listan är tom. // <--- Återkom till denna, ska egentligen inte behövas.
            if (TempWorkouts.Count == 0)
            {
                MessageBox.Show("No workouts to save. Add workouts before saving.", "Save", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Please enter a workout name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(WorkoutTypeComboBox))
            {
                MessageBox.Show("Please select a workout type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Repetitions <= 0)
            {
                MessageBox.Show("Please enter a valid number of repetitions. It must be greater than zero.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Distance <= 0)
            {
                MessageBox.Show("Please enter a valid distance. It must be greater than zero.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CaloriesBurned <= 0)
            {
                MessageBox.Show("Please enter a valid number of calories burned. It must be greater than zero.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(Notes))
            {
                MessageBox.Show("Please add notes about the workout.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Kontrollerar så att en användare är inloggad. //
            if (userManager.LoggedInUser != null)
            {
                // Loopar igenom temporära listan. //
                foreach (var workout in TempWorkouts)
                {
                    // Lägger till temporära träningslistan i användarens egna lista. //
                    userManager.LoggedInUser.UserWorkouts.Add(workout);
                }

                MessageBox.Show("All workouts have been saved!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                // Uppdaterar UI. //
                OnPropertyChanged(nameof(TempWorkouts));

                // Rensa den temporära listan efter sparande. //
                TempWorkouts.Clear(); 

                // Öppnar WorkoutsWindow. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();

                // Stänger AddWorkoutWindow. //
                _addWorkoutWindow.Close();
            }
        }

        // Tar bort vald träningspass från listan. //
        public void RemoveWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes && SelectedItem != null)
            {
                // Ta bort från den temporära listan istället för UserWorkouts. //
                TempWorkouts.Remove(SelectedItem);
                SelectedItem = null;

                // Uppdatera UI om listan ändras. //
                OnPropertyChanged(nameof(TempWorkouts));
            }
        }

        // Kopierar in nersparad träningspass som mall från WorkoutsDetailsWindow. //
        public void PasteWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            Workout newWorkout = null;

            if (result == MessageBoxResult.Yes)
            {
                Console.WriteLine($"Current workout!!");

                
                if (userManager.CopiedWorkout is StrengthWorkout)
                {
                    
                    // Om WorkoutTypeComboBox är "Strength" skapas en StrengthWorkout-instans. //
                    newWorkout = new StrengthWorkout(userManager.CopiedWorkout.Name, Repetitions, userManager.CopiedWorkout.Date, "Strength", Duration, userManager.CopiedWorkout.CaloriesBurned, userManager.CopiedWorkout.Notes);
                }
                else if(userManager.CopiedWorkout is CardioWorkout)
                {

                    // Om WorkoutTypeComboBox är "Cardio" skapas en CardioWorkout-instans. //
                    newWorkout = new CardioWorkout(userManager.CopiedWorkout.Name, Distance, userManager.CopiedWorkout.Date, "Cardio", Duration, userManager.CopiedWorkout.CaloriesBurned, userManager.CopiedWorkout.Notes);
                    UpdateCaloriesBurned();

                }
                else
                {
                    MessageBox.Show("No workout to paste.");
                }

            }

            TempWorkouts.Add(newWorkout);
            
        }

        // Metod för att avbryta och gå tillbaka till WorkoutsWindow-fönstret. //
        public void Exit()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit and go back?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Rensa temporära träningspass. //
                TempWorkouts.Clear();

                // Öppnar WorkoutsWindow-fönstret. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();

                // Stänger AddWorkoutWindow-fönstret. //
                _addWorkoutWindow.Close();
            }
        }

        private void UpdateCaloriesBurned()
        {
            // Debug statement to confirm this method is being called
            Debug.WriteLine("UpdateCaloriesBurned called");

            if (WorkoutTypeComboBox == "Strength" || Repetitions > 0)
            {
                var strengthWorkout = new StrengthWorkout(Name, Repetitions, Date, "Strength", Duration, 0, Notes);
                CaloriesBurned = strengthWorkout.CalculateCaloriesBurned();
                Debug.WriteLine($"Calculated Calories for Strength: {CaloriesBurned}");
            }
            else if (WorkoutTypeComboBox == "Strength" || Distance > 0)
            {
                var cardioWorkout = new CardioWorkout(Name, Distance, Date, "Cardio", Duration, 0, Notes);
                CaloriesBurned = cardioWorkout.CalculateCaloriesBurned();
                Debug.WriteLine($"Calculated Calories for Cardio: {CaloriesBurned}");
            }
            else
            {
                CaloriesBurned = 0; // Reset if conditions aren't met

                Debug.WriteLine("Calories reset to 0");
            }
            OnPropertyChanged(nameof(CaloriesBurned)); // Notify change after calculation
        }
    }
}
