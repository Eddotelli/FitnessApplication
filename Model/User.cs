using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class User : Person
    {
        // ------------------------------ Egenskaper ------------------------------ //
        public string Country {  get; set; }
        public string SecurityQuestion {  get; set; }
        public string SecurityAnswer {  get; set; }

        // ------------------------------ Konstruktor ------------------------------ //
        public User(string Username, string Password, string Country, string SecurityQuestion, string SecurityAnswer) : base(Username, Password)
        {
            this.Username = Username;
            this.Password = Password; 
            this.Country = Country;
            this.SecurityQuestion = SecurityQuestion;
            this.SecurityAnswer = SecurityAnswer;
        }

        // ------------------------------ Metoder ------------------------------ //
        public override void SignIn()
        {

        }
        

        public void ResetPassword(string securityAnswer)
        {

        }
    }
}
