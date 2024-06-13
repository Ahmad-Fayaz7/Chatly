$(document).ready(function () {
    $(".friendActionButton").click(function (e) {
        e.preventDefault();

        const button = $(this); // Store the clicked button
        const username = button.data("username"); // Get the username from the button's data attribute
        let actionUrl = '';
        if (button.attr("id") === 'acceptRequestButton') {
            actionUrl = friendshipUrls.acceptFriendRequest;
        }
        else if (button.attr("id") === 'rejectRequestButton') {
            actionUrl = friendshipUrls.rejectFriendRequest;
        }

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: { username: username }, // Include the username in the data sent to the server
            success: function (response) {
                // Handle success - update the UI
                if (response.success) {
                    if (button.attr("id") === 'acceptRequestButton') {
                        alert("Accepted successfully!");
                    }
                    // Check if the button is a reject button
                    else if (button.attr("id") === 'rejectRequestButton') {
                        alert("Rejected successfully!");
                    }

                    button.parent('li').remove();
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