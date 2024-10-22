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
        // Singleton-instans av UserManager, används för att hantera gemensam lista mellan olika fönster. //
        private UserManager userManager;

        // Privat variabel som håller träningsinformation. //
        private ObservableCollection<WorkoutInfo> workoutsInfo;
        //Public egenskap som ger kontrollerad åtkomst till träningsinformationen. //
        public ObservableCollection<WorkoutInfo> WorkoutsInfo
        {
            get { return workoutsInfo; } // Returnerar den privata listan med träningsinformation. //
            set
            {
                // Sätter det nya värdet för träningsinformationen. //
                workoutsInfo = value;
                //Berättar för UI om att WorkoutsInfo har ändrats. //
                OnPropertyChanged(nameof(WorkoutsInfo));
            }
        }


        // ------------------------------ Egenskaper ------------------------------ //


        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand UserCommand => new RelayCommand(execute => UserDetails());
        public RelayCommand EditCommand => new RelayCommand(execute => AddWorkout());
        public RelayCommand RemoveCommand => new RelayCommand(execute => RemoveWorkout());
        //public RelayCommand OpenCommand => new RelayCommand(execute => OpenDetails());

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

        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som skapar en ny instans av UserManager. //
        public WorkoutsWindowViewModel()
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //
            workoutsInfo = userManager.WorkoutsInfo; // Länkar direkt till listan från UserManager. //
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
