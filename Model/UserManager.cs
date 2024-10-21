using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FitTrack.Model
{
    public class UserManager
    {
        // Alternativ 1. //
        //private static UserManager instance;
        //public static UserManager => Instance ?? =new UserManager(); // Alternativ 1. //

        // Alternativ 2. //
        private static UserManager instance = null;
        private static readonly object padlock = new object();

        // Privat lista som innehåller användarkonton. //
        private ObservableCollection<UserAccount> users = new ObservableCollection<UserAccount>();

        // Skapa en ny, tom lista till en ny användare för hålla användarkonton. //
        private UserManager() { }

        // Publik egenskap för att få tillgång till listan med användaren. //
        public ObservableCollection<UserAccount> Users => users;

        // Metod för att hämta den enda instansen. //
        public static UserManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null) 
                    {
                        instance = new UserManager();
                    }
                    return instance;
                }
            }
        }

        // Lägger till en användare i listan. //
        public void AddUser (UserAccount user)
        {
            Users.Add(user);
        }

        //// Konstruktor som skapar en ny instans av en UserManager och intitierar en tom lista av användarkonton. //
        //public UserManager()
        //{
        //    // Skapa en ny, tom lista till en ny användare för hålla användarkonton. //
        //    users = new ObservableCollection<UserAccount>();
        //}

        //// För att få tillgång till listan med användare. //
        //public ObservableCollection<UserAccount> Users => users;

        //// Lägg till en användare i listan.
        //public void AddUser(string username, string password, string country)
        //{
        //    // Kontrollera om användarnamnet redan finns.
        //    if (users.Any(u => u.Username == username)) // <---- Förstå denna bättre!
        //    {
        //        MessageBox.Show("Användarnamnet finns redan. Välj ett annat.");
        //        return; // Avsluta metoden om användarnamnet redan existerar.
        //    }

        //    // Lägg till användaren om användarnamnet är unikt.
        //    users.Add(new UserAccount(username, password, country));
        //}

        // Lägg till andra användarrelaterade metoder här, t.ex. borttagning, uppdatering, etc.
    }
}
