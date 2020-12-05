$(document).ready(function () {

    $("#btnsave").click(function () {
        $(".validateTips").html("");
        if ($("#txttitle").val().trim() == "") {
            $('#txttitle').nextAll(".validateTips:first").html("Enter Title");
            return false;
        }


    });
});