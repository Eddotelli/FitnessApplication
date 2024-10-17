using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.ViewModel
{
    public class UserDetailsWindowViewModel
    {
        // ---------- Egenskaper ---------- //
        public string UsernameInput {  get; set; }
        public string PasswordInput { get; set; }
        public string ConfirmPasswordInput {  get; set; }
        public string CountryComboBox {  get; set; }


        // ------------------------------ Metoder ------------------------------ //
        public void SaveUserDetails()
        {

        }

        public void Cancel() 
        {
            
        }
    }
}
