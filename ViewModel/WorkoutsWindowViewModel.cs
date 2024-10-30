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

        // Denna referens används för att kunna stänga eller kontrollera fönstret från ViewModel.
        private readonly Window _workoutWindow;

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

        // Egenskap för att hantera popup-öppning
        private bool _isInfoPopupOpen;
        public bool IsInfoPopupOpen
        {
            get => _isInfoPopupOpen;
            set
            {
                _isInfoPopupOpen = value;
                OnPropertyChanged(nameof(IsInfoPopupOpen));
            }
        }

        // --- TEST --- //
        // Egenskap för att visa användarnamnet
        public string LoggedInUsername
        {
            get
            {
                if (userManager.LoggedInUser != null)
                {
                    // Om det finns en inloggad användare, returnera användarnamnet
                    return userManager.LoggedInUser.Username;
                }
                else
                {
                    // Om ingen användare är inloggad, returnera "Guest"
                    return "Guest";
                }
            }
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand UserCommand => new RelayCommand(execute => UserDetails());
        public RelayCommand EditCommand => new RelayCommand(execute => AddWorkout());
        public RelayCommand RemoveCommand => new RelayCommand(execute => RemoveWorkout());
        public RelayCommand OpenCommand => new RelayCommand(execute => OpenDetails(SelectedItem));
        public RelayCommand SignOutCommand => new RelayCommand(execute => SignOut());
        public RelayCommand InfoPopupCommand => new RelayCommand(execute => InfoPopup());


        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som skapar en ny instans av UserManager. //
        public WorkoutsWindowViewModel(Window workoutwindow)
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //

            _workoutWindow = workoutwindow; // Detta gör att ViewModel kan stänga fönstret när registreringen är klar. //
        }

        // ------------------------------ Metoder ------------------------------ //
        public void UserDetails()
        {
            // Öppnar upp UserDetailsWindow-fönstret. //
            UserDetailsWindow user = new UserDetailsWindow();
            user.Show();

            // Stänger ner WorkoutWindow-fönstret. //
            _workoutWindow.Close();
        }
        
        public void AddWorkout()
        {
            // Öppnar upp AddWorkoutWindow-fönstret. //
            AddWorkoutWindow add = new AddWorkoutWindow();
            add.Show();

            // Stänger ner WorkoutWindow-fönstret. //
            _workoutWindow.Close();
        }

        public void RemoveWorkout()
        {
            // Tar bort vald träningspass från listan. //
            if (selectedItem != null) 
            {
                userManager.WorkoutsInfo.Remove(selectedItem);
                SelectedItem = null; // Rensa SelectedItem för att förhindra referens till raderat objekt. //
            }
            else
            {
                MessageBox.Show("Please select a workout first.");
            }
        }

        public void OpenDetails(Workout workout) 
        {
            // Öppnar upp WorkoutsDetailsWindow-fönstret. //
            if (workout != null)
            {
                WorkoutsDetailsWindow detailsWindow = new WorkoutsDetailsWindow(workout);
                //WorkoutsDetailsWindow detailsWindow = new WorkoutsDetailsWindow();
                //detailsWindow.Show();

                // Stänger ner WorkoutWindow-fönstret. //
                //_workoutWindow.Close();
            }
            else
            {
                MessageBox.Show("Please select a workout first.");
            }
        }

        private void SignOut()
        {
            userManager.LoggedInUser = null;

            // Öppnar upp MainWindow-fönstret. //
            MainWindow main = new MainWindow();
            main.Show();

            // Stänger ner WorkoutWindow-fönstret. //
            _workoutWindow.Close();
        }

        // Metod för att uppdatera användarnamnet i UI vid ändringar
        public void UpdateLoggedInUser()
        {
            OnPropertyChanged(nameof(LoggedInUsername));
        }

        // Kommando för att öppna popupen
        public void InfoPopup()
        {
            IsInfoPopupOpen = !IsInfoPopupOpen;
        }
    }
}
