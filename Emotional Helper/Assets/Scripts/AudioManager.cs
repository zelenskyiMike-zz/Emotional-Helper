using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public AudioClip[] musicClips;

    public AudioSource source;
    private int currentTrack;

    public Text clipTitleText;
    public Text clipTimeText;

    private int playTime;
    private int seconds;
    private int minutes;

    // Use this for initialization
    void Start () {

        source = GetComponent<AudioSource>();

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
            playTime = (int)source.time;
            showPlayTime();
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
        ShowCurrTitle();

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
        ShowCurrTitle();

        StartCoroutine(WaitForMusicEnd());
    }

    public void StopMusic()
    {
        StopAllCoroutines();
       // StopCoroutine("WaitForMusicEnd");
        source.Stop();
    }
    
    void ShowCurrTitle()
    {
        clipTitleText.text = source.clip.name;
    }
    void showPlayTime()
    {
        seconds = playTime % 60;
        minutes = (playTime / 60) % 60;
        clipTimeText.text = minutes + ":" + seconds.ToString("D2");
    }
}
