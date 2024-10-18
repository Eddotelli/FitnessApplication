using FitTrack.Model;
using FitTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    public class RegisterWindowViewModel : ViewModelBase
    {
        private UserManager userManager;

        public RegisterWindowViewModel()
        {
            userManager = new UserManager();
        }

        // ---------- Egenskaper ---------- //
        public string UsernameInput{ get; set; }
        public string PasswordInput{ get; set; }
        

        public ObservableCollection<ListOfUsers> User => userManager.Users;

        // ---------- Kommando ---------- //
        public RelayCommand RegisterCommand => new RelayCommand(execute => RegisterNewUser());

        // ------------------------------ Metoder ------------------------------ //
        public void RegisterNewUser()
        {
            userManager.AddUser(UsernameInput, PasswordInput);

            //UsernameInput = string.Empty;
            //PasswordInput = string.Empty;
            

            OnPropertyChanged(nameof(UsernameInput));
            OnPropertyChanged(nameof(PasswordInput));
            

            //MessageBox.Show("Funkar!!");
        }
    }
}
