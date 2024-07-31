using Microsoft.Maui.Controls;
using ChattingHub.Models;
using ChattingHub.ViewModels;

namespace ChattingHub.Views
{
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage(string currentUserId)
        {
            InitializeComponent();
            BindingContext = new ContactsViewModel(currentUserId);
        }

        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is User selectedUser)
            {
                var viewModel = BindingContext as ContactsViewModel;
                var senderId = viewModel.CurrentUserId;
                var receiverId = selectedUser.Id;

                var chatPage = new ChatPage(senderId, receiverId);

                await Navigation.PushAsync(chatPage);
            }
        }
    }
}
