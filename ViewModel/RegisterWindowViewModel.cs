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
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    public class RegisterWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att dela en gemensam lista och centraliserad datahantering mellan olika fönster. //
        private UserManager userManager;

        // Denna referens används för att kunna stänga eller kontrollera fönstret från ViewModel. //
        private readonly Window _registerWindow;

        // ------------------------------ Egenskaper ------------------------------ //
        public string UsernameInput { get; set; }
        public string PasswordInput { get; set; }
        public string ConfirmPasswordInput { get; set; }
        public string CountryComboBox { get; set; }
        public string SecurityQuestionComboBox {  get; set; } // Vald säkerhetsfråga. //
        public string SecurityAnswer { get; set; } // Svar på säkerhetsfrågan. //

        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som initierar RegisterWindowViewModel. //
        public RegisterWindowViewModel(Window registerWindow)
        {
            // Hämtar Singleton-instansen av UserManager för att säkerställa att samma användar- och datahantering delas över hela applikationen. //
            userManager = UserManager.Instance;
            
            // Detta gör att ViewModel kan stänga fönstret när registreringen är klar. //
            _registerWindow = registerWindow;
        }

        // Lista för val av land //
        public ObservableCollection<string> Countries { get; set; } = new ObservableCollection<string>
        {
            "Gambia",
            "Sweden",
            "Norway",
            "Denmark",
        };

        // Lista för val av säkerhetsfråga. //
        public ObservableCollection<string> SecurityQuestions { get; set; } = new ObservableCollection<string>
        {
            "What is your pet's name?",
            "What is your mother's maiden name?",
            "What was the name of your first school?",
        };

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand RegisterCommand => new RelayCommand(execute => RegisterNewUser());
        public RelayCommand CancelCommand => new RelayCommand(execute => Cancel());

        // ------------------------------ Metoder ------------------------------ //

        // Metod för att lägga till ny användare. //
        public void RegisterNewUser()
        {
            // Kontrollerar om användarnamnet redan finns i UserManager. //
            if (userManager.UserExists(UsernameInput))
            {
                MessageBox.Show("The username already exists, please choose another one.");
                return;
            }

            // Kontrollerar om användarnamnet är kortare än 3 bokstäver. //
            if (UsernameInput.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long.");
            }

            // Kontrollerar lösenordets styrka. //
            if (!IsPasswordValid(PasswordInput))
            {
                MessageBox.Show("The password must be at least 8 characters long and contain at least one number and one special character.");
                return;
            }

            // Kontrollerar om lösernorden matchar. //
            if (PasswordInput != ConfirmPasswordInput)
            {
                MessageBox.Show("The passwords do not match.");
                return;
            }

            // Kontrollerar att ett land har valts. //
            if (string.IsNullOrEmpty(CountryComboBox))
            {
                MessageBox.Show("Please select a country.");
                return;
            }

            // Kontrollerar att en säkerhetsfråga har valts och att ett svar har angivits. //
            if (string.IsNullOrEmpty(SecurityQuestionComboBox) || string.IsNullOrWhiteSpace(SecurityAnswer))
            {
                MessageBox.Show("Please select a security question and provide an answer.");
                return;
            }

            // Skapar ett nytt objekt av typen User för den nya användaren. //
            var newUser = new User(UsernameInput, PasswordInput, CountryComboBox, SecurityQuestionComboBox, SecurityAnswer);

            // Lägger till användaren i UserManager. //
            userManager.AddUser(newUser);

            // Rensar fälten efter registrering. //
            UsernameInput = string.Empty;
            PasswordInput = string.Empty;
            ConfirmPasswordInput = string.Empty;
            CountryComboBox = string.Empty;
            SecurityQuestionComboBox = string.Empty;

            // Meddelar UI att värdena har uppdaterats. //
            OnPropertyChanged(nameof(UsernameInput));
            OnPropertyChanged(nameof(PasswordInput));
            OnPropertyChanged(nameof(ConfirmPasswordInput));
            OnPropertyChanged(nameof(CountryComboBox));
            OnPropertyChanged(nameof(SecurityQuestionComboBox));
            OnPropertyChanged(nameof(SecurityAnswer));

            MessageBox.Show("Registration successful!");

            // Öppnar upp MainWindow-fönstret. //
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Stänger ner RegisterWindow-fönstret. //
            _registerWindow.Close();
        }

        // Metod för att avbryta och gå tillbaka till MainWindow-fönstret. //
        public void Cancel()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to cancel and go back?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Öppnar upp MainWindow-fönstret. //
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                // Stänger ner RegisterWindow-fönstret. //
                _registerWindow.Close();
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
