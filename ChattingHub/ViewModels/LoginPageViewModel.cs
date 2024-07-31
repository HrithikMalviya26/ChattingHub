using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ChattingHub.Models;
using ChattingHub.Services;
using ChattingHub.Views;

namespace ChattingHub.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseService _firebaseService;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginPageViewModel()
        {
            _firebaseService = new FirebaseService();
            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        private async Task LoginAsync()
        {
            try
            {
                var user = await _firebaseService.GetUserAsync(Username);

                if (user != null && user.Password == Password)
                {
                    // Navigate to ContactsPage with the current user's ID
                    await Application.Current.MainPage.Navigation.PushAsync(new ContactsPage(Username));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid username or password.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle or log the error as appropriate
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task RegisterAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
