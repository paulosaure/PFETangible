var express = require('express');
var app = express();
var httpserver = require('http').Server(app);
var io = require('socket.io').listen(httpserver);

var port = 8080;

app.get('/', function (req, res) {
  res.sendFile(__dirname + '/index.html');
});

// launch the http server on given port
httpserver.listen(port);

console.log("Server listening on *:" + port);

var tableSocket = null;
var tablets = [];

io.on('connection', function(socket){
    console.log("New Client Connection : " + socket.id);

	tablets[socket.id] = socket;
	
	//For test
	socket.on('sendToTablet', function(messageDescription){
        console.log("sendToTablet");
		if(typeof(tablets[messageDescription.tabletId]) != "undefined") {
			console.log("emit : " + messageDescription.event + " to " + messageDescription.tabletId);
			console.log(JSON.parse(messageDescription.value));
			tablets[messageDescription.tabletId].emit(messageDescription.event, JSON.parse(messageDescription.value));
		}
    });
	
    var username = "unknown";
	var isTable  = false;

	//Tablet actions
    socket.on('profil', function(profilDescription){
        console.log("Profil description :");
        console.log(profilDescription);
        username = profilDescription.username;
    });
	
	socket.on('username', function(usernameDescription){
        console.log("username");
        username = usernameDescription.username;
		if(tableSocket != null) {
			console.log("emit newTablet to SurfaceTable")
			tableSocket.emit("newTablet", {"id" : socket.id, "username" : username})
		}
    });
	
	//Surface Table actions
	
    socket.on('isTable', function(){
        console.log("isTable");
        isTable = true;
		tableSocket = tablets[socket.id];
		tablets[socket.id] = "";
		delete(tablets[socket.id]);
    });
	
	socket.on('setTabletViewport', function(viewportDescription){
        console.log("setTabletViewport");
        if(isTable) {
			if(typeof(tablets[viewportDescription.id]) != "undefined") {
				tablets[viewportDescription.id].emit("viewport", {"width" : viewportDescription.width, "height" : viewportDescription.height})
			}
		}
    });

    socket.on('disconnect', function(){
        console.log("Client disconnected : " + socket.id);
    });

    socket.on('error', function(errorData){
        console.log("An error occurred during Client connection : " + socket.id);
        console.log(errorData);
    });

    socket.on('reconnect', function(attemptNumber){
        console.log("Client Connection : " + socket.id + " after " + attemptNumber + " attempts.");
    });

    socket.on('reconnect_attempt', function(){
        console.log("Client reconnect attempt : " + socket.id);
    });

    socket.on('reconnecting', function(attemptNumber){
        console.log("Client Reconnection : " + socket.id + " - Attempt number " + attemptNumber);
    });

    socket.on('reconnect_error', function(errorData){
        console.log("An error occurred during Client reconnection : " + socket.id);
        console.log(errorData);
    });

    socket.on('reconnect_failed', function(){
        console.log("Failed to reconnect Client : " + socket.id + ". No new attempt will be done.");
    });
});