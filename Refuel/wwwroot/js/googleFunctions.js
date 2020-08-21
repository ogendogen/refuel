var id_token;
onLoad();

function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    id_token = googleUser.getAuthResponse().id_token;
    localStorage.setItem("idtoken", id_token);
    console.log(id_token);
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.

    redirectToGoogleDelay();
}

function redirectToGoogleDelay() {
    window.location.href = "/Account/GoogleDelay";
}

function signOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.disconnect();
    auth2.signOut().then(function () {
        console.log('User signed out.');
        window.location.href = "/Account/Logout";
    });
}

function onSuccess(data) {
    if (data.status === "ok") {
        window.location.href = "../Panel/Index";
    }
    else {
        signOut();
    }
}

function onLoad() {
    gapi.load('auth2', function () {
        gapi.auth2.init();
        renderButton();
    });
}

function verifyIdToken() {
    $.ajax({
        type: 'POST',
        url: '/GoogleAuth/auth/google',
        data: JSON.stringify(localStorage.getItem("idtoken")),
        dataType: 'json',
        success: function (data) {
            onSuccess(data);
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        async: true
    });
}

function renderButton() {
    gapi.signin2.render('my-signin2', {
        'scope': 'profile email',
        'width': 240,
        'height': 50,
        'longtitle': true,
        'theme': 'dark',
        'onsuccess': onSignIn,
        'onfailure': onFailure
    });
}

function onFailure(error) {
    console.log(error);
}
