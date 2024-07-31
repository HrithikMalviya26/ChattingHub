using Microsoft.Maui.Controls;
using ChattingHub.ViewModels;

namespace ChattingHub.Views
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage(string senderId, string receiverId)
        {
            InitializeComponent();
            BindingContext = new ChatViewModel(senderId, receiverId);
        }
    }
}
