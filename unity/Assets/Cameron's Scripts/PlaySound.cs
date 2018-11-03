using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

    public XRPlayerItem XRI;
    private AudioSource audio;
    public bool isSelf = true;
    private Transform transformYEET;

    void Start() {
        this.audio = this.gameObject.GetComponent<AudioSource>();
        this.XRI = this.gameObject.transform.parent.gameObject.GetComponent<XRPlayerItem>();
        this.isSelf = (this.XRI == null);
        if (this.isSelf) {
            this.transformYEET = TeleportalPlayer.Current.gameObject.transform;
        } else {
            this.transformYEET = this.gameObject.transform.parent; // XR Player Item
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.isSelf) {
            this.enableSound();
            
            if (this.isSelf) {
                TeleportalProject.Shared.Send(string.Format("hold {0} 1", TeleportalAuth.Shared.Username));
            }
        } else if (Input.GetMouseButtonUp(0) && this.isSelf) {
            this.disableSound();

            if (this.isSelf) {
                TeleportalProject.Shared.Send(string.Format("hold {0} 0", TeleportalAuth.Shared.Username));
            }
        }

        if (Input.GetMouseButton(0) && this.isSelf) {
            this.changePitch();
        } else if (!this.isSelf) {
            this.changePitch();
        }
    }

    public void enableSound() {
        this.audio.enabled = true;
        this.audio.loop = true;
    }

    public void disableSound() {
        this.audio.enabled = false;
        this.audio.loop = false;
    }

    private void changePitch() {
        Vector3 position = this.transformYEET.position;
        this.audio.pitch = position.x;
        this.audio.volume = position.z;
    }
}
