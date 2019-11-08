using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class gameOverScript : MonoBehaviour
{
    public GameObject gameOverUI;
    private bool gameover = false;
    private health healthScript;
    public GameObject canvas;
	public GameObject musicPlayer;
	private AudioSource levelMusic;
	private float originalVolume;


    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        healthScript = canvas.GetComponent<health>();
        gameOverUI.SetActive(false);
		levelMusic = musicPlayer.GetComponent<AudioSource>();
		originalVolume = levelMusic.volume;
    }

    // Update is called once per frame

    void Update()
    {
			if (healthScript.Health <= 0)
			{
				gameover = true;
			}

			if (gameover)
			{
				levelMusic.volume = 0;
				gameOverUI.SetActive(true);
				Time.timeScale = 0;
			}
			else {
				levelMusic.volume = originalVolume;
			}
    }
}
