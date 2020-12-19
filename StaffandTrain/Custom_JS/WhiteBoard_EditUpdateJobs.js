$(document).ready(function () {

    $("#btnsave").click(function () {
        $(".validateTips").html("");
        if ($("#txtNotes").val().trim() == "") {
            $('#txtNotes').nextAll(".validateTips:first").html("Please enter notes");
            return false;
        }

    });
});