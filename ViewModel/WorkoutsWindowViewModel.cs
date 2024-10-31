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
        // Singleton-instans av UserManager, används för att dela en gemensam lista och centraliserad datahantering mellan olika fönster. //
        private UserManager userManager;

        // Denna referens används för att kunna stänga eller kontrollera fönstret från ViewModel. //
        private readonly Window _workoutWindow;

        //// Denna egenskap binder till den inloggade användarens WorkoutsInfo och uppdateras automatiskt. //
        //public ObservableCollection<Workout> WorkoutsInfo => userManager.LoggedInUser?.UserWorkouts;

        // Egenskap för träningspass-listan, anpassad efter om en admin eller vanlig användare är inloggad.
        private ObservableCollection<Workout> _workoutsInfo;
        public ObservableCollection<Workout> WorkoutsInfo
        {
            get { return _workoutsInfo; }
            private set
            {
                _workoutsInfo = value;
                OnPropertyChanged(nameof(WorkoutsInfo));
            }
        }

        // ------------------------------ Egenskaper ------------------------------ //

        // Privat egenskap som lagrar den valda träningspasset. //
        private Workout selectedItem;
        public Workout SelectedItem
        {
            // Returnerar värdet av selectedItem. //
            get { return selectedItem; }
            set
            {
                // Sätter värdet av selectedItem till det nya värdet. //
                selectedItem = value;

                // Meddelar att SelectedItem har ändrats, så UI kan uppdateras. //
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        // Egenskap för att hantera popup-öppning. //
        private bool _isInfoPopupOpen;
        public bool IsInfoPopupOpen
        {
            get { return _isInfoPopupOpen; }
            set
            {
                _isInfoPopupOpen = value;

                // Meddelar UI om ändringen. //
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

        // Konstruktor som initierar WorkoutsWindowViewModel. //
        public WorkoutsWindowViewModel(Window workoutwindow)
        {
            // Hämtar Singleton-instansen av UserManager för att säkerställa att samma användar- och datahantering delas över hela applikationen. //
            userManager = UserManager.Instance;

            // Detta gör att ViewModel kan stänga fönstret när registreringen är klar. //
            _workoutWindow = workoutwindow;

            if (userManager.LoggedInUser is AdminUser)
            {
                // Om AdminUser är inloggad, visa alla träningspass.
                WorkoutsInfo = userManager.GetAllWorkouts();
            }
            else
            {
                // För vanliga användare, visa bara användarens egna träningspass.
                WorkoutsInfo = userManager.LoggedInUser?.UserWorkouts ?? new ObservableCollection<Workout>();
            }
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
            if (SelectedItem != null) 
            {
                if (userManager.LoggedInUser != null)
                {
                    userManager.LoggedInUser.UserWorkouts.Remove(SelectedItem);
                    SelectedItem = null;
                    OnPropertyChanged(nameof(WorkoutsInfo));
                }
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
                detailsWindow.Show();

                // Stänger ner WorkoutWindow-fönstret. //
                _workoutWindow.Close();
            }
            else
            {
                MessageBox.Show("Please select a workout first.");
            }
        }

        private void SignOut()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure yow wanna sign out?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Rensar inloggad användare. //
                userManager.LoggedInUser = null;

                // Öppnar upp MainWindow-fönstret. //
                MainWindow main = new MainWindow();
                main.Show();

                // Stänger ner WorkoutWindow-fönstret. //
                _workoutWindow.Close();
            }   
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
