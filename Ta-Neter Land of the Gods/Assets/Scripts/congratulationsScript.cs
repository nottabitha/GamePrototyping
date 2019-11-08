using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class congratulationsScript : MonoBehaviour
{
    public GameObject congratulationsUI;
    private bool congratulations = false;
    private anubisScript anubisHealth;
    public GameObject anubis;


    // Start is called before the first frame update
    void Start()
    {
        anubisHealth = anubis.GetComponent<anubisScript>();
        congratulationsUI.SetActive(false);
    }

    // Update is called once per frame

    void Update()
    {
			if (anubisHealth.health <= 0)
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
