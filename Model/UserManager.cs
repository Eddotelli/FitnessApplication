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
        // Singleton implementation. //
        private static UserManager instance = null; // Håller den enda instansen av UserManager, och är initialiserad till null för att inte ska skapas direkt utan när den faktiskt behövs. //
        private static readonly object padlock = new object(); // <---  Padlock-objekt. Ett objekt som säkerställer att endast en tråd(t.ex. ett fönster) åt gången kan skapa instansen. //


        // ------------------------------ Privata Listor ------------------------------ //
           // Hanteras endast inom klassen. Ingen utanför klassen kan ändra listan. // 

        // Privat lista som innehåller användarkonton. //
        private ObservableCollection<User> users = new ObservableCollection<User>();

        // Privat lista som innehåller träningspass med kort info. //
        private ObservableCollection<Workout> workoutsInfo = new ObservableCollection<Workout>();

        // Privat konstruktor förhindrar skapande av fler instanser. //
        private UserManager() 
        {
            // Fast(test)-användare. //
            users.Add(new User("user", "user123!", "Gambia", "user", "user" ));

            // Admin-användare. //
            var adminUser = new AdminUser("admin", "admin123!", "Country", "donkey", "kong");

            // Lägger till admin-användaren till listan för användare. //
            users.Add(adminUser);
        }

        // Egenskap för att få den inloggade användarens träningspass.
        public ObservableCollection<Workout> LoggedInUserWorkouts => LoggedInUser?.UserWorkouts;

        // Egenskap för lagring av kopierat träningspass. //
        public Workout CopiedWorkout { get; set; }

        // ------------------------------ Publika egenskaper för att få tillgång till listorna ------------------------------ //
        // Låter andra klsser att läsa och observera listan, men kan ej ersätta den utan att gå igenom kontrollerade  metoder.//

        // ----  '=>'(lamdbafunktion) är ett uttryckspil för definera en get-metod på ett kompakt sätt ---- //

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
        
        // Lista med användaren. //
        public ObservableCollection<User> Users => users;

        // Publik egenskap för att få tillgång till listan med träningspass för kort info. //
        public ObservableCollection<Workout> WorkoutsInfo => workoutsInfo;

        // Publik egenskap för att hålla koll samt ev. ändra på den inloggade användaren. //
        public User LoggedInUser { get; set; } // <------------------------------------------ Granska detta.

        // Samlad lista över alla träningspass för alla användare. //
        public ObservableCollection<Workout> GetAllWorkouts()
        {
            // Listan som lagrar träningspassen från alla användare. //
            ObservableCollection<Workout> allWorkouts = new ObservableCollection<Workout>();

            // Loppar igenom och lägger in varje skapad träningspass och varje användare. //
            foreach (var user in Users)
            {
                
                foreach (var workout in user.UserWorkouts)
                {
                    allWorkouts.Add(workout);
                }
            }
            return allWorkouts;
        }


        // ------------------------------ Metoder ------------------------------- //

        // Lägger till en användare i listan för användare. //
        public void AddUser (User user)
        {
            users.Add(user);
        }

        // Kontrollerar om användaren redan finns i listan. //
        public bool UserExists(string username)
        {
            foreach (var user in users)
            {
                if (user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        // Håller koll på inloggade användare. //
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
        public void AddWorkout(Workout workoutInfo) 
        {
            // Kontrollera om ett träningspass redan finns i listan. //
            if (!workoutsInfo.Contains(workoutInfo))
            {
                workoutsInfo.Add(workoutInfo);
            }
        }

        // Metod för att meddela ändring i enskilda objekt. //
        public void UpdateWorkout(Workout workout)
        {
            // Temporär lösning för att signalera ändring för WorkoutWindow. //
            int index = WorkoutsInfo.IndexOf(workout);
            if (index >= 0)
            {
                WorkoutsInfo[index] = workout;
            }
        }
    }
}
