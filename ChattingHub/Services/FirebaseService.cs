using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChattingHub.Models;

namespace ChattingHub.Services
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient("https://fir-demo-586ab-default-rtdb.firebaseio.com/");
        }

        public async Task<User> GetUserAsync(string userId)
        {
            try
            {
                return await _firebaseClient
                    .Child("Users")
                    .Child(userId)
                    .OnceSingleAsync<User>();
            }
            catch (FirebaseException ex)
            {
                Console.WriteLine($"Firebase exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            try
            {
                await _firebaseClient
                    .Child("Users")
                    .Child(user.Id)
                    .PutAsync(user);
                Console.WriteLine("User registered successfully.");
                return true;
            }
            catch (FirebaseException ex)
            {
                Console.WriteLine($"Firebase exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendMessageAsync(string senderId, string receiverId, ChatMessage message)
        {
            try
            {
                var path = $"{senderId}_{receiverId}";

                await _firebaseClient
                    .Child("Messages")
                    .Child(path)
                    .Child(message.Id)
                    .PutAsync(message);

                var reversePath = $"{receiverId}_{senderId}";
                await _firebaseClient
                    .Child("Messages")
                    .Child(reversePath)
                    .Child(message.Id)
                    .PutAsync(message);

                Console.WriteLine("Message sent successfully.");
                return true;
            }
            catch (FirebaseException ex)
            {
                Console.WriteLine($"Firebase exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ChatMessage>> GetMessagesAsync(string userId, string otherUserId)
        {
            try
            {
                var path = $"{userId}_{otherUserId}";

                var messages = await _firebaseClient
                    .Child("Messages")
                    .Child(path)
                    .OnceAsync<ChatMessage>();

                return messages.Select(x => x.Object).OrderBy(m => m.Timestamp).ToList();
            }
            catch (FirebaseException ex)
            {
                Console.WriteLine($"Firebase exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var users = await _firebaseClient
                    .Child("Users")
                    .OnceAsync<User>();

                return users.Select(x => x.Object).ToList();
            }
            catch (FirebaseException ex)
            {
                Console.WriteLine($"Firebase exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public void SubscribeToMessages(string senderId, string receiverId, Action<ChatMessage> onMessageAdded)
        {
            // Subscribe to messages for the sender's view
            _firebaseClient
                .Child("Messages")
                .Child($"{senderId}_{receiverId}")
                .AsObservable<ChatMessage>()
                .Subscribe(d => onMessageAdded(d.Object));

            // Subscribe to messages for the receiver's view
            _firebaseClient
                .Child("Messages")
                .Child($"{receiverId}_{senderId}")
                .AsObservable<ChatMessage>()
                .Subscribe(d => onMessageAdded(d.Object));
        }
    }
}
