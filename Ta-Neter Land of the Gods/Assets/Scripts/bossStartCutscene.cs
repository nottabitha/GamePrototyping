using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossStartCutscene : MonoBehaviour
{
    public GameObject Cameratarget1;
    public GameObject Cameratarget2;
    public GameObject Playertarget;
    public GameObject player;
    //public GameObject healthBar;
    public GameObject anubis;
    public GameObject blockingTiles;


    public bool cutsceneActive;
    private Vector3 position;
    private Animator animator;
    private anubisScript anubisScript;

    // Start is called before the first frame update
    void Start()
    {
        blockingTiles.SetActive(false);
        cutsceneActive = true;
        position = transform.position;

        animator = player.GetComponent<Animator>();
        anubisScript = anubis.GetComponent<anubisScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (cutsceneActive == true)
        {
            if ((Vector3.Distance(player.transform.position, Playertarget.transform.position) <= .2f) && (Vector3.Distance(position, Cameratarget2.transform.position) < .2f))
            {
                animator.SetBool("Walking", false);
                blockingTiles.SetActive(true);
                cutsceneActive = false;
                //healthBar.SetActive(true);
                anubisScript.phase1 = true;
            }
            else
            {
                if (Vector3.Distance(player.transform.position, Playertarget.transform.position) > .2f)
                {
                    animator.SetBool("Walking", true);
                    player.transform.position += Vector3.right * 3 * Time.deltaTime;
                }
                if (Vector3.Distance(player.transform.position, Cameratarget2.transform.position) > .2f)
                {
                    position += Vector3.right * 4 * Time.deltaTime;
                }
            }
        }
        transform.position = position;
    }
}
