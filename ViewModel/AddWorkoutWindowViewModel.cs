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


        // ------------------------------ Egenskaper ------------------------------ //
        public string Name { get; set; }
        public string WorkoutTypeComboBox { get; set; } // Används för att välja mellan olika typer av Workout (t.ex. Strength eller Cardio). //
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0);
        public int CaloriesBurned { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public int Repetitions {  get; set; } // Endast relevant för StrengthWorkout. //
        public int Distance {  get; set; } // Endast relevant för CardioWorkout. //

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

                    // Sparar ner ursprungliga värden när ett nytt objekt väljs. //
                    if (selectedItem != null) 
                    {
                        originalName = SelectedItem.Name;
                        originalWorkoutTypeComboBox = SelectedItem.TypeInput;
                        originalDuration = SelectedItem.Duration;
                        originalCaloriesBurned = SelectedItem.CaloriesBurned;
                        originalNotes = SelectedItem.Notes;
                        originalDate = SelectedItem.Date;
                    }

                    // Meddelar att SelectedItem har ändrats, så UI kan uppdateras. //
                    OnPropertyChanged(nameof(SelectedItem)); 
                }      
            }
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand AddCommand => new RelayCommand(execute => AddWorkout());
        public RelayCommand RestoreCommand => new RelayCommand(execute => RestoreWorkout());
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
        }


        // ------------------------------ Metoder ------------------------------ //

        // Lägger till "tom" rad för att fylla i ett träningspass, som sedan läggs till i användarens workout-lista. //
        public void AddWorkout()
        {
            // Kontrollera att användaren har valt ett WorkoutType. //
            if (string.IsNullOrEmpty(WorkoutTypeComboBox))
            {
                MessageBox.Show("Vänligen välj en typ av träningspass innan du lägger till det.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Skapar ett nytt träningspass baserat på vald WorkoutTypeComboBox. //
            Workout newWorkout;
            if (WorkoutTypeComboBox == "Strength")
            {
                // Om WorkoutTypeComboBox är "Strength" skapas en StrengthWorkout-instans. //
                newWorkout = new StrengthWorkout(Name, Repetitions, Date, "Strength", Duration, CaloriesBurned, Notes);
            }
            else
            {
                // Om WorkoutTypeComboBox inte är "Strength" (dvs. är "Cardio"), skapas en CardioWorkout-instans och beräknar kalorier baserat på distansen. //
                //var cardioWorkout = new CardioWorkout(Name, Distance, Date, "Cardio", Duration, 0, Notes);
                //cardioWorkout.CaloriesBurned = cardioWorkout.CalculateCaloriesBurned();

                newWorkout = new CardioWorkout(Name, Distance, Date, "Cardio", Duration, CaloriesBurned, Notes);
                //newWorkout = cardioWorkout;
            }

            // Lägg till det nya träningspasset i den inloggade användarens lista. //
            if (userManager.LoggedInUser != null)
            {
                userManager.LoggedInUser.UserWorkouts.Add(newWorkout);

                // Uppdaterar UI. //
                OnPropertyChanged(nameof(WorkoutsInfo));
            }

        }

        // Sparar ner tillagda träningspas stänger ner AddWorkoutsWindow och återgår sedan till WorkoutsWindow. //
        public void SaveWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to save and go back?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("The training sessions have been saved!", "Saved!", MessageBoxButton.OK, MessageBoxImage.Information);

                // Uppdaterar UI vid sparande. //
                OnPropertyChanged(nameof(WorkoutsInfo));

                // Öppnar upp WorkoutsWindow-fönstret. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();

                // Stänger ner AddWorkoutWindow-fönstret. //
                _addWorkoutWindow.Close();
            }
        }

        // Återställer informationen som senast matades in. //
        public void RestoreWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to cancel and reset?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) 
            {
                // Ersätter valda träningspasset med sin senaste information. //
                if(result == MessageBoxResult.Yes && SelectedItem != null)
                {
                    SelectedItem.Name = originalName;
                    SelectedItem.TypeInput = originalWorkoutTypeComboBox;
                    SelectedItem.Duration = originalDuration;
                    SelectedItem.CaloriesBurned = originalCaloriesBurned;
                    SelectedItem.Notes = originalNotes;
                    SelectedItem.Date = originalDate;
                }

            }
        }

        // Tar bort vald träningspass från listan. //
        public void RemoveWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (SelectedItem != null)
                {
                    userManager.LoggedInUser?.UserWorkouts.Remove(SelectedItem);
                    SelectedItem = null;
                }
            }         
        }

        // Kopierar in nersparad träningspass som mall från WorkoutsDetailsWindow. //
        public void PasteWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (userManager.CopiedWorkout != null)
                {
                    // Fyller input raderna med kopierade informationen från ett pass. //
                    Name = userManager.CopiedWorkout.Name;
                    WorkoutTypeComboBox = userManager.CopiedWorkout.TypeInput;
                    Duration = userManager.CopiedWorkout.Duration;
                    CaloriesBurned = userManager.CopiedWorkout.CaloriesBurned;
                    Notes = userManager.CopiedWorkout.Notes;
                    Date = userManager.CopiedWorkout.Date;

                    // Om det kopierade träningspasset är av typen StrengthWorkout. //
                    if (userManager.CopiedWorkout is StrengthWorkout strengthWorkout)
                    {
                        Repetitions = strengthWorkout.Repetitions;
                        WorkoutTypeComboBox = "Strength";
                    }
                    // Om det kopierade träningspasset är av typen CardioWorkout. //
                    else if (userManager.CopiedWorkout is CardioWorkout cardioWorkout)
                    {
                        Distance = cardioWorkout.Distance;
                        WorkoutTypeComboBox = "Cardio";
                    }

                    MessageBox.Show("Workout pasted successfully!");
                }
                else
                {
                    MessageBox.Show("No workout to paste.");
                }
            } 
        }

        // Metod för att avbryta och gå tillbaka till WorkoutsWindow-fönstret. //
        public void Exit()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to exit and go back?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {   
                // Öppnar upp WorkoutsWindow-fönstret. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();

                // Stänger ner AddWorkoutWindow-fönstret. //
                _addWorkoutWindow.Close();
            }
        }
    }
}
