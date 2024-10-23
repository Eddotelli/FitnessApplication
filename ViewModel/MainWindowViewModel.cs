using FitTrack.Model;
using FitTrack.MVVM;
using FitTrack.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitTrack.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager som deklareras, används för att hantera gemensam lista mellan olika fönster. //
        private UserManager userManager;

        // ------------------------------ Egenskaper ------------------------------ //
        public string LabelTitle { get; set; }
        public string UsernameInput { get; set; }
        public string PasswordInput { get; set; }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand SignInCommand => new RelayCommand(execute => LogIn());
        public RelayCommand RegisterCommand => new RelayCommand(execute => Register());


        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som skapar en ny instans av UserManager. //
        public MainWindowViewModel()
        {
            // Ser till så att den används en och samma UserManager-instans varje gång den anropas. //
            userManager = UserManager.Instance; // Använda Singelton-instansen. //
        }

        // ------------------------------ Metoder ------------------------------ //
        private void LogIn()
        {
            // Kontrollerar om användarnamn och lösenord matchar en användare i listan direkt med hjälp av CurrentUser. //
            bool isAuthenticated = false;

            // Kontrollerar om användaren finns och bekräftar användaren. //
            foreach (var user in userManager.Users)
            {
                if (user.Username == UsernameInput && user.Password == PasswordInput)
                {
                    // isAuthenticated får värdet 'true' genom metoden CurrentUser i UserManager-klassen. //
                    isAuthenticated = userManager.CurrentUser(UsernameInput); // Detta ger även LoggedInUser username-inputet som värde. //
                    break;
                }
            }

            // Kontrollera om inloggningen gick igenom eller ej.
            if (isAuthenticated)
            {
                MessageBox.Show("Login successful!");

                // Rensa fälten efter inloggning. //
                UsernameInput = string.Empty;
                PasswordInput = string.Empty;

                // Meddelar UI att något har ändrats efter att värdena är tomma. //
                OnPropertyChanged(nameof(UsernameInput));
                OnPropertyChanged(nameof(PasswordInput));

                // Öppna WorkoutsWindow. //
                WorkoutsWindow workoutsWindow = new WorkoutsWindow();
                workoutsWindow.Show();
            }
            else
            {
                MessageBox.Show("Login NOT successful! Please try again.");
            }
        }

        // Metod för att öppna upp registrerings-fönstret. //
        private void Register()
        {
            // Skapar upp RegisterWindow. //
            RegisterWindow register = new RegisterWindow();
            register.Show();
        }
    }
}
