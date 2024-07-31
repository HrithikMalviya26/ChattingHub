using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using ChattingHub.Models;
using ChattingHub.Services;

namespace ChattingHub.Views
{
    public partial class RegistrationPage : ContentPage
    {
        private readonly FirebaseService _firebaseService;

        public RegistrationPage()
        {
            InitializeComponent();
            _firebaseService = new FirebaseService();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            var user = new User
            {
                Id = username,
                Username = username,
                Password = password
            };

            bool success = await _firebaseService.RegisterUserAsync(user);

            if (success)
            {
                await DisplayAlert("Registration Successful", "You can now login", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                await DisplayAlert("Registration Failed", "Please try again", "OK");
            }
        }
    }
}
