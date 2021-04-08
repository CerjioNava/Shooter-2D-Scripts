using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

	public int index;

	public Sound[] sounds;

	public static AudioManager instance;

	private float volume;

	private Sound stage;

	void Awake () {
	
		if (instance == null)
			instance = this;
		else {
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		foreach (Sound s in sounds) {

			s.source = gameObject.AddComponent <AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.spatialBlend = s.spatialBlend;
			s.source.loop = s.loop;
		
		}

	}

	void Start () {

		StageTheme ();
		
	}

	public void PlayAudio (string audioName) {

		Sound s = Array.Find (sounds, sound => sound.audioName == audioName);

		if (s == null)
			return;

		s.source.Play ();

	}

	public void PauseAudio () {
		
		if (SceneManager.GetActiveScene ().buildIndex == 0)
			stage = GetAudio ("Stage Theme");
		else if (SceneManager.GetActiveScene ().buildIndex == 1)
			stage = GetAudio ("Stage 1 Theme");
		else if (SceneManager.GetActiveScene ().buildIndex == 2)
			stage = GetAudio ("Stage 2 Theme");
		else if (SceneManager.GetActiveScene ().buildIndex == 3)
			stage = GetAudio ("Stage 3 Theme");

		if (!stage.source.mute) {
			PlayAudio ("Menu Out");
			stage.source.mute = true;
		} else {
			PlayAudio ("Menu In");
			stage.source.mute = false;
		}

	}

	public Sound GetAudio (string audioName) {

		Sound s = Array.Find (sounds, sound => sound.audioName == audioName);
		return s;

	}

	public void StopAudio (string audioName) {

		Sound s = GetAudio (audioName);
		s.source.Stop ();

	}

	public void StageTheme () {

		if (index != SceneManager.GetActiveScene ().buildIndex) {

			if (SceneManager.GetActiveScene ().buildIndex == 0)
				StopAudio ("Stage Theme");
			else if (SceneManager.GetActiveScene ().buildIndex == 1)
				StopAudio ("Stage 1 Theme");
			else if (SceneManager.GetActiveScene ().buildIndex == 2)
				StopAudio ("Stage 2 Theme");
			else if (SceneManager.GetActiveScene ().buildIndex == 3)
				StopAudio ("Stage 3 Theme");
			
		}

		if (index == 0)
			PlayAudio ("Stage Theme");
		else if (index == 1)
			PlayAudio ("Stage 1 Theme");
		else if (index == 2)
			PlayAudio ("Stage 2 Theme");
		else if (index == 3)
			PlayAudio ("Stage 3 Theme");
		
	}

	public void SetIndex (int n) {
		index = n;
	}

}
