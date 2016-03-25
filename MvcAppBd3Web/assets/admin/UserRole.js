$(document).ready(function () {

});
function Validation() {
    var RoleName = $('#RoleName').val();
    var Description = $('#Description').val();

    if (RoleName.length == 0) {
        alert('Nhập tên quyền!');
        return false;
    } else if (Description.length == 0) {
        alert('Nhập mô tả quyền!');
        return false;
    } else return true;
}