using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameOverScript : MonoBehaviour
{
    public GameObject gameOverUI;
    private bool gameover = false;
    private health healthScript;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        healthScript = player.GetComponent<health>();
        gameOverUI.SetActive(false);
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
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }

    }
}
