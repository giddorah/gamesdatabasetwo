let toggle;
$(function () {

    getAllDevelopers();
    getAllPublishers();

    $.ajax({
        url: '/users/returnrole',
        method: 'GET'
    }).done(function (result) {

        if (result == "Admin") {
            $("#adminButtons").html('<input type="text" id="gameId" /> <br />' +
                '<button class="btn btn-primary" id="getSpecificGame">GetGame</button> <br />' +
                '<button class="btn btn-primary" id="refillDatabase">Refill the database</button>' +
                '<button class="btn btn-danger" id="emptyDatabases">Empty databases</button> <br />');

            $("#getSpecificGame").click(function () {
                getSpecificGame();
            });

            $("#refillDatabase").click(function () {
                refillDatabase();
            });

            $("#emptyDatabases").click(function () {
                emptyDatabase();
            });
        }
        if (result == "Admin" || result == "Publisher") {
            generateCreateArea();
        }
    });



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
        });
    }

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
        });
    }
});

function getSpecificGame() {

    let number = $("#gameId").val();

    $.ajax({
        url: '/api/games/getspecificgame',
        method: 'GET',
        data: { id: number }
    }).done(function (result) {
        showModal(result);

    });
}

function refillDatabase() {

    $.ajax({
        url: '/api/games/refilldatabase',
        method: 'GET'
    }).done(function (result) {

    });
}

function emptyDatabase() {
    $.ajax({
        url: '/api/games/cleardatabase',
        method: 'POST'
    }).done(function (result) {

    });
}


function getAllGames(url) {

    toggle = !toggle;

    $.ajax({
        url: '/api/games/' + url,
        method: 'GET',
        data: { toggle: toggle }
    }).done(function (result) {
        let message = '<table class="table table-striped table-dark">' +
            '<thead>' +
            '<tr>' +
            '<th scope="col">#</th>' +
            '<th scope="col" id="sortByName">Name ↓↑</th>' +
            '<th scope="col" id="sortByYear">Year ↓↑</th>' +
            '<th scope="col">Platforms</th>' +
            '<th scope="col">Score</th>' +
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
            message += '<td>' + item.score.score.toFixed(2) + '</td>';
            message += '<td><span class="additional" id="' + item.name + '"data-title="Additional"><button class="btn btn-info">A</button></span></td>';
            message += '</tr>';
            numberInList++;
        });
        message += '</tbody ></table >';

        $("#showResults").html(message);
        $("#sortByName").click(function () {
            getAllGames("sortedByName");
        });
        $("#sortByYear").click(function () {

            getAllGames("sortedByYear");
        });
        $(".additional").click(function () {
            let gameNameToGet = this.id;

            $.ajax({
                url: '/api/games/getgamebyname',
                method: 'GET',
                data: { name: gameNameToGet }
            }).done(function (result) {
                showModal(result);

            });
        });
    });
}
$("#getAllGames").click(function () {
    getAllGames("getAllGames");
});

function showModal(result) {
    let footer = "";

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

    message += "<table class='table table-striped'>" +
        "<thead><tr>" +
        "<th scope='col'>Rating</th>" +
        "<th scope='col'>Votes</th>" +
        "</tr></thead><tbody><tr>" +
        "<td id='scoreCell'>" + result.score.score.toFixed(2) + " / 5</td>" +
        "<td id='votesCell'>" + result.score.votes + "</td>" +
        "</tr></tbody></table>";

    $(".modal-footer").html("");

    $.ajax({
        url: '/users/returnrole',
        method: 'GET'
    }).done(function (resultRole) {
        if (resultRole == "Admin" || resultRole == "User" || resultRole == "Publisher") {
            footer += '<div class="input-group input-group-sm mb-3">' +
                '<select class="custom-select my-1 mr-sm-2" id="chosenScore">' +
                '<option value="5">5</option>' +
                '<option value="4">4</option>' +
                '<option value="3">3</option>' +
                '<option value="2">2</option>' +
                '<option value="1">1</option>' +
                '</select>' +
                '</div>' +
                '<span class="sendScore" id="' + result.name + '"><button type="button" class="btn btn-primary">Vote</button></span>';
            if (resultRole == "Admin") {
               footer += '<div class="edit" id="' + result.name + '"><button type="button" class="btn btn-warning">Edit</button></div>' +
                    '<span class="delete" id="' + result.name + '"><button type="button" class="btn btn-danger">Delete</button></span>';
            }

            $(".modal-footer").html(footer);

            $(".edit").click(function () {
                $.ajax({
                    url: '/api/games/getgamebyname',
                    method: 'GET',
                    data: { name: result.unEditedName }
                }).done(function (result) {
                    showEditModal(result);
                });

            });

            $(".delete").click(function () {
                $.ajax({
                    url: '/api/games/removegame',
                    method: 'POST',
                    data: { name: this.id }
                }).done(function (result) {
                    alert(result);
                    $("#results").html('<div class="alert alert-danger alert-dismissible fade show" role="alert">'
                        + 'Game has been deleted.'
                        + '<button type= "button" class="close" data-dismiss="alert" aria-label="Close" >'
                        + '<span aria-hidden="true">&times;</span>'
                        + '</button >'
                        + '</div >');
                });
            });

            $(".sendScore").click(function () {
                let chosenGame = this.id;
                let chosenScore = $("#chosenScore").val();

                $.ajax({
                    url: '/api/games/addscore',
                    method: 'POST',
                    data: { name: chosenGame, score: chosenScore }
                }).done(function (resultScore) {
                    $("#scoreCell").text(resultScore.score.toFixed(2) + " / 5");
                    $("#votesCell").text(resultScore.votes);
                });
            });
        }
        
        $(".modal-footer").append('<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>');
    });

    

    //console.log(footer);
    //$(".modal-footer").html(footer);
    $(".modal-body").html(message);

    $('#detailsModal').modal('show');


}



function showEditModal(result) {
    //$('#detailsModal').modal('hide');
    let message = '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend" id="' + result.unEditedName + '">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Name:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="editGameName" value="' + result.name + '"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Year:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="editGameYear" value="' + result.year + '"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Platforms:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="editGamePlatforms" value="' + result.platforms + '"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Theme:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="editGameTheme" value="' + result.theme + '" />'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Genre:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="editGameGenre" value="' + result.genre + '" />'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Released where:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="editGameReleasedWhere" value="' + result.releasedWhere + '"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<select class="custom-select my-1 mr-sm-2" id="publisherEditSelectForm"></select>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<select class="custom-select my-1 mr-sm-2" id="developerEditSelectForm"></select>'
        + '</div>'
        + '</div >';

    let footer = '<span class="saveedit"><button type="button" class="btn btn-success" data-dismiss="modal">Save</button></span>' +
        '<span class="cancel" id="' + result.name + '"><button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button></span>';

    $(".modal-footer").html(footer);
    $(".modal-body").html(message);

    $('#detailsModal').modal('show');

    getAllDevelopers(result);
    getAllPublishers(result);

    function getAllDevelopers(defaultValue) {
        $.ajax({
            url: '/api/games/getdevelopers',
            method: 'GET'
        }).done(function (result) {
            let developerData = "<option selected>" + defaultValue.developer.name + "</option>";

            let number = 1;
            $.each(result, function (index, item) {
                developerData += '<option value="' + item.name + '">' + item.name + '</option>';
                number++;
            });
            $("#developerEditSelectForm").html(developerData);
        });
    }

    function getAllPublishers(defaultValue) {
        $.ajax({
            url: '/api/games/getpublishers',
            method: 'GET'
        }).done(function (result) {
            let publisherData = "<option selected>" + defaultValue.publisher.name + "</option>";

            let number = 1;
            $.each(result, function (index, item) {
                publisherData += '<option value="' + item.name + '">' + item.name + '</option>';
                number++;
            });
            $("#publisherEditSelectForm").html(publisherData);
        });
    }

    $(".saveedit").click(function () {
        let name = $("#editGameName").val();
        let year = $("#editGameYear").val();
        let platforms = $("#editGamePlatforms").val();
        let theme = $("#editGameTheme").val();
        let genre = $("#editGameGenre").val();
        let releasedWhere = $("#editGameReleasedWhere").val();
        let publisher = $("#publisherEditSelectForm").val();
        let developer = $("#developerEditSelectForm").val();


        $.ajax({
            url: '/api/games/editgame',
            method: 'POST',
            data: { nameOfGameToEdit: result.unEditedName, name: name, year: year, platforms: platforms, theme: theme, genre: genre, releasedWhere: releasedWhere, publisher: publisher, developer: developer }
        }).done(function (result) {

            $("#results").html('<div class="alert alert-success alert-dismissible fade show" role="alert">'
                + 'Game has been edited.'
                + '<button type= "button" class="close" data-dismiss="alert" aria-label="Close" >'
                + '<span aria-hidden="true">&times;</span>'
                + '</button >'
                + '</div >');

        }).fail(function (xhr, status, error) {

            let errorMessages = xhr.responseJSON;
            let concatinatedErrorMessages = "";
            $.each(errorMessages, function (index, item) {

                concatinatedErrorMessages += item[0] + " ";
            });

            $("#results").html('<div class="alert alert-danger alert-dismissible fade show" role="alert">'
                + concatinatedErrorMessages
                + '<button type= "button" class="close" data-dismiss="alert" aria-label="Close" >'
                + '<span aria-hidden="true">&times;</span>'
                + '</button >'
                + '</div >');
        });
    });
}

function generateCreateArea() {
    let message = '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend" id="create">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Name:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="gameName"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Year:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="gameYear"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Platforms:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="gamePlatforms"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Theme:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="gameTheme"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Genre:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="gameGenre"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Released where:'
        + '</span>'
        + '</div>'
        + '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="gameReleasedWhere"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<select class="custom-select my-1 mr-sm-2" id="publisherSelectForm"></select>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<select class="custom-select my-1 mr-sm-2" id="developerSelectForm"></select>'
        + '</div>'
        + '<button class="btn btn-primary" id="createGame">Create game</button>'
        + '</div >';

    getAllDevelopers();
    getAllPublishers();

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
        });
    }

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
        });
    }

    $("#createArea").html(message);
    $("#createGame").click(function () {
        createGame();
    });
}

function createGame() {
    console.log("test");
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

        $("#results").html('<div class="alert alert-success alert-dismissible fade show" role="alert">'
            + 'Game has been added.'
            + '<button type= "button" class="close" data-dismiss="alert" aria-label="Close" >'
            + '<span aria-hidden="true">&times;</span>'
            + '</button >'
            + '</div >');

    }).fail(function (xhr, status, error) {

        let errorMessages = xhr.responseJSON;
        let concatinatedErrorMessages = "";
        $.each(errorMessages, function (index, item) {

            concatinatedErrorMessages += item[0] + " ";
        });

        $("#results").html('<div class="alert alert-danger alert-dismissible fade show" role="alert">'
            + concatinatedErrorMessages
            + '<button type= "button" class="close" data-dismiss="alert" aria-label="Close" >'
            + '<span aria-hidden="true">&times;</span>'
            + '</button >'
            + '</div >');
    });
}