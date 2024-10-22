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
        // Singleton-instans av UserManager, används för att hantera gemensam lista av träningspass mellan olika fönster. //
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
            userManager = UserManager.Instance; // Använda Singelton-instansen. //
        }

        // ------------------------------ Metoder ------------------------------ //
        private void LogIn()
        {
            // Kontrollera om användarnamn och lösenord matchar en användare i listan. //
            bool isAuthenticated = false;

            //Loppar igenom användarlistam från UserManager. //
            foreach (var user in userManager.Users) 
            {
                // Kontrollerar om inmatade användare finns. //
                if (user.Username == UsernameInput && user.Password == PasswordInput)
                {
                    isAuthenticated = true;
                    break; // Avsluta loopen när inloggningen är lyckad. //
                }
            }

            // Kontrollera om inloggningen går igenom eller ej. //
            if (isAuthenticated)
            {
                MessageBox.Show("Login successful!");

                // Rensa fälten efter inloggning. //
                UsernameInput = string.Empty;
                PasswordInput = string.Empty;

                // Meddelar UI att något har ändrats efter att värdena är tomma. //
                OnPropertyChanged(nameof(UsernameInput));
                OnPropertyChanged(nameof(PasswordInput));

                // Sparar ner användarens Username. //
                userManager.CurrentUser(UsernameInput);
                
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
