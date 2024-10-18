using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class ListOfUsers
    {
        // ---------- Egenskaper ---------- //
        public string Username { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }

        // ---------- Konstruktor ---------- //
        public ListOfUsers(string Username, string Password, string Country)
        {
            this.Username = Username;
            this.Password = Password;
            this.Country = Country;
        }
    }
}
