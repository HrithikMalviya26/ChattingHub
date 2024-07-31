using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using ChattingHub.Models;
using ChattingHub.Services;

namespace ChattingHub.ViewModels
{
    public class ContactsViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseService _firebaseService;
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public string CurrentUserId { get; }

        public ContactsViewModel(string currentUserId)
        {
            _firebaseService = new FirebaseService();
            CurrentUserId = currentUserId;
            LoadUsers();
        }

        private async void LoadUsers()
        {
            var users = await _firebaseService.GetUsersAsync();
            foreach (var user in users)
            {
                if (user.Id != CurrentUserId) // Exclude the current user
                {
                    Users.Add(user);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
