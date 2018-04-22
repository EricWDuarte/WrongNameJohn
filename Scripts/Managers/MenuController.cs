using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public AudioSource menuMusic;

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			PlayGame();
		}
	}

	public void PlayGame() {
		SceneManager.LoadScene("Gameplay");
	}

	public void MusicOnOff () {
		if (MasterSoundController.instance.musicOn)
		{
			menuMusic.Play();
		} else {
			menuMusic.Stop();
		}
	}



}
