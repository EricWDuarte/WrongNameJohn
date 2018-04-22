using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public bool musicOn;
	private bool musicChanged;

	private AudioSource audioSource;
	public static SoundManager instance;

	public AudioClip backMusic;
	public AudioClip crazyMusic;

	void Start () {
		instance = this;
		audioSource = gameObject.GetComponent<AudioSource>();

		musicOn = MasterSoundController.instance.musicOn;
		musicChanged = musicOn;

		Background();
	}

	void Update () {
		musicOn = MasterSoundController.instance.musicOn;

		if (musicOn != musicChanged) {
			UpdateOnOff(musicOn);
			musicChanged = musicOn;
		}
			

	}

	public void Background () {
		
		audioSource.clip = backMusic;
		UpdateOnOff (musicOn);
	}

	public void CrazyMusic () {
		
		audioSource.clip = crazyMusic;
		UpdateOnOff (musicOn);
	}

	public void UpdateOnOff (bool on) {

		musicOn = on;

		if ( musicOn == false) {
			audioSource.Pause();
		} else {
			audioSource.Play();
		}
		

	}

}
