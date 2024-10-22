using FitTrack.Model;
using FitTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Egenskap för vald träningspass. //
        // Privat fält som lagrar den valda träningspasset. //
        private WorkoutInfo selectedItem;
        public WorkoutInfo SelectedItem
        {
            get { return selectedItem; } // Returnerar värdet av selectedItem. //
            set
            {
                selectedItem = value; // Sätter värdet av selectedItem till det nya värdet. //
                OnPropertyChanged(nameof(SelectedItem)); // Meddelar att SelectedItem har ändrats, så UI kan uppdateras. //
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
            LocalWorkoutsInfo = new ObservableCollection<WorkoutInfo>(); // Använd lokal lista istället.
        }


        // ------------------------------ Metoder ------------------------------ //

        // Lägger till "tom" rad för att fylla i ett träningspass, som sedan läggs till i Lokala workout-listan. //
        public void AddWorkout()
        {
            LocalWorkoutsInfo.Add(new WorkoutInfo { Name = "XXX", Type = "XXX", Duration = TimeSpan.FromSeconds(0), Calories = 0, Notes = "XXX", Date = DateTime.Now });
        }

        public void CancelWorkout()
        {
            if (SelectedItem != null) 
            {
                //var currentWorkout = new loc
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
