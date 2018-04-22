using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSoundController : MonoBehaviour {

	public static MasterSoundController instance;

	public bool musicOn = true;
	public bool sfxOn = true;

	void Awake () {
		MakeSingleton();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MakeSingleton () {
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void musicOnOff () {
		musicOn = !musicOn; 
	}

	public void sfxOnOff () {
		sfxOn = !sfxOn;
	}
}
