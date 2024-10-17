using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public abstract class Person
    {
        // ---------- Egenskaper ---------- //
        public string Username {  get; set; }
        public string Password { get; set; }

        // ---------- Konstruktor ---------- //
        public Person(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        // ------------------------------ Metoder ------------------------------ //
        public abstract void SignIn();
    }
}
