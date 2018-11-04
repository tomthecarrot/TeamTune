using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdProject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TeleportalActions.Shared.OnLocationLock += OnLocationLock;
	}

	public void OnLocationLock() {
		TeleportalProject.Shared.RegisterCommand("hold", Hold);	
	}
	
	public void Hold(List<string> args) {
		string name = args[1];
		float pitch = float.Parse(args[2]);
		float volume = float.Parse(args[3]);
		float zVal = float.Parse(args[4]);

		if (TeleportalAr.Shared.Items.ContainsKey(name)) {
			XRItem xri = TeleportalAr.Shared.Items[name];
			if (xri == null) { return; }

			PlaySound script = xri.transform.GetChild(0).GetComponent<PlaySound>();
			if (script == null) { return; }

			script.audio.pitch = pitch;
			script.audio.volume = volume;
			script.zVal = Mathf.Abs(zVal);

			if (script.mixer != null) {
				script.mixer.audioMixer.SetFloat("MyExposedParam 1", script.zVal * 10f);
				script.mixer.audioMixer.SetFloat("MyExposedParam 4", script.zVal * 20f);
			}

			switch (name)
            {
                case "snare":
                    script.changeDrums(false);
                    Debug.Log("f*** this snare");
                    break;
                case "lead":
                    script.changeLead(false);
                    Debug.Log("f*** this lead");
                    break;
                default:
                    script.changePitch(false);
                    Debug.Log("f*** this other");
                    break;
            }
		} 
        
    }
}
