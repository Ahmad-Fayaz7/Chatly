// Get the input element by its ID
const inputElement = document.getElementById('message-input');

// Add an event listener for the keypress event
inputElement.addEventListener('keypress', function(event) {
    // Check if the pressed key is Enter (key code 13)
    if (event.key === 'Enter') {
        // Prevent the default action (if any)
        event.preventDefault();

        // Call the sendMessage function
        sendMessage();
    }
});

function sendMessage() {
    const message = document.getElementById("message-input").value.trim();
    if (message === "") {
        return; // Do not send empty messages
    }
    if (connection && connection.state === signalR.HubConnectionState.Connected) {
        console.log("Message is sending...");
        connection.invoke("SendPrivateMessage", recipient, message)
            .then(() => {
                const msg = document.createElement("div");
                msg.setAttribute("class", "bg-primary message sent");
                msg.textContent = `You: ${message}`;
                document.getElementById("chat-messages").appendChild(msg);
                document.getElementById("chat-body").scrollTop = document.getElementById("chat-body").scrollHeight;
                document.getElementById("message-input").value = ""; // Clear the input
            })
            .catch(err => console.error("Error sending message: " + err.toString()));
    } else {
        console.error("Connection not established. Unable to send message.");
    }
}

// Scroll to the bottom after adding all messages
document.getElementById("chat-body").scrollTop = document.getElementById("chat-body").scrollHeight;