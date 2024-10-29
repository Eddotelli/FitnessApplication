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

        // Bunden till UserManager's WorkoutsInfo direkt för synkronisering. //
        public ObservableCollection<Workout> WorkoutsInfo => userManager.WorkoutsInfo;

        // ------------------------------ Egenskaper ------------------------------ //
        // Egenskap för vald träningspass. //
        // Privat fält som lagrar den valda träningspasset. //
        private Workout selectedItem;
        public Workout SelectedItem
        {
            get { return selectedItem; } // Returnerar värdet av selectedItem. //
            set
            {
                selectedItem = value; // Sätter värdet av selectedItem till det nya värdet. //
                OnPropertyChanged(nameof(SelectedItem)); // Meddelar att SelectedItem har ändrats, så UI kan uppdateras. //
            }
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand UserCommand => new RelayCommand(execute => UserDetails());
        public RelayCommand EditCommand => new RelayCommand(execute => AddWorkout());
        public RelayCommand RemoveCommand => new RelayCommand(execute => RemoveWorkout());
        public RelayCommand OpenCommand => new RelayCommand(execute => OpenDetails(SelectedItem));
  

        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som skapar en ny instans av UserManager. //
        public WorkoutsWindowViewModel()
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //
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
                userManager.WorkoutsInfo.Remove(selectedItem);
                SelectedItem = null; // Rensa SelectedItem för att förhindra referens till raderat objekt. //
            }
        }

        public void OpenDetails(Workout workout) 
        {
            // Öppnar upp WorkoutsDetailsWindow-fönstret. //
            if (workout != null)
            {
                WorkoutsDetailsWindow detailsWindow = new WorkoutsDetailsWindow(workout);
                detailsWindow.Show();
            } 
        }
    }
}
