using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

    public XRPlayerItem XRI;
    public AudioSource audio;
    public bool isSelf = true;
    public float networkSampleRate = 5f;
    public Transform transformYEET;
    public AudioMixerGroup mixer;
    public float zVal = 0f;

    bool isLead = false;

    public AudioClip snare0;
    public AudioClip snare1;
    public AudioClip snare2;

    public AudioClip lead;

    private int current = 0;

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
            this.audio.volume = 1f;
            switch (TeleportalAuth.Shared.Username)
            {
                case "snare":
                    changeDrums();
                    Debug.Log("fuck this snare");
                    break;
                case "lead":
                    changeLead();
                    Debug.Log("fuck this lead");
                    break;
                default:
                    changePitch();
                    Debug.Log("fuck this other");
                    break;
            }
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
        TeleportalProject.Shared.Send(string.Format("hold {0} {1} {2} {3}", TeleportalAuth.Shared.Username, this.audio.pitch, this.audio.volume, this.zVal));
    }

    private void changePitch() {
        Vector3 position;
        if (this.isSelf) {
            position = this.transformYEET.position;
        } else {
            position = new Vector3((float)(this.XRI.Longitude - -86.805816), 0f, (float)(this.XRI.Latitude - 36.143113));
        }
        this.audio.pitch = (position.x / 2) - ((position.x / 2) % 0.5f);
        this.zVal = Mathf.Abs(position.z);
        mixer.audioMixer.SetFloat("MyExposedParam 1", this.zVal * 10f);
        mixer.audioMixer.SetFloat("MyExposedParam 4", this.zVal * 20f);
    }

    private void changeLead() {
        Vector3 position = this.transformYEET.position;
        this.audio.pitch = (position.x / 2) - ((position.x / 2) % 0.5f);
        mixer.audioMixer.SetFloat("MyExposedParam 1", this.zVal * 10f);
        mixer.audioMixer.SetFloat("MyExposedParam 4", this.zVal * 20f);
        if (!isLead) {
          this.audio.clip = lead;
          this.audio.Play();
          isLead = true;
        }
    }

    private void changeDrums() {
        Vector3 position = this.transformYEET.position;
        this.audio.pitch = (position.x / 5) - ((position.x / 5) % 0.5f);

        Debug.Log(position.z);
        if (position.z > 10) {
            if(current != 2){
                current = 2;
                this.audio.clip = snare2;
                this.audio.Play();
            }
        } else if (position.z < -10) {
            if(current != 0){
                current = 0;
                this.audio.clip = snare0;
                this.audio.Play();
            }
        } else {
            if(current != 1){
                current = 1;
                this.audio.clip = snare1;
                this.audio.Play();
            }
        }
    }

    private void mute() {
        
    }
}
