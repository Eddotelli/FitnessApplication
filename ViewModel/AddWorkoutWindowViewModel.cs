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
        public string NameInput { get; set; }
        public string WorkoutTypeComboBoxInput { get; set; }
        public TimeSpan DurationInput { get; set; }
        public int CaloriesBurnedInput { get; set; }
        public string NotesInput { get; set; }
        public DateTime DateInput { get; set; }

        // Privata egenskaper som original information sparas(kopieras) till. //
        private string originalNameInput;
        private string originalWorkoutTypeComboBoxInput;
        private TimeSpan originalDurationInput;
        private int originalCaloriesBurnedInput;
        private string originalNotesInput;
        private DateTime originalDateInput;

        // Egenskap för vald träningspass. //
        // Privat fält som lagrar den valda träningspasset (via datagrid?). //
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
                        originalNameInput = SelectedItem.NameInput;
                        originalWorkoutTypeComboBoxInput = SelectedItem.TypeInput;
                        originalDurationInput = SelectedItem.DurationInput;
                        originalCaloriesBurnedInput = SelectedItem.CaloriesBurnedInput;
                        originalNotesInput = SelectedItem.NotesInput;
                        originalDateInput = SelectedItem.DateInput;
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
            LocalWorkoutsInfo.Add(new WorkoutInfo { NameInput = "", TypeInput = "", DurationInput = TimeSpan.FromSeconds(0), CaloriesBurnedInput = 0, NotesInput = "", DateInput = DateTime.Now });
        }

        public void SaveWorkout()
        {

            try
            {
                // Skapa en kontroll som kontrollerar om träningspasset redan finns i listan. //

                //Loppar igenom varje träningspass i listan och sparar ner i instans-listan som finns i UserManager. // <--- Återkom och förstå denna bättre.
                // --- Förbättrad version? --- //
                foreach (var workout in LocalWorkoutsInfo)
                {
                    //// Validering/kontroll för inmatningen. //
                    //if (string.IsNullOrWhiteSpace(workout.NameInput) || string.IsNullOrWhiteSpace(workout.TypeInput) || workout.DurationInput == null ||
                    //    workout.CaloriesBurnedInput < 0 || // Kontrollera att kalorier är ett positivt värde
                    //    workout.DateInput == default) // Kontrollera att ett giltigt datum har valts
                    //{
                    //    MessageBox.Show("Please ensure all fields are filled correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //    return; // Avbryt om validering misslyckas
                    //}

                    if (!userManager.WorkoutsInfo.Contains(workout))
                    {
                        userManager.AddWorkout(workout);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the workout: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            // Uppdatera listan så att träningspassen är synkroniserade.
            //userManager.WorkoutsInfo = new ObservableCollection<WorkoutInfo>(LocalWorkoutsInfo);
        }

        public void CancelWorkout() // <------------- Återkom och se över denna knappen!!
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to cancel and reset?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) 
            {
                if (SelectedItem != null)
                {
                    // Återställ ursprungliga värden. // <---- Be om hjälp till denna.
                    SelectedItem.NameInput = originalNameInput;
                    SelectedItem.TypeInput = originalWorkoutTypeComboBoxInput;
                    SelectedItem.DurationInput = originalDurationInput;
                    SelectedItem.CaloriesBurnedInput = originalCaloriesBurnedInput;
                    SelectedItem.NotesInput = originalNotesInput;
                    SelectedItem.DateInput = originalDateInput;

                    // Skapar en temporär kopia för att trigga ändringarna i UI. //
                    var tempItem = SelectedItem;
                    SelectedItem = null;
                    OnPropertyChanged(nameof(SelectedItem));

                    // Återställer till den ursprungliga vald objekt så att UI uppdateras korrekt. //
                    SelectedItem = tempItem;
                    OnPropertyChanged(nameof(SelectedItem));

                    // Notifiera UI om att värdena har ändrats. //
                    //OnPropertyChanged(nameof(selectedItem.NameInput));
                    //OnPropertyChanged(nameof(selectedItem.TypeInput));
                    //OnPropertyChanged(nameof(selectedItem.DurationInput));
                    //OnPropertyChanged(nameof(selectedItem.CaloriesBurnedInput));
                    //OnPropertyChanged(nameof(selectedItem.NotesInput));
                    //OnPropertyChanged(nameof(selectedItem.DateInput));
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
            if (selectedItem != null && LocalWorkoutsInfo.Contains(selectedItem))
            {
                // Ta bort träningspass från LocalWorkoutsInfo. //
                LocalWorkoutsInfo.Remove(selectedItem);

                // Ta även bort från UserManager:s lista, om den finns där. //
                if (userManager.WorkoutsInfo.Contains(selectedItem))
                {
                    userManager.WorkoutsInfo.Remove(selectedItem);
                }
            }
        }
    }
}
