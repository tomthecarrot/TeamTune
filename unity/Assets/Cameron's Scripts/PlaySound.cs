﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

    public XRPlayerItem XRI;
    public AudioSource audio;
    public bool isSelf = true;
    public int networkSampleRate = 5;
    public Transform transformYEET;

    void Start() {
        this.audio = this.gameObject.GetComponent<AudioSource>();
        this.XRI = this.gameObject.transform.parent.gameObject.GetComponent<XRPlayerItem>();
        this.isSelf = (this.XRI == null);
        if (this.isSelf) {
            this.transformYEET = TeleportalPlayer.Current.gameObject.transform;
        } else {
            this.transformYEET = this.gameObject.transform.parent; // XR Player Item
        }
        StartCoroutine(this.sendUpdatesC());
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && this.isSelf) {
            this.audio.volume = 0f;
        } else if (Input.GetMouseButton(0) && this.isSelf) {
            this.changePitch();
        }
    }

    private IEnumerator sendUpdatesC() {
        while (true) {
            if (this.isSelf) {
                this.sendUpdate();
            }
            yield return new WaitForSeconds(1 / this.networkSampleRate);
        }
    }

    private void sendUpdate() {
        TeleportalProject.Shared.Send(string.Format("hold {0} {1} {2}", TeleportalAuth.Shared.Username, this.audio.pitch, this.audio.volume));
    }

    private void changePitch() {
        Vector3 position = this.transformYEET.position;
        this.audio.pitch = position.x;
        this.audio.volume = position.z;
    }
}
