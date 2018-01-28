using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public AudioClip[] musicClips;

    public AudioSource source;
    private int currentTrack;

    // Use this for initialization
    void Start () {

        source = GetComponent<AudioSource>();

        //Play
        //PlayMusic();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayMusic()
    {
        if (source.isPlaying)
        {
            return;
        }
        currentTrack--;

        if (currentTrack < 0)
        {
            currentTrack = musicClips.Length - 1;
        }
        StartCoroutine(WaitForMusicEnd());
    }

    IEnumerator WaitForMusicEnd()
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        NextTitle(); 
    }

    public void NextTitle()
    {
        source.Stop();
        currentTrack++;

        if (currentTrack > musicClips.Length - 1)
        {
            currentTrack = 0;
        }
        source.clip = musicClips[currentTrack];
        source.Play();

        //show Title
        StartCoroutine(WaitForMusicEnd());
    }
    public void PrevTitle()
    {
        source.Stop();
        currentTrack--;

        if (currentTrack < 0)
        {
            currentTrack = musicClips.Length - 1;
        }
        source.clip = musicClips[currentTrack];
        source.Play();

        //show Title
        StartCoroutine(WaitForMusicEnd());
    }

    public void StopMusic()
    {
        StopAllCoroutines();
       // StopCoroutine("WaitForMusicEnd");
        source.Stop();
    }
}
