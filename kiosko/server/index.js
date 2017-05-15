var express=require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var clients = {};

app.use(express.static('public'));

app.get('/', function(req, res){
  res.sendFile(__dirname + '/index.html');
});

io.on('connection', function(socket){
  console.log("New connection!!");
  socket.emit("message","bienvenido!");
  socket.on('message', function(msg){
    console.log("new message: " + msg);
    socket.broadcast.emit('message', msg);
  });
});
http.listen(3000, function(){
  console.log('listening on:3000');
});

