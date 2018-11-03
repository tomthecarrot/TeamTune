using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {
    public AudioSource audio;
    void Start()
    {
        
    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            audio.enabled = true;
            audio.loop = true;
            Debug.Log(TeleportalPlayer.Current.gameObject.transform.position.x);
            audio.pitch = TeleportalPlayer.Current.gameObject.transform.position.x;
            audio.volume = TeleportalPlayer.Current.gameObject.transform.position.z;

        }
        else
        {

            audio.enabled = false;
            audio.loop = false;
        }
    }
}
