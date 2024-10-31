using FitTrack.Model;
using FitTrack.MVVM;
using FitTrack.View;
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
        // Singleton-instans av UserManager, används för att dela en gemensam lista och centraliserad datahantering mellan olika fönster. //
        private UserManager userManager;

        // Denna referens används för att kunna stänga eller kontrollera fönstret från ViewModel. //
        private readonly Window _detailsWindow;

        // Bunden till UserManager's WorkoutsInfo direkt för synkronisering med andra fönster. //
        public ObservableCollection<Workout> WorkoutsInfo => userManager.WorkoutsInfo;

        // Lokal kopia av träningspasset som redigeras. //
        private Workout localWorkout;

        


        // ------------------------------ Egenskaper ------------------------------ //

        // Bindningsbara egenskaper för att visa och ändra information i UI. //
        public string Name
        {
            // Hämtar namnet från träningspasset. //
            get { return localWorkout.Name; }
            set
            {
                // Uppdaterar namnet. //
                localWorkout.Name = value;

                // Meddelar UI om ändringen. //
                OnPropertyChanged(nameof(Name));
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

        // Fält för att styra om fälten ska vara låsta för redigering (default är låst). //
        private bool isReadOnly = true;
        private bool canSave = false;

        // Offentlig bindningsbar egenskap för att styra om fälten är redigerbara i UI. //
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;

                // Meddelar UI om ändringen. //
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public bool CanSave
        {
            get { return canSave; }
            set
            {
                canSave = value;
                OnPropertyChanged(nameof(CanSave));
            }
        }

        // ------------------------------ Konstruktor ------------------------------ //

        // Konstruktor som initierar AddWorkoutWindowViewModel. //
        public WorkoutsDetailsWindowViewModel(Workout workout, Window detailsWindow)
        {
            // Hämtar Singleton-instansen av UserManager för att säkerställa att samma användar- och datahantering delas över hela applikationen. //
            userManager = UserManager.Instance;

            // Sparar träningspasset som skickats in. //
            localWorkout = workout;

            // Sparar referens till fönstret. //
            _detailsWindow = detailsWindow;
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand EditCommand => new RelayCommand(execute => EditWorkout());
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveWorkout(), canExecute => CanSave);
        public RelayCommand CopyCommand => new RelayCommand(execute => CopyWorkout());
        public RelayCommand ExitCommand => new RelayCommand(execute => Exit());


        // ------------------------------ Metoder ------------------------------ //

        // Metod för att låsa upp textfälten så att de kan redigeras. //
        public void EditWorkout()
        {
            // Gör fälten redigerbara. //
            IsReadOnly = false;
            CanSave = true;
        }

        // Metod för att spara ändringar.
        public void SaveWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to save and go back?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Lås fälten igen. //
                IsReadOnly = true;
                CanSave = false;

                // Använd UpdateWorkout för att meddela ändringen i UserManager. //
                userManager.UpdateWorkout(localWorkout);

                // Öppnar upp WorkoutsWindow-fönstret. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();

                // Stänger ner WorkoutsDetailsWindow-fönstret. //
                _detailsWindow.Close();
            }    
        }

        // Kopierar träningspasset. //
        public void CopyWorkout()
        {
            // Kontrollera om träningspasset är av typen StrengthWorkout. //
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
            // Kontrollera om träningspasset är av typen CardioWorkout. //
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

        // Metod för att stänga fönstret och gå tillbaka.
        public void Exit()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to exit and go back?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Öppnar upp WorkoutsWindow-fönstret. //
                WorkoutsWindow work = new WorkoutsWindow();
                work.Show();

                // Stänger ner WorkoutsDetailsWindow-fönstret. //
                _detailsWindow.Close();
            }
        }
    }
}
