﻿using FitTrack.Model;
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
        // Deklaration av userManager. //
        private UserManager userManager;

        // ---------- Egenskaper ---------- //
        public string UsernameInput{ get; set; }
        public string PasswordInput{ get; set; }
        public string ConfirmPasswordInput { get; set; }
        public string CountryComboBox { get; set; }

        // ---------- Konstruktor ---------- //

        // Konstruktor som skapar en ny instans av UserManager. //
        public RegisterWindowViewModel()
        {
            userManager = new UserManager();
        }

        // Lista för användarna. //
        public ObservableCollection<ListOfUsers> User => userManager.Users;
        
        // Lista för olika länder (fasta värden). //
        public ObservableCollection<string> Countries { get; set; } = new ObservableCollection<string>
        {
            "Gambia",
            "Sweden",
            "Norway",
            "Denmark",
        };

        // ---------- Kommando ---------- //
        public RelayCommand RegisterCommand => new RelayCommand(execute => RegisterNewUser());

        // ------------------------------ Metoder ------------------------------ //
        
        // Metod för att lägga till ny användare. //
        public void RegisterNewUser()
        {
            if( PasswordInput == ConfirmPasswordInput)
            {
                // Lägger in Username och Password i listan UserManager. //
                userManager.AddUser(UsernameInput, PasswordInput, CountryComboBox);

                // Rensa fälten efter registrering. //
                UsernameInput = string.Empty;
                PasswordInput = string.Empty;
                ConfirmPasswordInput = string.Empty;
                CountryComboBox = string.Empty;

                // Meddelar UI att något har ändrats efter att värdena är tomma. //
                OnPropertyChanged(nameof(UsernameInput));
                OnPropertyChanged(nameof(PasswordInput));
                OnPropertyChanged(nameof(ConfirmPasswordInput));
                OnPropertyChanged(nameof(CountryComboBox));

                MessageBox.Show("Registration successful!");
            } 
            else
            {
                MessageBox.Show("Password didn't match, try again!");
            }
        }
    }
}
