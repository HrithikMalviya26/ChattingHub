using ChattingHub.ViewModels;
using Microsoft.Maui.Controls;

namespace ChattingHub.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModel();
        }
    }
}
 