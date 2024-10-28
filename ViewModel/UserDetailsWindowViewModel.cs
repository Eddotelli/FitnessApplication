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
    public class UserDetailsWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att hantera gemensam lista mellan olika fönster. //
        private UserManager userManager;

        // ---------- Egenskaper ---------- //
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


        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som skapar en ny instans av UserManager. //
        public UserDetailsWindowViewModel()
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //

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
                // Deklarerar en ny variabel för att lagra uppdaterade uppgifter. //
                var currentUserInfo = userManager.LoggedInUser;

                // Kontrollerar så att en användare är inloggad. //
                if (userManager.LoggedInUser != null)
                {
                    // Kontrollerar om det gamla lösernordet är korrekt. //
                    if (OldPasswordInput == userManager.LoggedInUser.Password)
                    {
                        UserAccount userToUpdate = null; // Deklararer variabel med UserAccount, en plats att lagra upphittad användare från UserAccount-listan. //
                        foreach (var user in userManager.Users)
                        {
                            if (user.Username == currentUserInfo.Username)
                            {
                                // Loppar igenom för att se om användaren-namnet redan finns i listan. //
                                if (user.Username == UsernameInput && user.Password == NewPasswordInput)
                                {
                                    MessageBox.Show("The username is already in use by another user. Please choose another.");
                                }
                                else
                                {
                                    userToUpdate = user;
                                    break;
                                }
                            }
                        }

                        if (userToUpdate != null)
                        {
                            // Uppdaterar användarens information. //
                            userToUpdate.Username = UsernameInput;
                            userToUpdate.Password = newPasswordInput;
                            userToUpdate.Country = countryComboBox;

                            // Uppdatera också referensen till LoggedInUser som för inloggade användaren. //
                            currentUserInfo.Username = UsernameInput;
                            currentUserInfo.Password = newPasswordInput;
                            currentUserInfo.Country = countryComboBox;

                            MessageBox.Show("User information updated.");

                            // Kontroll-utskrift i konsolen. //
                            Console.WriteLine("User information updated.");
                        }
                        else
                        {
                            Console.WriteLine("Could not find the user in the list.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Old password is NOT right. Please try again.");
                    }                   
                }
                else
                {
                    Console.WriteLine("No user is logged in.");
                }
            }           
            else if (result == MessageBoxResult.No)
            {
                // ------> Behövs det någon logik här? <------ //
            }
        }

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
    }
}
