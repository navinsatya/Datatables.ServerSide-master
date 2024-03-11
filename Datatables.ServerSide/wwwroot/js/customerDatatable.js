$(document).ready(function () {
    $("#customerDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "ordering": false,
        //"paging": false,
        //"filter": true,
        "ajax": {
            "url": "/api/customer/create",
            "type": "POST",
            "datatype": "json"
        },
        //"columnDefs": [{
        //    "targets": [0],
        //    "visible": false,
        //    "searchable": false
        //}],
        "columns": [
            {
                "data": "id", "name": "Id", "autoWidth": true, "render": function (data, type, full, meta) {
                    return '<label id="id_' + data + '" >' + data + ' </label>';
                }
            },
            {
                "data": "firstName", "name": "FirstName", "autoWidth": true, "render": function (data, type, full, meta) {
                    return '<label id="firstName_' + full.id + '" >' + data + ' </label>';
                }
            },
            {
                "data": "lastName", "name": "LastName", "auto   Width": true, "render": function (data, type, full, meta) {
                    return '<label type="label" id="lastName_' + full.id + '" >' + data + ' </label>';
                }
            },
            {
                "data": "contact", "name": "Contact", "autoWidth": true, "render": function (data, type, full, meta) {
                    return '<label type="label" id="contact_' + full.id + '" >' + data + ' </label>';
                }
            },
            {
                "data": "email", "name": "Email", "autoWidth": true, "render": function (data, type, full, meta) {
                    return '<label type="label" id="email_' + full.id + '" >' + data + ' </label>';
                }
            },
            {
                "data": "dateOfBirth", "name": "DateOfBirth", "autoWidth": true, "render": function (data, type, full, meta) {
                    return '<label type="label" id="dateOfBirth_' + full.id + '" >' + data + ' </label>';
                }
            },
            {
                "data": "read",
                "name": "Read",
                "autoWidth": true,
                "render": function (data, type, full, meta) {
                    return '<input type= "checkbox" id="read_' + full.id + '" ' + (data ? ' checked' : '') + '  >';
                }
            },
            {
                "data": "write",
                "name": "Write",
                "autoWidth": true,
                "render": function (data, type, full, meta) {
                    return '<input type="checkbox" id="write_' + full.id + '"  ' + (data ? ' checked' : '') + '  >';
                }
            }
        ]
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