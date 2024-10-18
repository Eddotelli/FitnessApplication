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

        // ---------- Konstruktor ---------- //
        public ListOfUsers(string Username, string Password)
        {
            //this.Username = Username;
            //this.Password = Password;
        }
    }
}
