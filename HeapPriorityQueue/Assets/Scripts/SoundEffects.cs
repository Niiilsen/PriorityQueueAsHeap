using UnityEngine;
using System.Collections;

public class SoundEffects : MonoBehaviour {
	public AudioClip allowed;
	public AudioClip notAllowed;
	public AudioClip newNode;


	public void Allowed(){
		GetComponent<AudioSource> ().PlayOneShot (allowed);
	}

	public void NotAllowed()
	{
		GetComponent<AudioSource> ().PlayOneShot (notAllowed);
	}

	public void New()
	{
		GetComponent<AudioSource> ().PlayOneShot (newNode);
	}

	public void Death()
	{
		GetComponent<AudioSource> ().PlayOneShot (notAllowed);
	}
}
