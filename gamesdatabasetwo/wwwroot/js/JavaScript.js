$("#changeSubmit").click(function () {
    let email = $("#emailChange").val();

    $.ajax({
        url: 'users/edit',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        console.log(result);
    })
});

$("#createUser").click(function () {
    let email = $("#userEmail").val();
    console.log(email);
    $.ajax({
        url: '/users/add',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        console.log(result);
    })
});

$("#logIn").click(function () {
    let email = $("#logInEmail").val();

    $.ajax({
        url: 'users/signin',
        method: 'POST',
        data: { email: email }
    }).done(function (result) {
        console.log(result);
    })
});

$("#logOut").click(function () {
    

    $.ajax({
        url: 'users/signout',
        method: 'POST',
        
    }).done(function (result) {
        console.log(result);
    })
});

$("#getAll").click(function () {


    $.ajax({
        url: 'users/getall',
        method: 'get',

    }).done(function (result) {
        console.log(result);
    })
});

$("#sortByEmail").click(function () {


    $.ajax({
        url: 'users/sort',
        method: 'get',

    }).done(function (result) {
        console.log(result);
    })
});