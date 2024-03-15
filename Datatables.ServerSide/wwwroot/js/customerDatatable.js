$(document).ready(function () {
       
    $('#searchInput').on('keyup', function () {
        dataTable.search(this.value).draw();
    });


    $('#openPopupButton').on('click', function () {
        $('#popupContent').dialog({
            modal: true,
            width: 400,
            buttons: {
                Submit: function () {
                    var firstName = $('#popupFirstName').val();
                    var lastName = $('#popupLastName').val();
                }
            }
        });
    });

    $('#customerForm').submit(function (e) {
        e.preventDefault(); // Prevent default form submission
        var formData = {
            firstName: $('#firstName').val(),
            lastName: $('#lastName').val(),
            contact: $('#contact').val(),
            email: $('#email').val(),
            dateOfBirth: $('#dateOfBirth').val(),
            read: $('#read').val(),
            write: $('#write').val(),
        };

        // Submit form data via AJAX
        $.ajax({
            type: 'POST',
            url: '/api/customer/update',
            data: JSON.stringify(formData),
            success: function (response) {
                // Handle success response
                console.log(response);
                $('#popupForm').hide(); // Hide popup after successful submission
            },
            error: function (xhr, status, error) {
                // Handle error response
                console.error(xhr.responseText);
            }
        });
    });


    $('#saveButton').click(function () {
        var customers = [];

        // Iterate over each row in the grid
        $('#customerDatatable tbody tr').each(function () {

            var dateOfBirthString = $(this).closest('tr').find('td:eq(5)').text().trim();
            var dateParts = dateOfBirthString.split('-');
            var dateOfBirth = new Date(dateParts[0], dateParts[1]-1, parseInt(dateParts[2]));

            var customer = {
                Id: parseInt($(this).closest('tr').find('td:eq(0)').text()),
                FirstName: $(this).closest('tr').find('td:eq(1)').text(),
                LastName: $(this).closest('tr').find('td:eq(2)').text(),
                Contact: $(this).closest('tr').find('td:eq(3)').text(),
                Email: $(this).closest('tr').find('td:eq(4)').text(),
                DateOfBirth: dateOfBirth,
                Read: $(this).closest('tr').find('td:eq(6) input[type="checkbox"][id^="read"]').prop('checked'),
                Write: $(this).closest('tr').find('td:eq(7) input[type="checkbox"][id^="write"]').prop('checked'),
            };
            customers.push(customer);
        });

        $.ajax({
            url: 'api/customer/saveCustomers', // Replace with your controller action URL
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(customers),
            success: function (response) {
                // Handle success
                alert('Data saved successfully');
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error('Error saving data:', error);
            }
        });
    });

});  