const username = document.getElementById("username-input").value;
const recipient = document.getElementById("recipient-user").value;

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`/chathub?username=${encodeURIComponent(username)}`)
    .build();

connection.start().then(() => {
    console.log("SignalR Connected.");
}).catch(err => console.error("Error starting connection: " + err.toString()));

connection.on("ReceivePrivateMessage", (user, message) => {
    const msg = document.createElement("div");
    msg.setAttribute("class", "message received");
    msg.textContent = `${user}: ${message}`;
    document.getElementById("chat-messages").appendChild(msg);
    document.getElementById("chat-body").scrollTop = document.getElementById("chat-body").scrollHeight;
});
