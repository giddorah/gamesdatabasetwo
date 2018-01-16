let toggle;

let fromDivToSpan = '</div><div class="input-group input-group-sm mb-3"><div class="input-group-prepend"><span class="input-group-text" id="inputGroup-sizing-sm">';
let spanDiv = '</span></div>';
let formControlBeginningWithoutSpanDiv = '<input class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm" type="text" id="';
let formControlBeginning = spanDiv + formControlBeginningWithoutSpanDiv;

$(function () {

    generateContent();

    $("#publisherDropdown").html();
});
function changeSubmit() {
    let email = $("#emailChange").val();

    $.ajax({
        url: 'users/edit',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {

        ShowStatus(result, 0);
    }).fail(function (xhr, status, error) {
        ShowStatus(xhr.responseText, 1);

    });
}

function ShowStatus(contents, statusType) {
    var alertStyle = "";
    var formattedStatus = "";

    if (statusType === 0) {
        alertStyle = '"alert alert-success alert-dismissible fade show"';
    }
    else if (statusType === 1) {
        alertStyle = '"alert alert-danger alert-dismissible fade show"';
    }

    formattedStatus = '<div class=' + alertStyle + ' role="alert">'
        + contents
        + '<button type= "button" class="close" data-dismiss="alert" aria-label="Close" >'
        + '<span aria-hidden="true">&times;</span>'
        + '</button >'
        + '</div >';

    $("#results").html(formattedStatus);
}

function removeUser(email) {


    $.ajax({
        url: '/users/remove',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        ShowStatus(result, 0);

    }).fail(function (xhr, status, error) {
        ShowStatus(xhr.responseText, 1);

    });
}

function createUser() {
    let email = $("#userEmail").val();

    $.ajax({
        url: '/users/add',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        ShowStatus(result, 0);

    }).fail(function (xhr, status, error) {
        ShowStatus(xhr.responseText, 1);

    });
}

function logIn() {
    let email = $("#logInEmail").val();

    $.ajax({
        url: 'users/signin',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        ShowStatus(result, 0);

        generateContent();
    }).fail(function (xhr, status, error) {
        ShowStatus(xhr.responseText, 1);

    });
}

function logOut() {


    $.ajax({
        url: 'users/signout',
        method: 'POST'

    }).done(function (result) {
        ShowStatus(result, 0);

        generateContent();
    });
}

function getAllUsers(url) {
    toggle = !toggle;
    console.log(toggle);
    $.ajax({
        url: 'users/' + url,
        method: 'GET',
        data: { toggle: toggle }

    }).done(function (result) {

        let message = '<table class="table table-striped table-dark">' +
            '<thead>' +
            '<tr>' +
            '<th scope="col">#</th>' +
            '<th scope="col" id="sortByEmail">Email ↓↑</th>' +
            '<th scope="col"> Role </th>' +
            '<th scope="col">Remove</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody>';
        let numberInList = 1;
        $.each(result, function (index, item) {
            message += '<tr>';
            message += '<th scope="row">' + numberInList + '</th>';
            message += '<td>' + item.email + '</td>';
            message += '<td>' + item.role + '</td>';
            message += '<td> <button id="' + item.email + '" class="btn btn-danger removeUser">X</button ></td > ';
            message += '</tr>';
            numberInList++;

        });
        message += '</tbody ></table >';

        $("#showResults").html(message);
        $(".removeUser").click(function () {
            removeUser(this.id);
        });
        $("#sortByEmail").click(function () {

            getAllUsers("sortbyemail");
        });


    });
}


function generateContent() {
    $("#userContent").empty();
    $("#adminButtons").empty();
    $("#createArea").empty();
    $.ajax({
        url: '/users/returnrole',
        method: 'GET'
    }).done(function (result) {

        if (result == "Anonymous") {
            $("#userContent").html('<input type="text" id="logInEmail" />' +
                '<button class ="btn btn-primary btn-sm" id="logIn">Log in</button><br />' +
                '<input id="userEmail" type="text" />' +
                '<button class="btn btn-success btn-sm" id="createUser">Register</button> <br />');

        }
        if (result == "User") {
            $("#userContent").html('<input id="emailChange" type="text" />' +
                '<button class="btn btn-warning btn-sm" id="changeSubmit">Change email</button ><br />' +
                '<button class="btn btn-danger btn-sm" id="logOut">Log out</button> <br />');
           
        }

        if (result == "Staff") {
            $("#userContent").html(' <input id="emailChange" type="text" />' +
                '<button class="btn btn-warning btn-sm" id="changeSubmit">Change email</button ><br />' +
                '<button class="btn btn-danger btn-sm" id="logOut">Log out</button><br />');
           
            generateCreateArea();
        }

        if (result == "Admin") {
            $("#userContent").html('<input id="userEmail" type="text" />' +
                '<button class ="btn btn-success btn-sm" id="createUser">Create staff</button> <br />' +
                '<button class="btn btn-primary btn-sm" id="getAll">Get all users</button><br />' +
                '<button class="btn btn-danger btn-sm" id="logOut">Log out</button><br />');
            $("#adminButtons").html('<input type="text" id="gameId" /> <br />' +
                '<button class="btn btn-primary btn-sm" id="getSpecificGame">GetGame</button> <br />' +
                '<button class="btn btn-primary btn-sm" id="refillDatabase">Refill the database</button>' +
                '<button class="btn btn-danger btn-sm" id="emptyDatabases">Empty databases</button> <br />');
            generateCreateArea();

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

        $("#changeSubmit").click(function () {
            changeSubmit();
        });


        $("#createUser").click(function () {
            createUser();
        });

        $("#logIn").click(function () {
            logIn();

        });

        $("#logOut").click(function () {
            logOut();
            $("#showResults").empty();

        });

        $("#getAll").click(function () {
            getAllUsers("getall");
        });




    });
}

function getAllDevelopers(editOrCreate) {
    let developerData = "";

    $.ajax({
        url: '/api/games/getdevelopers',
        method: 'GET'
    }).done(function (result) {
        let number = 1;
        if (editOrCreate === "create") {
            let developerData = "<option selected>Choose Developer...</option>";
            $.each(result, function (index, item) {
                developerData += '<option value="' + item.name + '">' + item.name + '</option>';
                number++;
            });
            $("#developerSelectForm").html(developerData);

        }
        else {
            let developerData = "<option selected>" + editOrCreate.developer.name + "</option>";
            $.each(result, function (index, item) {
                developerData += '<option value="' + item.name + '">' + item.name + '</option>';
                number++;
            });
            $("#developerEditSelectForm").html(developerData);

        }
    });
}

function getAllPublishers(editOrCreate) {
    let publisherData = "";

    $.ajax({
        url: '/api/games/getpublishers',
        method: 'GET'
    }).done(function (result) {
        if (editOrCreate === "create") {
            publisherData = "<option selected>Choose Publisher...</option>";
        }
        else {
            publisherData = "<option selected>" + editOrCreate.publisher.name + "</option>";
        }
        let number = 1;
        $.each(result, function (index, item) {
            if (editOrCreate === "create") {
                publisherData += '<option value="' + item.name + '">' + item.name + '</option>';
            }
            else {
                publisherData += '<option value="' + item.name + '">' + item.name + '</option>';
            }
        });
        if (editOrCreate === "create") {
            $("#publisherSelectForm").html(publisherData);
        }
        else {
            $("#publisherEditSelectForm").html(publisherData);
        }
        number++;
    });
}


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
            message += '<td style="width: 50px"><span class="additional" id="' + item.name + '"data-title="Additional"><button class="btn btn-info">A</button></span></td>';
            message += '</tr>';
            numberInList++;
        });
        message += '</tbody></table>';

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
        if (resultRole == "Admin" || resultRole == "User" || resultRole == "Staff") {

            if (result.score.id > -1) {
                footer += '<div class="input-group input-group-sm mb-3" id="voteArea">' +
                    '<select class="custom-select my-1 mr-sm-2" id="chosenScore">' +
                    '<option value="5">5</option>' +
                    '<option value="4">4</option>' +
                    '<option value="3">3</option>' +
                    '<option value="2">2</option>' +
                    '<option value="1">1</option>' +
                    '</select>' +
                    '</div>' +
                    '<span class="sendScore" id="' + result.name + '"><button id="sendscorebutton" type="button" class="btn btn-primary">Vote</button></span>';
            }

            if (resultRole == "Admin") {
                footer += '<div class="edit" id="' + result.name + '"><button type="button" class="btn btn-warning">Edit</button></div>' +
                    '<span class="delete" id="' + result.name + '"><button type="button" class="btn btn-danger">Delete</button></span>';
            }
            if (resultRole == "Staff") {
                footer += '<div class="edit" id="' + result.name + '"><button type="button" class="btn btn-warning">Edit</button></div>';
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
                    ShowStatus(result, 1);

                });
            });

            $(".sendScore").click(function () {
                let chosenGame = this.id;
                let chosenScore = $("#chosenScore").val();
                $("#voteArea").html("");
                $("#sendscorebutton").prop("disabled", true);
                $("#sendscorebutton").hide();


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

    $(".modal-body").html(message);

    $('#detailsModal').modal('show');

}



function showEditModal(result) {
    let message = '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend" id="' + result.unEditedName + '">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Name:'
        + spanDiv
        + formControlBeginningWithoutSpanDiv + 'editGameName" value="' + result.name + '"/>'
        + fromDivToSpan
        + 'Year:'
        + formControlBeginning + 'editGameYear" value="' + result.year + '"/>'
        + fromDivToSpan
        + 'Platforms:'
        + formControlBeginning + 'editGamePlatforms" value="' + result.platforms + '"/>'
        + fromDivToSpan
        + 'Theme:'
        + formControlBeginning + 'editGameTheme" value="' + result.theme + '" />'
        + fromDivToSpan
        + 'Genre:'
        + formControlBeginning + 'editGameGenre" value="' + result.genre + '" />'
        + fromDivToSpan
        + 'Released where:'
        + formControlBeginning + 'editGameReleasedWhere" value="' + result.releasedWhere + '"/>'
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
            ShowStatus(result, 0);


        }).fail(function (xhr, status, error) {

            let errorMessages = xhr.responseJSON;
            let concatinatedErrorMessages = "";
            $.each(errorMessages, function (index, item) {

                concatinatedErrorMessages += item[0] + " ";
            });
            ShowStatus(concatinatedErrorMessages, 1);

        });
    });
}

function generateCreateArea() {
    let message = '<div class="input-group input-group-sm mb-3">'
        + '<div class="input-group-prepend" id="create">'
        + '<span class="input-group-text" id="inputGroup-sizing-sm">'
        + 'Name:'
        + spanDiv
        + formControlBeginningWithoutSpanDiv + 'gameName" />'
        + fromDivToSpan
        + 'Year:'
        + formControlBeginning +'gameYear"/>'
        + fromDivToSpan
        + 'Platforms:'
        + formControlBeginning +'gamePlatforms"/>'
        + fromDivToSpan
        + 'Theme:'
        + formControlBeginning + 'gameTheme"/>'
        + fromDivToSpan
        + 'Genre:'
        + formControlBeginning + 'gameGenre"/>'
        + fromDivToSpan
        + 'Released where:'
        + formControlBeginning + 'gameReleasedWhere"/>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<select class="custom-select my-1 mr-sm-2" id="publisherSelectForm"></select>'
        + '</div>'
        + '<div class="input-group input-group-sm mb-3">'
        + '<select class="custom-select my-1 mr-sm-2" id="developerSelectForm"></select>'
        + '</div>'
        + '<button class="btn btn-primary" id="createGame">Create game</button>'
        + '</div >';

    getAllDevelopers("create");
    getAllPublishers("create");

    $("#createArea").html(message);
    $("#createGame").click(function () {
        createGame();
    });
}

function createGame() {

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
        ShowStatus(result, 0);


    }).fail(function (xhr, status, error) {

        let errorMessages = xhr.responseJSON;
        let concatinatedErrorMessages = "";
        $.each(errorMessages, function (index, item) {

            concatinatedErrorMessages += item[0] + " ";
        });
        ShowStatus(concatinatedErrorMessages, 1);

    });
}