using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class UserAccount
    {
        // ------------------------------ Egenskaper ------------------------------ //
        public string Username { get; set; } // Användarens användarnamn. //
        public string Password { get; set; } // Användarens lösernord. //
        public string Country { get; set; } // Användarens land. //

        // ------------------------------ Konstruktor ------------------------------ //
        // Konstruktor som initialiserar en ny instans av UserAccount med angivet användarnamn, lösenord och land. //
        public UserAccount(string Username, string Password, string Country)
        {
            this.Username = Username;
            this.Password = Password;
            this.Country = Country;
        }
    }
}
