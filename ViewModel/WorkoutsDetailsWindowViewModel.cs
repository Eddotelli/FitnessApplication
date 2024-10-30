using FitTrack.Model;
using FitTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitTrack.ViewModel
{
    public class WorkoutsDetailsWindowViewModel : ViewModelBase
    {
        // Singleton-instans av UserManager, används för att få tillgång till träningspassen
        private UserManager userManager;

        // Bunden till UserManager's WorkoutsInfo direkt för synkronisering med andra fönster. //
        public ObservableCollection<Workout> WorkoutsInfo => userManager.WorkoutsInfo;

        // Lokal kopia av träningspasset som redigeras
        private Workout localWorkout;

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
        // Bindningsbara egenskaper för att visa och ändra information i UI. //
        public string Name
        {
            get { return localWorkout.Name; } // Hämtar namnet från träningspasset
            set
            {
                localWorkout.Name = value; // Uppdaterar namnet. //
                OnPropertyChanged(nameof(Name)); // Meddelar UI om ändringen. //
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
            get { return localWorkout.Duration; }
            set
            {
                localWorkout.Duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public int CaloriesBurned
        {
            get { return localWorkout.CaloriesBurned; }
            set
            {
                localWorkout.CaloriesBurned = value;
                OnPropertyChanged(nameof(CaloriesBurned));
            }
        }

        public string Notes
        {
            get { return localWorkout.Notes; }
            set
            {
                localWorkout.Notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        public DateTime Date
        {
            get { return localWorkout.Date; }
            set
            {
                localWorkout.Date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        // ------------------------------ Konstruktor ------------------------------ //
        // Konstruktorn tar emot det träningspass som ska redigeras
        public WorkoutsDetailsWindowViewModel(Workout workout)
        {
            userManager = UserManager.Instance; // Använd befintlig instans av UserManager. //
            localWorkout = workout; // Spara träningspasset som skickats in. //
        }  

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand EditCommand => new RelayCommand(execute => EditWorkout());
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveWorkout());
        public RelayCommand CopyCommand => new RelayCommand(execute => CopyWorkout());


        // ------------------------------ Metoder ------------------------------ //
        // Metod för att låsa upp textfälten så att de kan redigeras. //
        public void EditWorkout()
        {
            IsReadOnly = false; // Gör fälten redigerbara. //
        }

        // Metod för att spara ändringar.
        public void SaveWorkout()
        {
            IsReadOnly = true; // Lås fälten igen. //

            // Använd UpdateWorkout för att meddela ändringen i UserManager
            userManager.UpdateWorkout(localWorkout);
        }

        public void CopyWorkout()
        {
            // Kontrollera om träningspasset är av typen StrengthWorkout
            if (localWorkout is StrengthWorkout strengthWorkout)
            {
                userManager.CopiedWorkout = new StrengthWorkout(
                    strengthWorkout.Name,
                    strengthWorkout.Repetitions,
                    strengthWorkout.Date,
                    strengthWorkout.TypeInput,
                    strengthWorkout.Duration,
                    strengthWorkout.CaloriesBurned,
                    strengthWorkout.Notes
                );
            }
            // Kontrollera om träningspasset är av typen CardioWorkout
            else if (localWorkout is CardioWorkout cardioWorkout)
            {
                userManager.CopiedWorkout = new CardioWorkout(
                    cardioWorkout.Name,
                    cardioWorkout.Distance,
                    cardioWorkout.Date,
                    cardioWorkout.TypeInput,
                    cardioWorkout.Duration,
                    cardioWorkout.CaloriesBurned,
                    cardioWorkout.Notes
                );
            }

            MessageBox.Show("Workout copied successfully!");
        }

    }
}
