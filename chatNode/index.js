var express = require("express");
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);

//Testi mielessä 
app.use(express.static(__dirname+'/ChatTesti'));

app.get('/', function(req, res){
    res.sendFile(__dirname+'/ChatTesti/index.html');
});

io.on('connection', function(socket){

    var address = socket.handshake.address;
    console.log('User connected: '+ address);

    socket.on('chat', function(msg){
      console.log('message:' + msg);
      io.emit('chat', {
        msg: msg
      });
    });
  });

http.listen(3000, function()
{
    console.log('listening on *:3000');
});