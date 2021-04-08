using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string audioName;

	public AudioClip clip;

	[Range(0, 1f)]
	public float volume;

	[Range(0f, 2f)]
	public float pitch = 1f;

	[Range(0f, 1f)]
	public float spatialBlend = 1f;

	public bool loop;

	[HideInInspector]
	public AudioSource source;

}
