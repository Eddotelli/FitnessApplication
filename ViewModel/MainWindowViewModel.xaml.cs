using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitTrack.ViewModel
{
    /// <summary>
    /// Interaction logic for MainWindowViewModel.xaml
    /// </summary>
    public partial class MainWindowViewModel : Window
    {
        public MainWindowViewModel()
        {
            InitializeComponent();
        }

        // ---------- Egenskaper ---------- //
        public string LabelTitle {  get; set; }
        public string UsernameInput { get; set; }
        public string PasswordInput { get; set; }

        // ---------- Konstruktor ---------- //
        public MainWindowViewModel(string LabelTitle, string UsernameInput, string PasswordInput)
        {
            this.LabelTitle = LabelTitle;
            this.UsernameInput = UsernameInput;
            this.PasswordInput = PasswordInput;
        }

        // ------------------------------ Metoder ------------------------------ //
        public void SignIn()
        {

        }

        public void Register()
        {

        }
    }
}
