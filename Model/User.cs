using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitTrack.Model
{
    public class User : Person
    {
        // ------------------------------ Egenskaper ------------------------------ //
        public string Country {  get; set; }
        public string SecurityQuestion {  get; set; }
        public string SecurityAnswer {  get; set; }

        // Egenskap för att lagra träningspass specifikt för varje användare. //
        public ObservableCollection<Workout> UserWorkouts { get; set; }

        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som initialiserar en ny instans av UserAccount med angivet användarnamn, lösenord, land, säkerhetsfråga och svar till säkerhetsfrågan. //
        public User(string Username, string Password, string Country, string SecurityQuestion, string SecurityAnswer) : base(Username, Password)
        {
            this.Username = Username;
            this.Password = Password; 
            this.Country = Country;
            this.SecurityQuestion = SecurityQuestion;
            this.SecurityAnswer = SecurityAnswer;

            // Initiera UserWorkouts-listan vid skapande av en användare
            UserWorkouts = new ObservableCollection<Workout>();
        }

        

        // ------------------------------ Metoder ------------------------------ //
        public override void SignIn()
        {
            // Om inloggningen lyckas skickas ett välkomstmeddelande. //
            MessageBox.Show($"Login successful! Welcome {Username}.");
        }
        

        public void ResetPassword(string securityAnswer)
        {
            if (securityAnswer.Equals(SecurityAnswer, StringComparison.OrdinalIgnoreCase))
            {
                // Lösenordsåterställning logik – prompt användaren att ange ett nytt lösenord
                string newPassword = Microsoft.VisualBasic.Interaction.InputBox("Enter a new password:", "Reset Password");

                if (!string.IsNullOrEmpty(newPassword))
                {
                    Password = newPassword;
                    MessageBox.Show("Password has been reset successfully.");
                }
                else
                {
                    MessageBox.Show("Password reset canceled.");
                }
            }
            else
            {
                MessageBox.Show("Incorrect security answer. Cannot reset password.");
            }
        }
    }
}
