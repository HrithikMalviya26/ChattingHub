using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ChattingHub.ViewModels
{
    public class RegistrationPageViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand RegisterCommand { get; }

        public RegistrationPageViewModel()
        {
            RegisterCommand = new Command(OnRegisterCommandExecuted);
        }

        private async void OnRegisterCommandExecuted()
        {
            // Handle registration logic here
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
