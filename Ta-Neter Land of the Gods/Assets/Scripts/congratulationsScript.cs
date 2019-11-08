using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class congratulationsScript : MonoBehaviour
{
    public GameObject congratulationsUI;
    private bool congratulations = false;
    private health anubisHealth;
    public GameObject anubis;


    // Start is called before the first frame update
    void Start()
    {
        anubisHealth = anubis.GetComponent<health>();
        congratulationsUI.SetActive(false);
    }

    // Update is called once per frame

    void Update()
    {
			if (anubisHealth.Health <= 0)
			{
				congratulations = true;
			}

			if (congratulations)
			{
				congratulationsUI.SetActive(true);
				Time.timeScale = 0;
			}
    }
}
