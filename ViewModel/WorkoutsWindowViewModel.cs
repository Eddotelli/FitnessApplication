using FitTrack.Model;

namespace FitTrack.ViewModel
{
    public class WorkoutsWindowViewModel
    {
        // ---------- Egenskaper ---------- //
        public User User{ get; set; }
        public int MyProperty { get; set; }

        // ---------- Konstruktor ---------- // --- Denna kanske inte behövs?... Återkom till denna!
        //public WorkoutsWindowViewModel(User User, int MyProperty)
        //{
        //    this.User = User;
        //    this.MyProperty = MyProperty;
        //}

        // ------------------------------ Metoder ------------------------------ //
        public void AddWorkout()
        {

        }

        public void RemoveWorkout()
        {
            
        }

        public void OpenDetails(Workout workout) 
        {

        }
    }
}
