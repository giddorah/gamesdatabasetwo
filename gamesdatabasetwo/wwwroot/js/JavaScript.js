$(function () {
    
    generateContent();

    
   });
function generateContent() {
    $("#userContent").empty();
    $.ajax({
        url: '/users/returnrole',
        method: 'GET'
    }).done(function (result) {
        if (result == "Anonymous") {
            $("#userContent").html('<input type="text" id="logInEmail" />' +
                '<button id="logIn">Log in</button><br />' +
                '<input id="userEmail" type="text" />' +
                '<button id="createUser">Register</button> <br />');

        }

        if (result == "Publisher" || result == "User") {
            $("#userContent").html('<button id="logOut">Log out</button><br />' +
                ' <input id="emailChange" type="text" />' +
                '<button id="changeSubmit">Change email</button ><br />');
        }

        if (result == "Admin") {
            $("#userContent").html('<input id="userEmail" type="text" />' +
                '<button id="createUser">Create publisher</button> <br />' +
                '<button id="getAll">Get all users</button><br />' +
                '<button id="sortByEmail">Sort by email</button><br />' +
                '<input type="text" id="removeUserEmail" />' +
                '<button id="removeUser">Remove user</button ><br />' +
                '<button id="logOut">Log out</button><br />');
        }

        $("#changeSubmit").click(function () {
            changeSubmit();
        });

        $("#removeUser").click(function () {
            removeUser();
        });

        $("#createUser").click(function () {
            createUser();
        });

        $("#logIn").click(function () {
            logIn();

        });

        $("#logOut").click(function () {
            logOut();

        });

        $("#getAll").click(function () {
            getAll();
        });

        $("#sortByEmail").click(function () {
            sortByEmail();
        });


    });
}
function changeSubmit(){
    let email = $("#emailChange").val();

    $.ajax({
        url: 'users/edit',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        console.log(result);
    });
}

function removeUser () {
    let email = $("#removeUserEmail").val();
    console.log(email);
    $.ajax({
        url: '/users/remove',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        console.log(result);
    });
}

function createUser () {
    let email = $("#userEmail").val();
    console.log(email);
    $.ajax({
        url: '/users/add',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        console.log(result);
    });
}

function logIn () {
    let email = $("#logInEmail").val();

    $.ajax({
        url: 'users/signin',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        console.log(result);
        generateContent();
    });
}

function logOut () {


    $.ajax({
        url: 'users/signout',
        method: 'POST'

    }).done(function (result) {
        console.log(result);
        generateContent();
    });
}

function getAll () {


    $.ajax({
        url: 'users/getall',
        method: 'GET'

    }).done(function (result) {
        console.log(result);
    });
}

function sortByEmail() {


    $.ajax({
        url: 'users/sortbyemail',
        method: 'GET'

    }).done(function (result) {
        console.log(result);
    });
}