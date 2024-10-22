using FitTrack.Model;
using FitTrack.MVVM;
using FitTrack.View;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace FitTrack.ViewModel
{
    public class WorkoutsWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att hantera gemensam lista av träningspass mellan olika fönster. //
        private UserManager userManager;

        // Binda till den gemensamma listan med träningspass med info från UserManager. //
        private ObservableCollection<WorkoutInfo> workoutsInfo;
        public ObservableCollection<WorkoutInfo> WorkoutsInfo
        {
            get { return workoutsInfo; }
            set
            {
                workoutsInfo = value;
                OnPropertyChanged(nameof(WorkoutsInfo));
            }
        }


        // ------------------------------ Egenskaper ------------------------------ //
        public User User{ get; set; }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand UserCommand => new RelayCommand(execute => UserDetails());
        public RelayCommand EditCommand => new RelayCommand(execute => AddWorkout());
        public RelayCommand RemoveCommand => new RelayCommand(execute => RemoveWorkout());
        //public RelayCommand OpenCommand => new RelayCommand(execute => OpenDetails());

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

        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som skapar en ny instans av UserManager. //
        public WorkoutsWindowViewModel()
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //
            workoutsInfo = userManager.WorkoutsInfo; // Länka direkt till listan från UserManager. //
        }

        // ------------------------------ Metoder ------------------------------ //
        public void UserDetails()
        {
            // Öppnar upp UserDetailsWindow-fönstret. //
            UserDetailsWindow user = new UserDetailsWindow();
            user.Show();
        }
        
        public void AddWorkout()
        {
            // Öppnar upp AddWorkoutWindow-fönstret. //
            AddWorkoutWindow add = new AddWorkoutWindow();
            add.Show();
        }

        public void RemoveWorkout()
        {
            // Tar bort vald träningspass från listan. //
            if (selectedItem != null) 
            {
                workoutsInfo.Remove(selectedItem);
            }
        }

        public void OpenDetails(Workout workout) 
        {
            // Öppnar upp WorkoutsDetailsWindow-fönstret. //
            if (selectedItem != null)
            {
                WorkoutsDetailsWindow work = new WorkoutsDetailsWindow();
                work.Show();
            } 
        }
    }
}
