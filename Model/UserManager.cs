using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack.Model
{
    public class UserManager
    {
        private ObservableCollection<ListOfUsers> _users;

        public UserManager()
        {
            _users = new ObservableCollection<ListOfUsers>();
        }

        public ObservableCollection<ListOfUsers> Users => _users;

        public void AddUser(string username, string password)
        {
            _users.Add(new ListOfUsers(username, password));
        }

        // Lägg till andra användarrelaterade metoder här, t.ex. borttagning, uppdatering, etc.
    }

}
