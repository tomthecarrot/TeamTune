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
        // this.mixer = (AudioMixerGroup) GameObject.Find("AudioMixerControl");
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
                    changeDrums(true);
                    Debug.Log("fuck this snare");
                    break;
                case "lead":
                    changeLead(true);
                    Debug.Log("fuck this lead");
                    break;
                default:
                    changePitch(true);
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

    public void changePitch(bool sound) {
        if (sound) {
            Vector3 position = this.getPosition();
            this.audio.pitch = (position.x / 2) - ((position.x / 2) % 0.5f);
            this.zVal = Mathf.Abs(position.z);
            mixer.audioMixer.SetFloat("MyExposedParam 1", this.zVal * 10f);
            mixer.audioMixer.SetFloat("MyExposedParam 4", this.zVal * 20f);
        }
    }

    public void changeLead(bool sound) {
        if (sound) {
            Vector3 position = this.getPosition();
            this.audio.pitch = (position.x / 2) - ((position.x / 2) % 0.5f);
            mixer.audioMixer.SetFloat("MyExposedParam 1", this.zVal * 10f);
            mixer.audioMixer.SetFloat("MyExposedParam 4", this.zVal * 20f);
        }
        if (!isLead) {
          this.audio.clip = lead;
          this.audio.Play();
          isLead = true;
        }
    }

    public void changeDrums(bool sound) {
        Vector3 position = this.getPosition();
        
        if (sound) {
            this.audio.pitch = (position.x / 5) - ((position.x / 5) % 0.5f);
        }

        if (position.z > 10) {
            if (current != 2){
                current = 2;
                this.audio.clip = snare2;
                this.audio.Play();
            }
        } else if (position.z < -10) {
            if (current != 0){
                current = 0;
                this.audio.clip = snare0;
                this.audio.Play();
            }
        } else {
            if (current != 1){
                current = 1;
                this.audio.clip = snare1;
                this.audio.Play();
            }
        }
    }

    private Vector3 getPosition() {
        if (this.isSelf) {
            return this.transformYEET.position;
        } else {
            return new Vector3((float)(this.XRI.Longitude - -86.805816), 0f, (float)(this.XRI.Latitude - 36.143113));
        }
    }

    private void mute() {
        
    }
}
