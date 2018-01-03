$(function () {

    getAllDevelopers();
    getAllPublishers();



    $("#publisherDropdown").html();

    function getAllDevelopers() {
        $.ajax({
            url: '/api/games/getdevelopers',
            method: 'GET'
        }).done(function (result) {
            let developerData = "<option selected>Choose Developer...</option>";

            let number = 1;
            $.each(result, function (index, item) {
                developerData += '<option value="' + item.name + '">' + item.name + '</option>';
                number++;
            });
            $("#developerSelectForm").html(developerData);
        })
    };

    function getAllPublishers() {
        $.ajax({
            url: '/api/games/getpublishers',
            method: 'GET'
        }).done(function (result) {
            let publisherData = "<option selected>Choose Publisher...</option>";

            let number = 1;
            $.each(result, function (index, item) {
                publisherData += '<option value="' + item.name + '">' + item.name + '</option>';
                number++;
            });
            $("#publisherSelectForm").html(publisherData);
        })
    };
})

$("#getSpecificGame").click(function () {
    let number = $("#gameId").val();

    $.ajax({
        url: '/api/games/getspecificgame',
        method: 'GET',
        data: { id: number }
    }).done(function (result) {
        showModal(result);
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
        let message = '<table class="table table-striped table-dark">' +
            '<thead>' +
            '<tr>' +
            '<th scope="col">#</th>' +
            '<th scope="col">Name</th>' +
            '<th scope="col">Year</th>' +
            '<th scope="col">Platforms</th>' +
            '<th scope="col">Additional info</th>' +
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
            message += '<td><span class="additional" id="' + item.name + '"data-title="Additional"><button class="btn btn-info">A</button></span></td>';
            message += '</tr>';
        });
        message += '</tbody ></table >'

        $("#showResults").html(message);

        $(".additional").click(function () {
            let gameNameToGet = this.id;

            $.ajax({
                url: '/api/games/getgamebyname',
                method: 'GET',
                data: { name: gameNameToGet }
            }).done(function (result) {
                showModal(result);
            })
        });
    })
});

function showModal(result) {
    let message = '<table class="table table-striped table-dark">' +
        '<thead>' +
        '<tr>' +
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
        '<tbody>' +
        '<tr>' +
        '<td>' + result.name + '</td>' +
        '<td>' + result.year + '</td>' +
        '<td>' + result.platforms + '</td>' +
        '<td>' + result.theme + '</td>' +
        '<td>' + result.genre + '</td>' +
        '<td>' + result.releasedWhere + '</td>' +
        '<td>' + result.publisher.name + '</td>' +
        '<td>' + result.developer.name + '</td>' +
        '</tr>' +
        '</tbody></table>';

    let footer = '<button type="button" id="'+result.name+'" class="btn btn-warning" data-dismiss="modal">Edit</button>' +
        '<button type="button" id="'+result.name+'" class="btn btn-danger" data-dismiss="modal">Delete</button>' +
        '<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>';

    $(".modal-footer").html(footer);

    $(".modal-body").html(message);
    $('#detailsModal').modal('show');

    $(".btn-warning").click(function () {

    })

    $(".btn-danger").click(function () {
        $.ajax({
            url: '/api/games/removegame',
            method: 'POST',
            data: { name: this.id }
        }).done(function (result) {
            alert(result);
        })

    })
}

$("#createGame").click(function () {
    let name = $("#gameName").val();
    let year = $("#gameYear").val();
    let platforms = $("#gamePlatforms").val();
    let theme = $("#gameTheme").val();
    let genre = $("#gameGenre").val();
    let releasedWhere = $("#gameReleasedWhere").val();
    let publisher = $("#publisherSelectForm").val();
    let developer = $("#developerSelectForm").val();

    $.ajax({
        url: '/api/games/addgame',
        method: 'POST',
        data: { name: name, year: year, platforms: platforms, theme: theme, genre: genre, releasedWhere: releasedWhere, publisher: publisher, developer: developer }
    }).done(function (result) {
        
    })
});