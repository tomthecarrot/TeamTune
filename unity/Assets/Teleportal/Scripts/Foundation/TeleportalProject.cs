// Teleportal SDK
// Code by Thomas Suarez
// Copyright 2018 WiTag Inc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportalProject : MonoBehaviour {

	/* Shared Object References */
	public static TeleportalProject Shared;

	public string ApiKey = "";
	public string Id = "";

	void Awake () {
		// Set static self reference
		TeleportalProject.Shared = this;

		// Load Teleportal scene
		SceneManager.LoadScene("Teleportal", LoadSceneMode.Additive);
	}

	public void Send(params string[] msg) {
		// Relay to TeleportalNet
		TeleportalNet.Shared.Send(TeleportalProject.Shared.Id, msg);
	}

	public void RegisterCommand(string cmd, System.Action<List<string>> func) {
		TeleportalNet.Shared.RegisterCommand(this.Id, cmd, func);
	}

}
