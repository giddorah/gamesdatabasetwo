$("#getSpecificGame").click(function () {
    let number = $("#gameId").val();

    $.ajax({
        url: '/api/games/getspecificgame',
        method: 'GET',
        data: { id: number }
    }).done(function (result) {
        console.log(result);
    })
});

$("#refillDatabase").click(function () {
    $.ajax({
        url: '/api/games/refilldatabase',
        method: 'GET',
    }).done(function (result) {
        console.log(result);
    })
});