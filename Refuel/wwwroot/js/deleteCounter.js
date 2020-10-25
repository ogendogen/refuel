var seconds = 3;

$(document).ready(function () {

    setTimeout(deleteButtonHandler, 1000);

});

function deleteButtonHandler()
{
    seconds--;
    if (seconds > 0)
    {
        $("#deleteButton").html("Usuń (" + seconds + ")");
        setTimeout(deleteButtonHandler, 1000);
        return;
    }

    $("#deleteButton").html("Usuń");
    $("#deleteButton").removeAttr("disabled");
    $("#deleteButton").removeClass("cursor-notallowed");
}