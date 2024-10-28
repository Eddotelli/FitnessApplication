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
    public class WorkoutsDetailsWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att få tillgång till träningspassen
        private UserManager userManager;

        // Lokal kopia av träningspasset som redigeras
        private WorkoutInfo localWorkout;

        // Fält för att styra om fälten ska vara låsta för redigering (default är låst)
        private bool isReadOnly = true;

        // Offentlig bindningsbar egenskap för att styra om fälten är redigerbara i UI
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly)); // Meddelar UI om ändringen
            }
        }


        // ------------------------------ Egenskaper ------------------------------ //
        //public Workout Workout { get; set; } // Se över nedan och i AddworkoutWindow-fönstret gällande WorkoutInfo-parameterna mot Workout-parameterna.

        //// Privat variabel som håller träningsinformation. //
        //private ObservableCollection<WorkoutInfo> workoutsInfo;

        ////Public egenskap som ger kontrollerad åtkomst till träningsinformationen. //
        //public ObservableCollection<WorkoutInfo> WorkoutsInfo

        //{
        //    get { return workoutsInfo; } // Returnerar den privata listan med träningsinformation. //
        //    set
        //    {
        //        // Sätter det nya värdet för träningsinformationen. //
        //        workoutsInfo = value;

        //        //Berättar för UI om att WorkoutsInfo har ändrats. //
        //        OnPropertyChanged(nameof(WorkoutsInfo));
        //    }
        //}

        // ------------------------------ Konstruktor ------------------------------ //
        // Konstruktorn tar emot det träningspass som ska redigeras
        public WorkoutsDetailsWindowViewModel(WorkoutInfo workout)
        {
            userManager = UserManager.Instance; // Använd befintlig instans av UserManager
            localWorkout = workout; // Spara träningspasset som skickats in
        }


        // Bindningsbara egenskaper för att visa och ändra information i UI
        public string Name
        {
            get { return localWorkout.NameInput; } // Hämtar namnet från träningspasset
            set
            {
                localWorkout.NameInput = value; // Uppdaterar namnet
                OnPropertyChanged(nameof(Name)); // Meddelar UI om ändringen
            }
        }

        public string Type
        {
            get { return localWorkout.TypeInput; }
            set
            {
                localWorkout.TypeInput = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public TimeSpan Duration
        {
            get { return localWorkout.DurationInput; }
            set
            {
                localWorkout.DurationInput = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public int CaloriesBurned
        {
            get { return localWorkout.CaloriesBurnedInput; }
            set
            {
                localWorkout.CaloriesBurnedInput = value;
                OnPropertyChanged(nameof(CaloriesBurned));
            }
        }

        public string Notes
        {
            get { return localWorkout.NotesInput; }
            set
            {
                localWorkout.NotesInput = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        public DateTime Date
        {
            get { return localWorkout.DateInput; }
            set
            {
                localWorkout.DateInput = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand EditCommand => new RelayCommand(execute => EditWorkout());
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveWorkout());


        // ------------------------------ Metoder ------------------------------ //
        // Metod för att låsa upp textfälten så att de kan redigeras.
        public void EditWorkout()
        {
            IsReadOnly = false; // Gör fälten redigerbara.
        }

        // Metod för att spara ändringar.
        public void SaveWorkout()
        {
            // Spara ändringarna till UserManager om så behövs.
            //userManager.AddWorkout(localWorkout); // Exempelmetod för att spara uppdaterad information.
            IsReadOnly = true; // Lås fälten igen.
        }
    }
}
