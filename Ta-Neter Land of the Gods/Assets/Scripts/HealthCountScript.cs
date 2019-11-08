using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthCountScript : MonoBehaviour
{
    public health healthScript;
    public int healthNumber;
    public GameObject player;
    private Scene scene;
    private bool level2 = false;
    private bool level3 = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerPivot");
        healthScript = player.GetComponent<health>();

    }

    // Update is called once per frame
    void Update()
    {
        if (scene.name == "Level 1")
        {
            healthNumber = healthScript.Health;
        }

        if (scene.name == "Level 2_edit")
        {
            if (!level2)
            {
                player = GameObject.Find("PlayerPivot");
                healthScript = player.GetComponent<health>();
                healthScript.Health = healthNumber;
                level2 = true;
            }
            else
            {
                healthNumber = healthScript.Health;
            }
        }

        if (scene.name == "Level Boss")
        {
            if (!level3)
            {
                player = GameObject.Find("PlayerPivot");
                healthScript = player.GetComponent<health>();
                healthScript.Health = healthNumber;
                level2 = true;
            }
            else
            {
                healthNumber = healthScript.Health;
            }
        }
    }
}
