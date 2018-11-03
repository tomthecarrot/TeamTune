/**
Teleportal SDK
Code by Thomas Suarez.
Copyright 2018 WiTag Inc.
**/

WebSocket = require('ws');
lib_user = require('./user');

Debug = false;
// Debug = true;

var TeleportalHost = "sa.teleportal.app";
var TeleportalPort = "443";

if (Debug) {
  TeleportalHost = "127.0.0.1";
  TeleportalPort = "8081";
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
}

teleportalConn = new WebSocket("wss://" + TeleportalHost + ":" + TeleportalPort);

teleportalConn.on('open', function open() {
  // Identify self as a module server
  teleportalConn.send(TELEPORTAL_CMD_EXTERNAL_APP_IDENTIFY + " " + TELEPORTAL_API_KEY);
});

// Handle client data
teleportalConn.on('message', function incoming(message) {
  // console.log("Received message: %s", message);
  try {
    message = message.toString();
    message = message.split("\n")[0]; // remove newline character
    message = message.replace(/\*/g, ' ');
    var args = message.split(" "); // split message by whitespace
    var name = args[0]; // get user name (first argument)
    var cmd = args[1]; // get command (second argument)
    args.shift(); // remove username from arguments list
    args.shift(); // remove command from arguments list

    // Get the user (if it currently exists)
    var user = users[name];
    if (user == null) {
      user = initUser(name);
    }

    // Parse message & execute
    teleportalParse(user, cmd, args, message);

  } catch (err) {
    console.log("ERR_DATA " + err);
  }
});

teleportalHookAdd = function(scope, arg) {
  teleportalConn.send(TELEPORTAL_CMD_HOOK_ADD + " " + scope + " " + arg);
}

teleportalHookRemove = function(scope, arg) {
  teleportalConn.send(TELEPORTAL_CMD_HOOK_REMOVE + " " + scope + " " + arg);
}

// Execute command with arguments
teleportalParse = function(user, cmd, args, str) {

  switch (cmd) {

    case TELEPORTAL_CMD_API_KEY_OK:
      console.log("[TP] Connected!");

      break;

    case TELEPORTAL_CMD_API_KEY_BAD:
      console.log("[TP] Invalid API Key!");

      break;

    case TELEPORTAL_CMD_DISCONNECT:
      user.disconnect();

      break;

    case TELEPORTAL_CMD_LOCATION_UPDATE:
      // Get values
      var name = args[0];
      var lat = args[1];
      var lon = args[2];

      // Set the current location
      users[name].latitude = lat;
      users[name].longitude = lon;

      break;

  }

  // Send all commands to module, regardless of whether
  // they are teleportal or module commands
  parse(user, cmd, args, str);

}

// Send message to client user
teleportalSend = function(user, msg) {
  try {
    teleportalConn.send(TELEPORTAL_CMD_EXTERNAL_APP_SEND + " " + user.name + " " + msg + "\n");
  }
  catch (err) {
    console.log("SEND ERROR: " + err);
  }
}

// Commands
TELEPORTAL_CMD_EXTERNAL_APP_IDENTIFY = "aa0";
TELEPORTAL_CMD_EXTERNAL_APP_SEND = "aa1";
TELEPORTAL_CMD_API_KEY_OK = "a1";
TELEPORTAL_CMD_API_KEY_BAD = "a2";
TELEPORTAL_CMD_DISCONNECT = "a10";
TELEPORTAL_CMD_LOCATION_UPDATE = "e4";
TELEPORTAL_CMD_LOCATION_SYNC = "e3";
TELEPORTAL_CMD_HOOK_ADD = "a6";
TELEPORTAL_CMD_HOOK_REMOVE = "a7";

// Hook Scopes
TELEPORTAL_HOOKSCOPE_LOCATION_UPDATE = "1";
TELEPORTAL_HOOKSCOPE_DISCONNECT = "2";
TELEPORTAL_HOOKSCOPE_LOCATION_SYNC = "3";

// Hook Arguments
TELEPORTAL_HOOKARG_ALLUSERS = "-1";
