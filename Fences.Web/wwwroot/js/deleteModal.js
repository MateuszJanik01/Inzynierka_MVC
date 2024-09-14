// Modal Deleting

document.addEventListener('DOMContentLoaded', function () {
    var modal = document.getElementById("deleteModal");
    var closeModalBtn = document.getElementsByClassName("fa-xmark")[0];
    var cancelModalBtn = document.getElementsByClassName("btn-cancel")[0];
    var confirmDeleteBtn = document.getElementById("confirmDelete");

    var deleteUrl = ""; // URL to delete the order

    // Function to show the modal
    function showModal(orderId) {
        deleteUrl = `/Job/Delete/${orderId}`; // Update URL with order ID
        modal.style.display = "block";
    }

    // Function to close the modal
    function closeModal() {
        modal.style.display = "none";
    }

    // When the user clicks the "Delete" button
    document.querySelectorAll('.delete-button').forEach(function (deleteButton) {
        deleteButton.addEventListener('click', function () {
            var orderId = this.getAttribute('data-id');
            showModal(orderId);
        });
    });

    // When the user clicks on the "X" or Cancel button
    closeModalBtn.onclick = closeModal;
    cancelModalBtn.onclick = closeModal;

    // When the user confirms delete
    confirmDeleteBtn.onclick = function () {
        window.location.href = deleteUrl; // Redirect to delete action
    };

    // Close the modal if the user clicks outside of it
    window.onclick = function (event) {
        if (event.target == modal) {
            closeModal();
        }
    };
});

