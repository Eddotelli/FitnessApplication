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
        // Singleton-instans av UserManager, används för att hantera gemensam lista av träningspass mellan olika fönster. //
        private UserManager userManager;

        // Lokal lista för att hålla träningspass i AddWorkoutWindow. //
        public ObservableCollection<WorkoutInfo> LocalWorkoutsInfo { get; set; }


        // ------------------------------ Egenskaper ------------------------------ //
        public string Name {  get; set; }
        public string WorkoutTypeComboBox{ get; set; }
        public TimeSpan DurationInput { get; set; }
        public int CaloriesBurnedInput { get; set; }
        public string NotesInput { get; set; }

        // Egenskap för vald träningspass. //
        private WorkoutInfo selectedItem;
        public WorkoutInfo SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand AddCommand => new RelayCommand(execute => AddWorkout());
        public RelayCommand CancelCommand => new RelayCommand(execute => CancelWorkout());
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveWorkout());


        // ------------------------------ Konstruktor ------------------------------ //

        // Singleton-instansen av UserManager som hanterar gemensamma träningspass. //
        public AddWorkoutWindowViewModel()
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //

            LocalWorkoutsInfo = new ObservableCollection<WorkoutInfo>(); // Använd lokal lista istället.
        }


        // ------------------------------ Metoder ------------------------------ //
        public void AddWorkout()
        {
            LocalWorkoutsInfo.Add(new WorkoutInfo { Name = "XXX", Type = "XXX", Duration = TimeSpan.FromSeconds(0), Calories = 0, Notes = "XXX", Date = DateTime.Now });
        }

        public void CancelWorkout()
        {

        }
            
        public void SaveWorkout()
        {
            foreach (var workout in LocalWorkoutsInfo)
            {
                userManager.AddWorkout(workout);
            }
        }
    }
}
