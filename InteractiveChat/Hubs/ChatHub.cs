using System.Collections.Concurrent;
using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace InteractiveChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IApplicationUserRepository _userRepository;
        
        // This dictionary stores the mapping of usernames to their connection IDs.
        private static ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();

        public ChatHub(IMessageRepository messageRepository, IConversationRepository conversationRepository, IApplicationUserRepository userRepository)
        {
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _conversationRepository = conversationRepository ?? throw new ArgumentNullException(nameof(conversationRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                if (httpContext == null)
                {
                    throw new Exception("HttpContext is null. Unable to retrieve the username from the query string.");
                }
                var username = Context.GetHttpContext().Request.Query["username"].ToString();
                if (string.IsNullOrEmpty(username))
                {
                    throw new NullReferenceException("Username is null or empty");
                }

                _userConnections[username] = Context.ConnectionId;
             

                // Send any pending messages
                var user =  _userRepository.GetByUsername(username);
                if (user == null)
                {
                    throw new NullReferenceException("User not found in the repository");
                }

                var pendingMessages = await _messageRepository.GetPendingMessagesForUserAsync(user);

                foreach (var message in pendingMessages)
                {
                    if (Context.ConnectionId == null)
                    {
                        throw new NullReferenceException("Context.ConnectionId is null");
                    }

                    await Clients.Client(Context.ConnectionId).SendAsync("ReceivePrivateMessage", message.Sender.UserName, message.Content);
                    message.Status = "delivered"; // Update message status
                    await _messageRepository.UpdateAsync(message); // Save changes
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnConnectedAsync: {ex.Message}");
                throw; // Re-throw the exception to let the SignalR pipeline handle it
            }

            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.GetHttpContext()?.Request.Query["username"].ToString();
            if (!string.IsNullOrEmpty(username))
            {
                _userConnections.TryRemove(username, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendPrivateMessage(string recipientUsername, string messageContent)
        {
            var senderUsername = Context.GetHttpContext().Request.Query["username"].ToString();
            var timestamp = DateTime.UtcNow;
            var conversation = GetConversation(senderUsername, recipientUsername);

            // Create and save message
            var message = new Message
            {
                Sender = conversation.Participants.FirstOrDefault(p => p.UserName == senderUsername),
                Recipient = conversation.Participants.FirstOrDefault(p => p.UserName == recipientUsername),
                Content = messageContent,
                Timestamp = timestamp,
                Status = _userConnections.ContainsKey(recipientUsername) ? "delivered" : "pending",
                ConversationId = conversation.ConversationId
            };
             _messageRepository.Add(message);

            // Send message to all participants
            foreach (var participant in conversation.Participants)
            {
                if (participant.UserName != senderUsername && _userConnections.TryGetValue(participant.UserName, out string connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", senderUsername, messageContent);
                }
            }
        }

        public Conversation GetConversation(string senderUsername, string recipientUsername)
        {
            var senderUser =  _userRepository.GetByUsername(senderUsername);
            var recipientUser =  _userRepository.GetByUsername(recipientUsername);
            var conversation =  _conversationRepository.GetByRecipients(senderUser, recipientUser);

            if (conversation == null)
            {
                var timestamp = DateTime.UtcNow;
                conversation = new Conversation();
                conversation.CreatedTimestamp = timestamp;
                conversation.Participants = new List<ApplicationUser> { senderUser, recipientUser };
                conversation.Title = "Conversation" + Guid.NewGuid().ToString(); // Generate a unique title
                _conversationRepository.Add(conversation);
            }
            return conversation;
        }
        
        
    }
}
