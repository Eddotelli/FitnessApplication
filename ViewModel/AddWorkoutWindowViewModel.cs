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

        // Lokal lista för att hålla träningspass i AddWorkoutWindow. //
        public ObservableCollection<WorkoutInfo> LocalWorkoutsInfo { get; set; }


        // ------------------------------ Egenskaper ------------------------------ //
        public string Name { get; set; }
        public string WorkoutTypeComboBox { get; set; }
        public TimeSpan DurationInput { get; set; }
        public int CaloriesBurnedInput { get; set; }
        public string NotesInput { get; set; }

        // Privata egenskaper som original information sparas(kopieras) till. //
        private string originalName;
        private string originalWorkoutType;
        private TimeSpan originalDuration;
        private int originalCaloriesBurned;
        private string originalNotes;

        // Egenskap för vald träningspass. //
        // Privat fält som lagrar den valda träningspasset. //
        private WorkoutInfo selectedItem;
        public WorkoutInfo SelectedItem
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
                        originalName = selectedItem.Name;
                        originalWorkoutType = selectedItem.Type;
                        originalDuration = selectedItem.Duration;
                        originalCaloriesBurned = selectedItem.Calories;
                        originalNotes = selectedItem.Notes;
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
            LocalWorkoutsInfo = new ObservableCollection<WorkoutInfo>(); // Använda lokala listan. //
            
        }


        // ------------------------------ Metoder ------------------------------ //

        // Lägger till "tom" rad för att fylla i ett träningspass, som sedan läggs till i Lokala workout-listan. //
        public void AddWorkout()
        {
            LocalWorkoutsInfo.Add(new WorkoutInfo { Name = "", Type = "", Duration = TimeSpan.FromSeconds(0), Calories = 0, Notes = "", Date = DateTime.Now });
        }

        public void CancelWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to cancel and reset?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) 
            {
                if (SelectedItem != null)
                {
                    // Återställ ursprungliga värden. // <---- Be om hjälp till denna.
                    selectedItem.Name = originalName;
                    selectedItem.Type = originalWorkoutType;
                    selectedItem.Duration = originalDuration;
                    selectedItem.Calories = originalCaloriesBurned;
                    selectedItem.Notes = originalNotes;

                    // Skapar en temporär kopia för att trigga ändringarna i UI. //
                    var tempItem = SelectedItem;
                    SelectedItem = null;
                    OnPropertyChanged(nameof(SelectedItem));

                    // Återställ till den ursprungliga vald objekt så att UI uppdateras korrekt. //
                    SelectedItem = tempItem;
                    OnPropertyChanged(nameof(SelectedItem));


                    // Notifiera UI om att värdena har ändrats
                    //OnPropertyChanged(nameof(SelectedItem));
                    //OnPropertyChanged(nameof(SelectedItem.Name));
                    //OnPropertyChanged(nameof(SelectedItem.Type));
                    //OnPropertyChanged(nameof(SelectedItem.Duration));
                    //OnPropertyChanged(nameof(SelectedItem.Notes));
                    //OnPropertyChanged(nameof(SelectedItem.Date)); // Alternativet med OnPropertyChanged.
                }
            }
            else if (result == MessageBoxResult.No) 
            {
                
            }   
        }

        public void SaveWorkout()
        {

            // Skapa en kontroll som kontrollerar om träningspasset redan finns i listan. //

            //Loppar igenom varje träningspass i listan och sparar ner i instans-listan som finns i UserManager.
            foreach (var workout in LocalWorkoutsInfo)
            {
                userManager.AddWorkout(workout);
            }
        }

        // Tar bort vald träningspass från listan. //
        public void RemoveWorkout()
        {
            if (selectedItem != null)
            {
                LocalWorkoutsInfo.Remove(selectedItem);
            }
        }
    }
}
