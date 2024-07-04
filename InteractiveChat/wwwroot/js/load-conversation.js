async function loadConversation(senderUsername, recipientUsername) {
    try {
        const response = await fetch(`/api/message/conversation?senderUsername=${senderUsername}&recipientUsername=${recipientUsername}`);
        const messages = await response.json();
        console.log(messages);
        const chatMessagesContainer = document.getElementById("chat-messages");
        chatMessagesContainer.innerHTML = ""; // Clear the container before loading messages

        messages.forEach(message => {
            const msg = document.createElement("div");
            msg.setAttribute("class", message.senderUser === username ? "bg-primary message sent" : "message received");
            msg.textContent = `${message.recipientUser}: ${message.content}`;
            chatMessagesContainer.appendChild(msg);
        });

        // Scroll to the bottom after adding all messages
        document.getElementById("chat-body").scrollTop = document.getElementById("chat-body").scrollHeight;
    } catch (error) {
        console.error('Error loading conversation:', error);
    }
}

// Example usage
loadConversation(username, recipient);