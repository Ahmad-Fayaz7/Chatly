$(document).ready(function () {
    // Use event delegation to handle click events for dynamically updated elements
    $(document).on('click', '.friendActionButton', function (e) {
        e.preventDefault();

        const button = $(this); // Store the clicked button
        const username = button.data("username"); // Get the username from the button's data attribute
        let actionUrl = '';
        let newButtonText = '';
        let newButtonId = '';

        // Determine the action based on the button's ID
        if (button.attr("id") === 'addFriendButton') {
            actionUrl = friendshipUrls.sendFriendRequest;
            newButtonText = 'Cancelar';
            newButtonId = 'cancelRequestButton';
        } else if (button.attr("id") === 'cancelRequestButton') {
            actionUrl = friendshipUrls.cancelFriendRequest;
            newButtonText = 'Agregar Amigo';
            newButtonId = 'addFriendButton';
        } else if (button.attr("id") === 'acceptRequestButton') {
            actionUrl = friendshipUrls.acceptFriendRequest;
            newButtonText = 'Eliminar';
            newButtonId = 'unfriendButton';
        } else if (button.attr("id") === 'rejectRequestButton') {
            actionUrl = friendshipUrls.rejectFriendRequest;
            newButtonText = 'Agregar Amigo';
            newButtonId = 'addFriendButton';
        } else if (button.attr("id") === "unfriendButton") {
            actionUrl = friendshipUrls.unfriend;
            newButtonText = 'Agregar Amigo';
            newButtonId = 'addFriendButton';
        }

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: { username: username }, // Include the username in the data sent to the server
            success: function (response) {
                // Handle success - update the UI
                if (response.success) {
                    // Check if the button is a reject button
                    if (button.attr("id") === 'rejectRequestButton') {
                        // Remove the accept button which is next to the reject button
                        button.siblings("#acceptRequestButton").remove();
                    } else if (button.attr("id") === 'acceptRequestButton') {
                        // Remove the accept button which is next to the reject button
                        button.siblings("#rejectRequestButton").remove();
                    }
                    button.fadeOut('fast', function () {
                        button.text(newButtonText)
                            .attr("id", newButtonId)
                            .fadeIn('fast');
                    });
                } else {
                    alert('Error: ' + response.message);
                }
            },
            error: function (error) {
                // Handle error - notify the user
                alert('An error occurred. Please try again.');
            }
        });
    });
});
