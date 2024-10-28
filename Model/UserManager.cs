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
        private static readonly object padlock = new object(); // <---  Padlock-objekt. Ett objekt som säkerställer att endast en tråd(t.ex. ett fönster) åt gången kan skapa instansen. //


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
            users.Add(new UserAccount("t", "t", "t" ));
        }

        // ------------------------------ Publika egenskaper för att få tillgång till listorna ------------------------------ //
        // Låter andra klsser att läsa och observera listan, men kan ej ersätta den utan att gå igenom kontrollerade  metoder.//

                         // ----  '=>' är ett uttryckspil för definera en get-metod på ett kompakt sätt ---- //

        // Proccesen för att hämta instansen. //
        // 1. Kontollerar om instansen är null när egenskapen Instance nås. //
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
        
        // Listan med användaren. //
        public ObservableCollection<UserAccount> Users => users;

        // Publik egenskap för att få tillgång till listan med träningspass för kort info. //
        public ObservableCollection<WorkoutInfo> WorkoutsInfo => workoutsInfo;

        // Publik egenskap för att hålla koll samt ev. ändra på den inloggade användaren. //
        public UserAccount LoggedInUser { get; set; } // <------------------------------------------ Granska detta.
         

        // ------------------------------ Metoder ------------------------------- //

        // Lägger till en användare i listan för användare. //
        public void AddUser (UserAccount user)
        {
            users.Add(user);
        }

        public bool CurrentUser(string username)
        {
            // Sätts till null innan vi börjar leta efter användare. //
            LoggedInUser = null;

            // Loopa igenom användarlistan för att hitta användaren. //
            foreach (var user in users)
            {
                // Om en match finns, ger `LoggedInUser` värdet username-inputet och returnerar true. //
                if (user.Username == username)
                {
                    LoggedInUser = user;
                    Console.WriteLine($"{LoggedInUser.Username} är inloggad!!"); // Kontroll-utskrift för att se att det funkar. //
                    return true;
                }
            }

            // Om ingen match hittas, returnerar false. //
            return false;
        }

        // Lägger till användare träningspass i listan för träningspass. //
        public void AddWorkout(WorkoutInfo workoutInfo) 
        {
            // Kontrollera om ett träningspass redan finns i listan. //
            if (!workoutsInfo.Contains(workoutInfo))
            {
                workoutsInfo.Add(workoutInfo);
            }
        }
    }
}
