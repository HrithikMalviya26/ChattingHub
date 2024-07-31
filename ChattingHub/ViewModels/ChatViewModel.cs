using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ChattingHub.Models;
using ChattingHub.Services;

namespace ChattingHub.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseService _firebaseService;

        // Observable collection of messages that will be displayed in the chat
        public ObservableCollection<ChatMessage> Messages { get; }

        // Command to handle sending messages
        public ICommand SendMessageCommand { get; }

        // Property for the new message text
        private string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                if (_newMessage != value)
                {
                    _newMessage = value;
                    OnPropertyChanged(nameof(NewMessage));
                }
            }
        }

        // Identifiers for the sender and receiver
        public string SenderId { get; }
        public string ReceiverId { get; }

        public ChatViewModel(string senderId, string receiverId)
        {
            _firebaseService = new FirebaseService();
            SenderId = senderId;
            ReceiverId = receiverId;
            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new Command(async () => await SendMessageAsync());

            // Load initial messages
            _ = LoadMessagesAsync();

            // Subscribe to real-time updates
            _firebaseService.SubscribeToMessages(SenderId, ReceiverId, OnMessageReceived);
        }

        private async Task LoadMessagesAsync()
        {
            try
            {
                var messages = await _firebaseService.GetMessagesAsync(SenderId, ReceiverId);

                // Clear existing messages and add new ones
                Messages.Clear();
                foreach (var message in messages)
                {
                    // Determine if the message is from the current user
                    message.IsCurrentUser = message.SenderId == SenderId;
                    Messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load messages: {ex.Message}", "OK");
                Console.WriteLine($"Exception in LoadMessagesAsync: {ex}");
            }
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Message cannot be empty", "OK");
                return;
            }

            try
            {
                var message = new ChatMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    SenderId = SenderId,
                    ReceiverId = ReceiverId,
                    Text = NewMessage,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), // Convert current time to Unix timestamp
                    IsCurrentUser = true // Mark as the current user's message
                };

                // Send the message to Firebase
                await _firebaseService.SendMessageAsync(SenderId, ReceiverId, message);

                // Clear the input field after sending
                NewMessage = string.Empty;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to send message: {ex.Message}", "OK");
                Console.WriteLine($"Exception in SendMessageAsync: {ex}");
            }
        }

        private void OnMessageReceived(ChatMessage message)
        {
            // Check if the message is null
            if (message == null)
            {
                // Log or handle the null message scenario
                Console.WriteLine("Received a null message.");
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                // Determine if the message is from the current user
                message.IsCurrentUser = message.SenderId == SenderId;

                // Add the message to the collection if it's not already present
                if (Messages.All(m => m.Id != message.Id))
                {
                    Messages.Add(message);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
