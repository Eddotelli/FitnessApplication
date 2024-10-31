using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class AdminUser : User
    {
        // ------------------------------ Konstruktor ------------------------------ //
        public AdminUser(string Username, string Password, string Country, string SecurityQuestion, string SecurityAnswer) : base(Username, Password, Country, SecurityQuestion, SecurityAnswer)
        {
            this.Username = Username;
            this.Password = Password;
            this.Country = Country;
            this.SecurityQuestion = SecurityQuestion;
            this.SecurityAnswer = SecurityAnswer;
        }

        // ------------------------------ Metoder ------------------------------ //
        public ObservableCollection<Workout> ManageAllWorkouts()
        {
            return UserManager.Instance.GetAllWorkouts();
        }
    }
}
