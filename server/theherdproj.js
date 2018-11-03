/**
The Herd Project - VandyHacks
Dervied from the Teleportal Module Example
**/

// Load libraries
Teleportal = require('./teleportal-lib/teleportal');

// Define Teleportal API Key
// (generated from the Teleportal Developer Dashboard)
TELEPORTAL_API_KEY = "TslmoAjczv";

// Define commands
// (must be the same as the ones you define in the Unity code)
CMD_HOLD = "hold";

// Tracked players
players = {};

// Send a message to all tracked players
sendPlayers = function(msg) {
  // Iterate through the players
  for (name of Object.keys(players)) {
      if (players[name] != null) {
          // Send the message
          teleportalSend(players[name], msg);
      }
  }
}

// Execute command with arguments
parse = function(user, cmd, args, str) {

  switch (cmd) {

    case CMD_HOLD:
      if (players[user.name] == null) {
          players[user.name] = user;
      }
      var newArgs = str.split(' ');
      newArgs.splice(0, 1);
      var newStr = newArgs.join(' ');
      console.log(newStr);
      sendPlayers(newStr);
      break;

  }
}
