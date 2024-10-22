using FitTrack.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.ViewModel
{
    public class UserDetailsWindowViewModel
    {
        // Singleton-instans av UserManager, används för att hantera gemensam lista mellan olika fönster. //
        private UserManager userManager;

        public ObservableCollection<User> users { get; set; }

        // ---------- Egenskaper ---------- //
        public string UsernameInput {  get; set; }
        public string PasswordInput { get; set; }
        public string ConfirmPasswordInput {  get; set; }
        public string CountryComboBox {  get; set; }


        // ------------------------------ Konstruktor ------------------------------ //
        // Konstruktor som skapar en ny instans av UserManager. //
        public UserDetailsWindowViewModel()
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //

            if (userManager.LoggedInUser != null) 
            {
                UsernameInput = userManager.LoggedInUser.Username;
                PasswordInput = userManager.LoggedInUser.Password;
                CountryComboBox = userManager.LoggedInUser.Country;
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
            
        }

        public void Cancel() 
        {
            
        }
    }
}
