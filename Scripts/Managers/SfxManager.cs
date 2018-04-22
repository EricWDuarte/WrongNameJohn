using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour {

	public bool SfxOn;
	public float pitchVar;

	private AudioSource audioSource;
	public static SfxManager instance;

	public AudioClip ShootSfx;
	public AudioClip ShootImpactSfx;
	public AudioClip BlockExplodeSfx1;
	public AudioClip BlockExplodeSfx2;
	public AudioClip BlockImpactSfx;
	public AudioClip JumpSfx;
	public AudioClip FallSfx;
	public AudioClip laserSfx;

	void Start () {
		instance = this;
		audioSource = gameObject.GetComponent<AudioSource>();

		SfxOn = MasterSoundController.instance.sfxOn;

		UpdateOnOff (SfxOn);
	}

	void Update () {
		SfxOn = MasterSoundController.instance.sfxOn;
	}

	public void Shoot () {

		audioSource.pitch = Random.Range(1.8f - pitchVar, 1.8f + pitchVar);
		audioSource.PlayOneShot(ShootSfx, 0.05f);
		UpdateOnOff (SfxOn);
	}

//	public void ShootImpact () {
//
//		audioSource.pitch = Random.Range(1f - pitchVar, 1f + pitchVar);
//		audioSource.PlayOneShot(ShootImpactSfx, 0.2f);
//		UpdateOnOff (SfxOn);
//	}

	public void BlockExplode (int size) {

		audioSource.pitch = Random.Range(0.8f - pitchVar, 0.8f + pitchVar);
		audioSource.PlayOneShot(Random.Range(0f, 1f) < 0.5f ? BlockExplodeSfx1 : BlockExplodeSfx2, 0.3f);
		UpdateOnOff (SfxOn);
	}

	public void BlockImpact () {

		audioSource.pitch = Random.Range(1f - pitchVar, 1f +  pitchVar);
		audioSource.PlayOneShot(BlockImpactSfx, 0.1f);
		UpdateOnOff (SfxOn);
	}

	public void Jump () {

		audioSource.pitch = Random.Range(1.5f - pitchVar, 1.5f + pitchVar);
		audioSource.PlayOneShot(JumpSfx, 0.8f);
		UpdateOnOff (SfxOn);
	}

	public void Fall () {

		audioSource.pitch = Random.Range(1f - pitchVar, 1f + pitchVar);
		audioSource.PlayOneShot(FallSfx, 0.2f);
		UpdateOnOff (SfxOn);
	}

	public void Laser () {

		audioSource.pitch = Random.Range(1f - pitchVar, 1f + pitchVar);
		audioSource.PlayOneShot(laserSfx, 0.5f);
		UpdateOnOff (SfxOn);
	}
		
	public void UpdateOnOff (bool on) {

		SfxOn = on;

		if ( SfxOn == false )
			audioSource.Pause();
		else
			audioSource.Play();


	}
}
