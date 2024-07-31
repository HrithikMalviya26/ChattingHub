using ChattingHub.Views;
using Microsoft.Maui.Controls;

namespace ChattingHub
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
