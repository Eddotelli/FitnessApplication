using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FitTrack.Model
{
    public class UserManager
    {
        private ObservableCollection<ListOfUsers> users;

        public UserManager()
        {
            users = new ObservableCollection<ListOfUsers>();
        }

        public ObservableCollection<ListOfUsers> Users => users;

        public void AddUser(string username, string password, string Country)
        {
            users.Add(new ListOfUsers(username, password, Country));         
        }

        // Lägg till andra användarrelaterade metoder här, t.ex. borttagning, uppdatering, etc.
    }

    //public class UserManager
    //{
    //    public ObservableCollection<ListOfUsers> Users { get; } = new ObservableCollection<ListOfUsers>();

    //    public void AddUser(string username, string password)
    //    {
    //        Users.Add(new ListOfUsers { Username = username, Password = password });
    //    }
    //}
}
