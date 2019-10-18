using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anubisScript : MonoBehaviour
{
	private Animator animator;
	
	public float minAttackCooldown = 0.5f;
	public float maxAttackCooldown = 2f;
	public float speed = 5;
    public Transform[] waypoints;
    int currentWaypointIndex;
    public healthBar healthBar;
    public GameObject healthBarObject;

    public bool phase1 = false;
    public bool phase2 = false;

    public float minLaserCooldown = 2.0f;
    public float maxLaserCooldown = 3.0f;

    private float health = 1f;
    private float aiCooldown;
	private bool isAttacking;
	private bool isWalking;
    private Transform currentWaypoint;

    private GameObject laser;
    private laser laserScript;
    private GameObject player;
    private float laserCooldown;

    void Awake()
	{
        //animator = GetComponent<Animator>();
        //healthBarObject = GetComponent<GameObject>();
        healthBar = healthBarObject.GetComponent<healthBar>();
        laser = GameObject.Find("Laser");

        laserScript = laser.GetComponent<laser>();
        player = GameObject.Find("Player");

        laserScript.enabled = false;
    }		
    // Start is called before the first frame update
    void Start()
    {
        //collider2D.enabled = false;
        //currentWaypointIndex = 0;
        //currentWaypoint = waypoints[currentWaypointIndex];
        //isWalking = false;
        //transform.parent.position = currentWaypoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        	
    	aiCooldown -= Time.deltaTime;
    	//move or attack 
		//animator.SetBool("Walking", isWalking);

        Attacking();
        //Movement
		//Direction ();

        if (phase1 == true)
        {
            Debug.Log("Hello");
            Phase1();
        }
    }
    
    private void TakeDamage()
    {
        if (health > .01)
        {
            health -= .01f;
            healthBar.SetSize(health);
        }
    }

	void Direction ()
	{
        if (Vector3.Distance(transform.parent.position, currentWaypoint.position) < .5f)
        {
            Debug.Log((Vector3.Distance(transform.parent.position, currentWaypoint.position)));
            if (currentWaypointIndex + 1 < waypoints.Length)
            {
                currentWaypointIndex++;
            }
            else
            {
                currentWaypointIndex = 0;
            }

            currentWaypoint = waypoints[currentWaypointIndex];
        }

		if (currentWaypointIndex == 0)
		{
			isWalking = true;
			if (transform.parent.localScale.x != -0.91902f)
			{
				transform.parent.localScale = new Vector3(-0.91902f, 0.91902f, 0);
			}
			transform.parent.position += Vector3.left * speed * Time.deltaTime;
			animator.SetBool("Walking", isWalking);
		}
        else if (currentWaypointIndex == 1)
        {
            isWalking = true;
            if (transform.parent.localScale.x != 0.91902f)
            {
                transform.parent.localScale = new Vector3(0.91902f, 0.91902f, 0);
            }
            animator.SetBool("Walking", isWalking);
            transform.parent.position += Vector3.right * speed * Time.deltaTime;
        }
        else
		{
        	isWalking = false;
			animator.SetBool("Walking", isWalking);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TakeDamage();
        }
    }

    void Attacking()
    {
        //Physics2D.Raycast(laserPosition.transform.position, player.transform.position, Mathf.Infinity);
    }

    private void Phase1()
    {
        healthBarObject.SetActive(true);

        aiCooldown -= Time.deltaTime;
        /*
        if (laserCooldown <= 0f)
        {
            laserScript.enabled = false;
            laserCooldown = Random.Range(minLaserCooldown, maxLaserCooldown);
        }
        else
        */
        laserScript.enabled = true;
    }
}
