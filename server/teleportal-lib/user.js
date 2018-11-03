/**
Teleportal SDK
Code by Thomas Suarez.
Copyright 2018 WiTag Inc.
**/

users = {}; // [name:User]
locationTokens = {}; // [token:User]
locationTokenIndex = 0; // newest index of location token array

// Changes user reference from IP/port to name
// Then initializes a user object and adds to users dictionary
initUser = function initUser(name) {
  users[name] = new User(name); // instantiate user object
  return users[name];
}

///// MAIN CLASS /////

User = class User {

  constructor(name) {
    // Add to global users dictionary
    users[name] = this;

    this.name = name;
    this.connectTime = Date.now();
    this.latitude = 0.0;
    this.longitude = 0.0;

    ///// ADD HERE - LISTENER FOR USER CONNECTION /////

    // Listen for this user's future disconnection
    teleportalHookAdd(TELEPORTAL_HOOKSCOPE_DISCONNECT, name);
  }

  disconnect() {
    // Remove from global users dictionary
    delete users[this.name];

    // Log total time user was connected
    var uptime = Math.ceil((Date.now() - this.connectTime) / 1000);
    console.log(this.name + " DISCONNECT", uptime.toString() + " sec uptime");

    // Stop listening for this user's disconnection
    teleportalHookRemove(TELEPORTAL_HOOKSCOPE_DISCONNECT, this.name);

    ///// ADD HERE /////
  }

  send(msg) {
    teleportalConn.send(TELEPORTAL_MODULE_ID + " " + this.name + " " + msg + "\n");
  }

  ///// ADD HERE /////

}
