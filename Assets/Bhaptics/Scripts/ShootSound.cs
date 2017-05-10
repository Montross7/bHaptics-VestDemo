using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSound : MonoBehaviour
{
    private AudioSource AudioSource;
    [SerializeField] private AudioClip soundClip;
	void Awake ()
	{
	    AudioSource = GetComponent<AudioSource>();
	    AudioSource.clip = soundClip;
	}

    public void Shoot()
    {
        AudioSource.Play();
    }

    public void ShootEnd()
    {
        AudioSource.Stop();
    }

    public bool IsPlaying()
    {
        return AudioSource.isPlaying;
    }
}
