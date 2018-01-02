$("#getSpecificGame").click(function () {
    let number = $("#gameId").val();

    $.ajax({
        url: '/api/games/getspecificgame',
        method: 'GET',
        data: { id: number }
    }).done(function (result) {
        console.log(result);
        let message = '<div class="alert alert-warning alert-dismissible fade show" role="alert">' +
            '<strong>Found: </strong>' + result.name +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
            '<span aria-hidden="true">&times;</span>' +
            '</button>' +
            '</div>';
        $("#showResults").html(message);
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

$("#getAllGames").click(function () {
    $.ajax({
        url: '/api/games/getAllGames',
        method: 'GET',
    }).done(function (result) {
        console.log(result);
        let message = '<table class="table table-striped table-dark">' +
            '<thead>' +
            '<tr>' +
            '<th scope="col">#</th>' +
            '<th scope="col">Name</th>' +
            '<th scope="col">Year</th>' +
            '<th scope="col">Platforms</th>' +
            '<th scope="col">Theme</th>' +
            '<th scope="col">Genre</th>' +
            '<th scope="col">Location</th>' +
            '<th scope="col">Publisher</th>' +
            '<th scope="col">Developer</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody>';
        let numberInList = 1;
        $.each(result, function (index, item) {
            message += '<tr>';
            message += '<th scope="row">' + numberInList + '</th>';
            message += '<td>' + item.name + '</td>';
            message += '<td>' + item.year + '</td>';
            message += '<td>' + item.platforms + '</td>';
            message += '<td>' + item.theme + '</td>';
            message += '<td>' + item.genre + '</td>';
            message += '<td>' + item.releasedWhere + '</td>';
            message += '<td>' + item.publisher + '</td>';
            message += '<td>' + item.developer + '</td>';
            message += '</tr>';
        });
        message += '</tbody ></table >'

        $("#showResults").html(message);
    })
});

$("#createGame").click(function () {
    let name = $("#gameName").val();
    let year = $("#gameYear").val();
    let platforms = $("#gamePlatforms").val();
    let theme = $("#gameTheme").val();
    let genre = $("#gameGenre").val();
    let releasedWhere = $("#gameReleasedWhere").val();
    let publisher = $("#gamePublisher").val();
    let developer = $("#gameDeveloper").val();

    console.log(developer);
    $.ajax({
        url: '/api/games/addgame',
        method: 'POST',
        data: { name: name, year: year, platforms: platforms, theme: theme, genre: genre, releasedWhere: releasedWhere, publisher: publisher, developer: developer }
    }).done(function (result) {
        console.log(result);
    })
});