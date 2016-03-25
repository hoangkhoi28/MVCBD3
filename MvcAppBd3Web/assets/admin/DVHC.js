$(document).ready(function () {

});
function Validation() {
    var RoleName = $('#RoleName').val();
    var Description = $('#Description').val();

    if (RoleName.length == 0) {
        alert('Nhập tên đơn vị hành chính!');
        return false;
    } else if (Description.length == 0) {
        alert('Nhập mô tả đơn vị hành chính!');
        return false;
    } else return true;
}