$(document).ready(function () {

});

function toBlock(status, id) {
    var r = null;;
    if (status == 1) {
        r = confirm("Khóa tài khoản này?")
    } else {
        r = confirm("Mở tài khoản này?")
    }
    if (r == true) {
        $.ajax({
            cache: false,
            async: true,
            type: 'POST',
            url: "/Account/block/" + id,
            data: "",
            error: function (error) {
                console.log(error);
            },
            dataType: 'json',
            success: function (data) {
                if (data.Success) {
                    if (data.Status == 1) {
                        $('#idviewStatus-' + id).html('Đang hoạt động');
                        $('#idviewAction-' + id).text('Tạm khóa');
                    } else {
                        $('#idviewStatus-' + id).html('Tạm khóa');
                        $('#idviewAction-' + id).text('Mở hoạt động');
                    }
                    alert(" Đã xử lý tài khoản này");
                }
            }

        });
    }
}

function checkmail() {
    var emailaddress = $('#Email').val();
    if (validateEmail(emailaddress)) {
        $('#btn-submit').click();
        return true;
    } else {
        alert('Mail không đúng định dạng');
        return false;
    }
}