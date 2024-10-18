using FitTrack.MVVM;
using FitTrack.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitTrack.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        // ---------- Egenskaper ---------- //
        public string LabelTitle { get; set; }
        public string UsernameInput { get; set; }
        public string PasswordInput { get; set; }


        public RelayCommand SignInCommand => new RelayCommand(execute => LogIn());
        public RelayCommand RegisterCommand => new RelayCommand(execute => Register());


        // ------------------------------ Metoder ------------------------------ //
        private void LogIn()
        {
            MessageBox.Show("Sign in knappen funkar!");
            
        }

        private void Register()
        {
            //MessageBox.Show("Register knappen funkar!");
            RegisterWindow register = new RegisterWindow();
            register.Show();
        }
    }
}
