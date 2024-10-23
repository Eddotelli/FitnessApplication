using FitTrack.Model;
using FitTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.ViewModel
{
    public class UserDetailsWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att hantera gemensam lista mellan olika fönster. //
        private UserManager userManager;

        // ---------- Egenskaper ---------- //
        //public string UsernameInput {  get; set; }
        //public string PasswordInput { get; set; }
        //public string ConfirmPasswordInput {  get; set; }
        //public string CountryComboBox {  get; set; }

        private string usernameInput;
        private string oldPasswordInput;
        private string newPasswordInput;
        private string countryComboBox;

        public string UsernameInput
        {
            get { return usernameInput; }
            set
            {
                if (usernameInput != value)
                {
                    usernameInput = value;
                    OnPropertyChanged(nameof(UsernameInput));
                }
            }
        }

        public string OldPasswordInput
        {
            get { return oldPasswordInput; }
            set
            {
                if (oldPasswordInput != value)
                {
                    oldPasswordInput = value;
                    OnPropertyChanged(nameof(OldPasswordInput));
                }
            }
        }

        public string NewPasswordInput
        {
            get { return newPasswordInput; }
            set
            {
                if (newPasswordInput != value)
                {
                    newPasswordInput = value;
                    OnPropertyChanged(nameof(NewPasswordInput));
                }
            }
        }

        public string CountryComboBox
        {
            get { return countryComboBox; }
            set
            {
                if (countryComboBox != value)
                {
                    countryComboBox = value;
                    OnPropertyChanged(nameof(CountryComboBox));
                }
            }
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveUserDetails());
        public RelayCommand CancelCommand => new RelayCommand(execute => Cancel());
        

        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som skapar en ny instans av UserManager. //
        public UserDetailsWindowViewModel()
        {
            userManager = UserManager.Instance; // Använda Singelton-instansen. //

            if (userManager.LoggedInUser != null) 
            {
                UsernameInput = userManager.LoggedInUser.Username;
                OldPasswordInput = userManager.LoggedInUser.Password;
                CountryComboBox = userManager.LoggedInUser.Country;

                OnPropertyChanged(nameof(UsernameInput));
                OnPropertyChanged(nameof(OldPasswordInput));
                OnPropertyChanged(nameof(CountryComboBox));
                
            }
        }

        // Lista för olika länder (fasta värden). //
        public ObservableCollection<string> Countries { get; set; } = new ObservableCollection<string>
        {
            "Gambia",
            "Sweden",
            "Norway",
            "Denmark",
        };

        // ------------------------------ Metoder ------------------------------ //
        public void SaveUserDetails()
        {
           
        }

        public void Cancel() 
        {
            
        }
    }
}
