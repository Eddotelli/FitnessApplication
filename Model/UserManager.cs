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

        // Privat konstruktor förhindrar skapande av fler instanser. //
        private UserManager() 
        {
            // (test)-användare "user". //
            var user = new User("user", "user123!", "Gambia", "user", "user");

            // Lägger till user-användaren till listan för användare. //
            AddUser(user);

            // Lägger till träningspass till användaren "user". //
            Workout passEtt = new StrengthWorkout("Squats", 30, DateTime.Now, "Strength", TimeSpan.FromMinutes(5), 300, "3x10 sets");
            Workout passTva = new CardioWorkout("Intervals", 10, DateTime.Now, "Cardio", TimeSpan.FromMinutes(3), 630, "3x10 sets");

            // Lägger till träningspassen i den user(test)-användare användarens träningslista. //
            user.UserWorkouts.Add(passEtt);
            user.UserWorkouts.Add(passTva);

            // Admin-användare. //
            var adminUser = new AdminUser("admin", "admin123!", "Country", "donkey", "kong");

            // Lägger till admin-användaren till listan för användare. //
            AddUser(adminUser);
        }

        // Privat lista som innehåller träningspass med kort info. //
        private ObservableCollection<Workout> workoutsInfo = new ObservableCollection<Workout>();

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
        public User LoggedInUser { get; set; }

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
                    UpdateUserWorkouts();
                    Console.WriteLine($"{LoggedInUser.Username} är inloggad!!"); // Kontroll-utskrift för att se att det funkar. //
                    return true;
                }
            }

            // Om ingen match hittas, returnerar false. //
            return false;
        }

        // Rensar träningspass-listan och lägger till inloggad användares pass
        public void UpdateUserWorkouts()
        {
            workoutsInfo.Clear();  // Rensar listan innan vi lägger till träningspassen

            if (LoggedInUser is AdminUser)
            {
                // För AdminUser, hämta alla träningspass från alla användare.
                foreach (var user in Users)
                {
                    foreach (var workout in user.UserWorkouts)
                    {
                        workoutsInfo.Add(workout);
                    }
                }
            }
            else if (LoggedInUser != null)
            {
                // För vanliga användare, lägg till endast deras egna träningspass.
                foreach (var workout in LoggedInUser.UserWorkouts)
                {
                    workoutsInfo.Add(workout);
                }
            }
        }

        // Rensar WorkoutsInfo, användbar vid byte av användare. //
        public void ClearAllWorkouts()
        {
            workoutsInfo.Clear();
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
