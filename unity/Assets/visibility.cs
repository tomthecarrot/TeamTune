using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visibility : MonoBehaviour {

	// Use this for initialization
	void Start () {
    TeleportalActions.Shared.OnLocationLock += OnLocationLock;
	}

  void OnLocationLock() {
    if (TeleportalAuth.Shared.Username != "snare") {
      this.gameObject.SetActive(false);
    }
  }
}
