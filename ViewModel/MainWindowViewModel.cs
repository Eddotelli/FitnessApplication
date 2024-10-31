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
        // Singleton-instans av UserManager, används för att dela en gemensam lista och centraliserad datahantering mellan olika fönster. //
        private UserManager userManager;

        // Denna referens används för att kunna stänga eller kontrollera fönstret från ViewModel. //
        private readonly Window _mainWindow;

        // ------------------------------ Egenskaper ------------------------------ //
        public string LabelTitle { get; set; }
        public string UsernameInput { get; set; }
        public string PasswordInput { get; set; }
        public string SecurityAnswer { get; set; }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand SignInCommand => new RelayCommand(execute => SignIn());
        public RelayCommand RegisterCommand => new RelayCommand(execute => Register());
        public RelayCommand ResetPasswordCommand => new RelayCommand(execute => ResetPassword());


        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som initierar MainWindowViewModel. //
        public MainWindowViewModel(Window mainwindow)
        {
            // Hämtar Singleton-instansen av UserManager för att säkerställa att samma användar- och datahantering delas över hela applikationen. //
            userManager = UserManager.Instance;

            // Förinmatade user(test)-användare. //
            UsernameInput = "user";
            PasswordInput = "user123!";

            OnPropertyChanged(nameof(UsernameInput));
            OnPropertyChanged(nameof(PasswordInput));

            // Sparar referens till fönstret. //
            _mainWindow = mainwindow;
        }

        // ------------------------------ Metoder ------------------------------ //
        private void SignIn()
        {
            // Kontrollerar om användarnamn och lösenord matchar en användare i listan direkt med hjälp av CurrentUser. //
            bool isAuthenticated = false;

            // Kontrollerar om användaren finns och bekräftar användaren. //
            foreach (var user in userManager.Users)
            {
                if (user.Username == UsernameInput && user.Password == PasswordInput)
                {
                    // isAuthenticated får värdet 'true' genom metoden CurrentUser i UserManager-klassen. Detta ger även LoggedInUser username-inputet som värde. //
                    isAuthenticated = userManager.CurrentUser(UsernameInput);
                    break;
                }
            }

            // Kontrollerar om inloggningen gick igenom eller ej. //
            if (isAuthenticated)
            {
                // Om inloggningen lyckas visas ett välkomstmeddelande. //
                userManager.LoggedInUser.SignIn();

                // Rensa fälten efter inloggning. //
                UsernameInput = string.Empty;
                PasswordInput = string.Empty;

                // Meddelar UI att något har ändrats efter att värdena är tomma. //
                OnPropertyChanged(nameof(UsernameInput));
                OnPropertyChanged(nameof(PasswordInput));

                // Öppna WorkoutsWindow. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();

                // Stänger ner MainWindow-fönstret. //
                _mainWindow.Close();
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

            // Stänger ner MainWindow-fönstret. //
            _mainWindow.Close();

        }

        private void ResetPassword()
        {
            // Sök efter användaren baserat på inmatat användarnamn. //
            User user = null;

            foreach (User u in userManager.Users)
            {
                if (u.Username == UsernameInput)
                {
                    user = u;
                    break;
                }
            }

            if (user != null)
            {
                // Om användaren hittas, visas säkerhetsfrågan. //
                var userQuestion = user.SecurityQuestion;
                string answer = Microsoft.VisualBasic.Interaction.InputBox($"Security Question:\n{userQuestion}", "Answer Security Question");

                // Kontrollera om svaret matchar det som användaren har anget. //
                if (answer.Equals(user.SecurityAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Security answer correct!");

                    // Anropar ResetPassword på user-objektet och visar användarens lösenord i PasswordInput. //
                    user.ResetPassword(answer);

                    // Uppdaterar UI. //
                    OnPropertyChanged(nameof(PasswordInput));
                }
                else
                {
                    MessageBox.Show("Incorrect security answer. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Username not found. Please check the username and try again.");
            }
        }

        // Metod som hanterar visning av lösenordet. //
        public void ShowPassword(string password)
        {
            PasswordInput = password;
        }
    }
}
