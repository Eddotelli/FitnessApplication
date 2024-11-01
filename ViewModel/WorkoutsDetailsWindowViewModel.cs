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

        // Alternativ för ComboBox i kolumnen "Type". //
        public ObservableCollection<string> WorkoutTypes { get; set; }

        // Lagrar den temporära kopian av träningspasset. //
        //private Workout originalWorkout;

        private Workout originalWorkout;

        public Workout OriginalWorkout
        {
            get { return originalWorkout; }
            set 
            { 
                originalWorkout = value;
                OnPropertyChanged();
            }
        }


        // ------------------------------ Egenskaper ------------------------------ //

        // Bindningsbara egenskaper för att visa och ändra information i UI. //
        private string name;
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

        private string type;
        public string Type
        {
            get { return localWorkout.TypeInput; }
            set
            {
                localWorkout.TypeInput = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private string duration;
        public TimeSpan Duration
        {
            get { return localWorkout.Duration; }
            set
            {
                localWorkout.Duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        private string caloriesBurned;
        public int CaloriesBurned
        {
            get { return localWorkout.CaloriesBurned; }
            set
            {
                localWorkout.CaloriesBurned = value;
                OnPropertyChanged(nameof(CaloriesBurned));
            }
        }

        private string notes;
        public string Notes
        {
            get { return localWorkout.Notes; }
            set
            {
                localWorkout.Notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        private string date;
        public DateTime Date
        {
            get { return localWorkout.Date; }
            set
            {
                localWorkout.Date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        // Egenskaper för Repetitions och Distance beroende på vilken typ av träningspass det är
        private string repetitons;
        public int Repetitions
        {
            get
            {
                if (localWorkout is StrengthWorkout strengthWorkout)
                {
                    return strengthWorkout.Repetitions;
                }
                return 0;
            }
            set
            {
                if (localWorkout is StrengthWorkout strengthWorkout)
                {
                    strengthWorkout.Repetitions = value;
                    OnPropertyChanged(nameof(Repetitions));
                }
            }
        }

        private string distance;
        public int Distance
        {
            get
            {
                if (localWorkout is CardioWorkout cardioWorkout)
                {
                    return cardioWorkout.Distance;
                }
                return 0;
            }
            set
            {
                if (localWorkout is CardioWorkout cardioWorkout)
                {
                    cardioWorkout.Distance = value;
                    OnPropertyChanged(nameof(Distance));
                }
            }
        }

        // Visibilitet för Edit och Restore knappar. //
        private Visibility editVisibility;
        public Visibility EditVisibility
        {
            get { return editVisibility; }
            set
            {
                editVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility restoreVisibility;
        public Visibility RestoreVisibility
        {
            get { return restoreVisibility; }
            set 
            { 
                restoreVisibility = value;
                OnPropertyChanged();
            }
        }

        // Aktiverar redigering i UI. //
        private bool isEnabled = false;  
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;

                // Meddelar UI om ändringen. //
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private bool canSave = false;
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

            // Lista över typer som visas i ComboBox för "Type"-kolumnen. //
            WorkoutTypes = new ObservableCollection<string>
            {
                "Strength", "Cardio"
            };

            RestoreVisibility = Visibility.Collapsed;

            CaloriesBurned = localWorkout.CalculateCaloriesBurned();

            // Skapa en kopia av workout för att kunna återställas. //
            originalWorkout = CloneWorkout(workout);
        }

        // ------------------------------ Kommando ------------------------------ //
        public RelayCommand EditCommand => new RelayCommand(execute => EditWorkout());
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveWorkout(), canExecute => CanSave);
        public RelayCommand CopyCommand => new RelayCommand(execute => CopyWorkout());
        public RelayCommand ExitCommand => new RelayCommand(execute => Exit());
        public RelayCommand RestoreCommand => new RelayCommand(execute => RestoreWorkout());


        // ------------------------------ Metoder ------------------------------ //     

        
        

        // Metod för att låsa upp textfälten så att de kan redigeras. //
        public void EditWorkout()
        {
            // Gör fälten redigerbara. //
            IsEnabled = true;
            CanSave = true;

            RestoreVisibility = Visibility.Visible;
            EditVisibility = Visibility.Collapsed;
        }

        // Metod för att spara ändringar.
        public void SaveWorkout()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to save and go back?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                
                // Lås fälten igen. //
                IsEnabled = true;
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

        // Återställer data. //
        private void RestoreWorkout()
        {
            EditVisibility = Visibility.Visible;
            RestoreVisibility = Visibility.Collapsed;

            // Kopierar data från originalWorkout tillbaka till localWorkout. //
            CopyWorkoutData(originalWorkout, localWorkout);

            // Uppdaterar knappens synlighet för redigering och återställning. //
            EditVisibility = Visibility.Visible;
            RestoreVisibility = Visibility.Collapsed;

            // Lås fälten och inaktivera spara-funktionen efter återställning. //
            IsEnabled = false;
            CanSave = false;

            // Meddela UI att data har ändrats. //
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(Duration));
            OnPropertyChanged(nameof(CaloriesBurned));
            OnPropertyChanged(nameof(Notes));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Repetitions));
            OnPropertyChanged(nameof(Distance));
        }

        // Kopierar träningspasset. //
        public void CopyWorkout()
        {
            userManager.CopiedWorkout = localWorkout;

            MessageBox.Show($"Workout copied '{userManager.CopiedWorkout.Name}' successfully!");
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

        // Skapar en kopia av träningspasset. //
        private Workout CloneWorkout(Workout workout)
        {
            if (workout is StrengthWorkout strengthWorkout)
            {
                return new StrengthWorkout(
                    strengthWorkout.Name,
                    strengthWorkout.Repetitions,
                    strengthWorkout.Date,
                    strengthWorkout.TypeInput,
                    strengthWorkout.Duration,
                    strengthWorkout.CaloriesBurned,
                    strengthWorkout.Notes
                );
            }
            else if (workout is CardioWorkout cardioWorkout)
            {
                return new CardioWorkout(
                    cardioWorkout.Name,
                    cardioWorkout.Distance,
                    cardioWorkout.Date,
                    cardioWorkout.TypeInput,
                    cardioWorkout.Duration,
                    cardioWorkout.CaloriesBurned,
                    cardioWorkout.Notes
                );
            }
            return null;
        }

        // Kopierar ursprungliga informationen på träningspasset. //
        private void CopyWorkoutData(Workout source, Workout target)
        {
            target.Name = source.Name;
            target.TypeInput = source.TypeInput;
            target.Duration = source.Duration;
            target.CaloriesBurned = source.CaloriesBurned;
            target.Notes = source.Notes;
            target.Date = source.Date;

            if (source is StrengthWorkout sourceStrength && target is StrengthWorkout targetStrength)
            {
                targetStrength.Repetitions = sourceStrength.Repetitions;
            }
            else if (source is CardioWorkout sourceCardio && target is CardioWorkout targetCardio)
            {
                targetCardio.Distance = sourceCardio.Distance;
            }
        }
    }
}
