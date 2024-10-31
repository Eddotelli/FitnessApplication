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
    public class UserDetailsWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att hantera gemensam lista mellan olika fönster. //
        private UserManager userManager;

        private readonly Window _userDetailsWindow;

        // Alternativ för ComboBox i kolumnen "Type". //
        public ObservableCollection<string> WorkoutTypes { get; set; }

        // ------------------------------ Egenskaper ------------------------------ //
        private string orignalUsernameInput;
        private string orignalOldPasswordInput;
        private string orignalCountryComboBox;

        private string usernameInput;
        private string oldPasswordInput;
        private string newPasswordInput;
        private string countryComboBox;

        public string UsernameInput
        {
            get { return usernameInput; }
            set
            {
                if (usernameInput != value)
                {
                    usernameInput = value;
                    OnPropertyChanged(nameof(UsernameInput));
                }
            }
        }

        public string OldPasswordInput
        {
            get { return oldPasswordInput; }
            set
            {
                if (oldPasswordInput != value)
                {
                    oldPasswordInput = value;
                    OnPropertyChanged(nameof(OldPasswordInput));
                }
            }
        }

        public string NewPasswordInput
        {
            get { return newPasswordInput; }
            set
            {
                if (newPasswordInput != value)
                {
                    newPasswordInput = value;
                    OnPropertyChanged(nameof(NewPasswordInput));
                }
            }
        }

        public string CountryComboBox
        {
            get { return countryComboBox; }
            set
            {
                if (countryComboBox != value)
                {
                    countryComboBox = value;
                    OnPropertyChanged(nameof(CountryComboBox));
                }
            }
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveUserDetails());
        public RelayCommand CancelCommand => new RelayCommand(execute => Cancel());
        public RelayCommand ExitCommand => new RelayCommand(execute => Exit());


        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som initierar UserDetailsWindowViewModel. //
        public UserDetailsWindowViewModel(Window userDetailsWindow)
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //

            _userDetailsWindow = userDetailsWindow;

            // Kontrollerar så att en användare är inloggad. //
            if (userManager.LoggedInUser != null)
            {
                // Tar in inmatade ändringar på användareinformationen. //
                UsernameInput = userManager.LoggedInUser.Username;
                OldPasswordInput = userManager.LoggedInUser.Password;
                CountryComboBox = userManager.LoggedInUser.Country;

                OnPropertyChanged(nameof(UsernameInput));
                OnPropertyChanged(nameof(OldPasswordInput));
                OnPropertyChanged(nameof(CountryComboBox));

                // Display:ar(kontrollerar) i min konsolfönster så att uppgifterna uppdateras. //
                Console.WriteLine($"Nuvarande användare" +
                    $"\nUsername: {UserManager.Instance.LoggedInUser.Username}" +
                    $"\nPassword: {UserManager.Instance.LoggedInUser.Password}" +
                    $"\nCountry: {UserManager.Instance.LoggedInUser.Country}");

                // Privata egenskaper som original informationen sparas(kopieras) till, när användare ångrar sig. //
                orignalUsernameInput = userManager.LoggedInUser.Username;
                orignalOldPasswordInput = userManager.LoggedInUser.Password;
                orignalCountryComboBox = userManager.LoggedInUser.Country;
            }

            _userDetailsWindow = userDetailsWindow;

            // Lista över typer som visas i ComboBox för "Type"-kolumnen. //
            WorkoutTypes = new ObservableCollection<string>
            {
                "Strength", "Cardio"
            };
        }

        // Lista för olika länder (fasta värden). //
        public ObservableCollection<string> Countries { get; set; } = new ObservableCollection<string>
        {
            "Gambia",
            "Sweden",
            "Norway",
            "Denmark",
        };

        
        // ------------------------------ Metoder ------------------------------ //
        public void SaveUserDetails()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to save?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Kontrollera att en användare är inloggad //
                var currentUserInfo = userManager.LoggedInUser;
                if (currentUserInfo == null)
                {
                    Console.WriteLine("No user is logged in.");
                    return;
                }

                // Lagrar referens till användare som ska uppdateras. //
                User userToUpdate = null;

                foreach (var user in userManager.Users)
                {
                    if (user.Username == currentUserInfo.Username)
                    {
                        userToUpdate = user;

                        // Avsluta loopen när rätt användare hittas. //
                        break; 
                    }
                }

                if (userToUpdate == null)
                {
                    Console.WriteLine("Could not find the user in the list.");
                    return;
                }

                // Kontroll av gammalt lösenord om användaren försöker ändra sitt lösenord. //
                if (!string.IsNullOrEmpty(OldPasswordInput) && OldPasswordInput != currentUserInfo.Password)
                {
                    MessageBox.Show("Old password is NOT correct. Please try again.");
                    return;
                }

                // Kontroll om användarnamnet ändras och finns redan. //
                if (!string.IsNullOrEmpty(UsernameInput) && UsernameInput != currentUserInfo.Username)
                {
                    if (userManager.Users.Any(u => u.Username == UsernameInput))
                    {
                        MessageBox.Show("The username is already in use by another user. Please choose another.");
                        return;
                    }
                    else
                    {
                        userToUpdate.Username = UsernameInput;
                        currentUserInfo.Username = UsernameInput;
                    }
                }

                // Kontroll och validering av nytt lösenord om användaren vill ändra det. //
                if (!string.IsNullOrEmpty(NewPasswordInput) && NewPasswordInput != currentUserInfo.Password)
                {
                    if (!IsPasswordValid(NewPasswordInput))
                    {
                        MessageBox.Show("The password must be at least 8 characters long and contain at least one number and one special character.");
                        return;
                    }
                    else
                    {
                        userToUpdate.Password = NewPasswordInput;
                        currentUserInfo.Password = NewPasswordInput;
                    }
                }

                // Uppdatera land om användaren har valt ett nytt land. //
                if (!string.IsNullOrEmpty(CountryComboBox) && CountryComboBox != currentUserInfo.Country)
                {
                    userToUpdate.Country = CountryComboBox;
                    currentUserInfo.Country = CountryComboBox;
                }

                MessageBox.Show("User information updated.");
                Console.WriteLine("User information updated.");

                // Öppnar WorkoutsWindow och stänger UserDetailsWindow. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();
                _userDetailsWindow.Close();
            }
        }

        // Metod för att avbryta och återställa informationen. //
        public void Cancel() 
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to cancel and reset?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Återgår till föregående användar information. //
                UsernameInput = orignalUsernameInput;
                OldPasswordInput = orignalOldPasswordInput;
                CountryComboBox = orignalCountryComboBox;

                OnPropertyChanged(nameof(UsernameInput));
                OnPropertyChanged(nameof(OldPasswordInput));
                OnPropertyChanged(nameof(CountryComboBox));

                // Kontroll-utskrift i konsolen. //
                Console.WriteLine($"Nuvarande användare - Username: {UsernameInput}" +
                    $"\nPassword: {OldPasswordInput}" +
                    $"\nCountry: {CountryComboBox}");
            }
            else if (result == MessageBoxResult.No)
            {
                // ------> Behövs det någon logik här? <------ //
            }
        }

        // Metod för att avbryta och gå tillbaka till WorkoutsWindow-fönstret. //
        public void Exit()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to exit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Öppnar upp WorkoutsWindow-fönstret. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();

                _userDetailsWindow.Close(); // Stänger ner UserDetailsWindow-fönstret. //
            }
        }

        // Metod för att validera lösenordets styrka. //
        private bool IsPasswordValid(string password)
        {
            // Kontrollerar att lösenordet är minst 8 tecken, innehåller minst en siffra och ett specialtecken. //
            return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(ch => !char.IsLetterOrDigit(ch)); // <----- Förstå denna bättre!
        }
    }
}
