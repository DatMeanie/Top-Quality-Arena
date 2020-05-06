using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    //plays random music
    //music chosen in unity editor

    public List<AudioClip> music = new List<AudioClip>();
    AudioSource audio;
    float length;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = music[Random.Range(0, music.Count)];
        length = audio.clip.length;
        audio.Play();
    }
    private void Update()
    {
        //length is a timer for when next song to play
        length -= Time.deltaTime;
        if(length <= 0)
        {
            audio.clip = music[Random.Range(0, music.Count)];
            length = audio.clip.length;
            audio.Play();
        }
    }
}
