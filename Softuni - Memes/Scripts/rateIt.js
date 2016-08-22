$('.rate').on('click', function () {
    var rankId = $(this).prev().children().last()[0].id;
    var score = $("#" + rankId).val();
    alert(score);
    /*$.ajax({
        url: "/",
        type: "post",
        data: serializedData
    })*/
});