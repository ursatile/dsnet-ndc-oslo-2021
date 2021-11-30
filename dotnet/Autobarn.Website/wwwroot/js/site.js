// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(connectToSignalR);

function connectToSignalR() {
    console.log('Connecting to SignalR');
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on('DisplayNotification',
        (user, message) => {
            console.log(user);
            console.log(message);
            console.log('===========================');
        });
    conn.start().then(function() {
        console.log('SignalR connected and working!');
    }).catch(function(err) {
        console.log('ERROR starting signalR: ' + err);
    });
}
