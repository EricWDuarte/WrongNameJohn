using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SfxBtn : MonoBehaviour {

	private bool changed;
	public Sprite spriteOn;
	public Sprite spriteOff;
	private Button btn;

	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
		changed = MasterSoundController.instance.sfxOn;
	}

	void Update () {
		if (changed != MasterSoundController.instance.sfxOn) {
			changed = MasterSoundController.instance.sfxOn;

			if (MasterSoundController.instance.sfxOn == true) {
				gameObject.GetComponent<Image>().sprite = spriteOn;
			} else {
				gameObject.GetComponent<Image>().sprite = spriteOff;
			}
		}
	}

	void OnClick() {
		MasterSoundController.instance.sfxOnOff();
	}
}
