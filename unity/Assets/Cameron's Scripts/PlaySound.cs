using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

    private AudioSource audio;
    private bool isSelf = true;
    private Transform transformYEET;

    void Start() {
        this.audio = gameObject.GetComponent<AudioSource>();
        this.isSelf = (this.gameObject.transform.parent.GetComponent<XRPlayerItem>() == null);
        if (this.isSelf) {
            this.transformYEET = TeleportalPlayer.Current.gameObject.transform;
        } else {
            this.transformYEET = this.gameObject.transform.parent; // XR Player Item
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && this.isSelf) {
            this.changePitch();
        } else if (!this.isSelf) {
            this.changePitch();
        } else {
            this.audio.enabled = false;
            this.audio.loop = false;
        }
    }

    private void changePitch() {
        Vector3 position = this.transformYEET.position;
        this.audio.enabled = true;
        this.audio.loop = true;
        Debug.Log(position.x);
        this.audio.pitch = position.x;
        this.audio.volume = position.z;
    }
}
