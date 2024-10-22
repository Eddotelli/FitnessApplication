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
        //private static UserManager instance;
        //public static UserManager => Instance ?? =new UserManager(); // Alternativ 1. //

        // Alternativ 2. //
        // Singleton implementation. //
        private static UserManager instance = null; // Håller den enda instansen av UserManager, och är initialiserad till null för att inte ska skapas direkt utan när den faktiskt behövs. //
        private static readonly object padlock = new object(); // <---  Padlock-objekt. Ett objekt som säkerställer att endast en tråd åt gången kan skapa instansen. //


        // ------------------------------ Privata Listor ------------------------------ //
           // Hanteras endast inom klassen. Ingen utanför klassen kan ändra listan. // 

        // Privat lista som innehåller användarkonton. //
        private ObservableCollection<UserAccount> users = new ObservableCollection<UserAccount>();

        // Privat lista som innehåller träningspass med kort info. //
        private ObservableCollection<WorkoutInfo> workoutsInfo = new ObservableCollection<WorkoutInfo>();

        // Privat konstruktor förhindrar skapande av fler instanser. //
        private UserManager() 
        {
            //Fast(test)-användare. //
            users.Add(new UserAccount("test", "test", "test" ));
        }

        // ============================================================================== //


        // ------------------------------ Publika egenskaper för att få tillgång till listorna ------------------------------ //
        // Låter andra klsser att läsa och observera listan, men kan ej ersätta den utan att gå igenom kontrollerade  metoder.//

                         // ----  '=>' är ett uttryckspil för definera en get-metod på ett kompakt sätt ---- //

        // Listan med användaren. //
        public ObservableCollection<UserAccount> Users => users;

        // Publik egenskap för att få tillgång till listan med träningspass för kort info. //
        public ObservableCollection<WorkoutInfo> WorkoutsInfo => workoutsInfo;

        // Publik egenskap för att hålla koll samt ev. ändra på den inloggade användaren. //
        public UserAccount LoggedInUser { get; set; } // <------------------------------------------ Granska detta.

        
        // ============================================================================== //
        // 

        // Proccesen för att hämta instansen. //
        // 1. Kontollerar om instancen är null när egenskapen Instance nås. //
        // 2. Om instansen är null, går den i ett lock-block för att säkerhetställa trådsäkerheten. //
        // 3. Kontollerar inuti samma block om instansen återigen är null innan ny instans av UserManager skapas. //
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

        // ------------------------------ Metoder ------------------------------- //

        // Lägger till en användare i listan för användare. //
        public void AddUser (UserAccount user)
        {
            users.Add(user);
        }

        // För att hålla koll på inloggande användare. //
        //public void CurrentUser(string username) // <--- Granska detta.
        //{
        //    // Sätts till null då vi antar att ingen är användare är inloggad. //
        //    LoggedInUser = null;

        //    // Loppar igenom listan. //
        //    foreach (var user in users)
        //    {
        //        // Kontrollerar om en match finns, då sätts värdet(användaren) i LoggedInUser. //
        //        if (user.Username == username)
        //        {
        //            LoggedInUser = user;
        //            break;
        //        }
        //    }
        //}

        // ============== TEST - för att se om användaren sparas i CurrentUser metoden. ============== //
        public bool CurrentUser(string username)
        {
            // Sätts till null då vi antar att ingen användare är inloggad.
            LoggedInUser = null;

            // Lopp genom listan.
            foreach (var user in users)
            {
                // Kontrollera om en match finns, då sätts värdet (användaren) i LoggedInUser.
                if (user.Username == username)
                {
                    LoggedInUser = user;
                    return true; // Inloggningen lyckades
                }
            }

            return false; // Inloggningen misslyckades
        }
        // =============== TEST =============== //


        // Lägger till användare träningspass i listan för träningspass. //
        public void AddWorkout(WorkoutInfo workoutInfo) 
        {
            // Kontrollera om ett träningspass redan finns i listan. //
            if (!workoutsInfo.Contains(workoutInfo))
            {
                workoutsInfo.Add(workoutInfo );
            }
        }
    }
}
