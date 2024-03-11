function confirmAndPost(message, id, url, redirectUrl) {
    bootbox.confirm({
        title: 'Confirm',
        message: `Are you sure you want to ${message}?`,
        centerVertical: true,
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                post(id, url, redirectUrl);
            }
        }
    });
}

function post(id, url, redirectUrl) {
    var btn = document.getElementById(id);
    var dataAttributes = btn
        .getAttributeNames()
        .filter(x => x.includes('data'))
        .map(x => x.split('-')[1]);

    var data = {};
    dataAttributes.forEach(function (x) {
        data[x] = $(`#${btn.id}`).data(x);
    });

    $.ajax({
        url,
        type: "POST",
        dataType: 'json',
        data,
        success: function (response) {
            if (response.success) {
                if (redirectUrl) {
                    window.location.href = redirectUrl;
                } else {
                    window.location.reload();
                }
            } else {
                window.location.href = '/Error?error=' + response.error;
            }
        }
    });
}