using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicBtn : MonoBehaviour {

	private bool changed;
	public Sprite spriteOn;
	public Sprite spriteOff;
	private Button btn;

	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
		changed = MasterSoundController.instance.musicOn;
	}

	void Update () {
		if (changed != MasterSoundController.instance.musicOn) {
			changed = MasterSoundController.instance.musicOn;

			if (MasterSoundController.instance.musicOn == true) {
				gameObject.GetComponent<Image>().sprite = spriteOn;
			} else {
				gameObject.GetComponent<Image>().sprite = spriteOff;
			}
		}
	}

	void OnClick() {
		MasterSoundController.instance.musicOnOff();
	}
}
