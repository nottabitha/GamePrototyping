using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
	public AudioSource bgMusic;
	public AudioSource newTrack;
	public float time = 9f;

    // Start is called before the first frame update
    void Start()
    {
        bgMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0) 
		{
			time -= Time.deltaTime;
		}
		else 
		{
			if (bgMusic.isPlaying)
			{
				bgMusic.Stop();
				newTrack.Play();
			}
		}
    }

}
