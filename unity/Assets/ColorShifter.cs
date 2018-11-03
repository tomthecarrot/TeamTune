using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShifter : MonoBehaviour {

  Light lt;
	// Use this for initialization
	void Start () {
		lt = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		lt.color -= (Color.HSVToRGB(0.3f, 0.0f, 0.0f, true) / 2.0f) * Time.deltaTime;
	}
}
