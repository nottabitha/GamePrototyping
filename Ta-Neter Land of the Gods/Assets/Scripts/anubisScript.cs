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


    private float aiCooldown;
	private bool isAttacking;
	private bool isWalking;
    private Transform currentWaypoint;

    void Awake()
	{
		//animator = GetComponent<Animator>();
	}		
    // Start is called before the first frame update
    void Start()
    {
        //collider2D.enabled = false;
        currentWaypointIndex = 0;
        currentWaypoint = waypoints[currentWaypointIndex];
        //isWalking = false;
        transform.parent.position = currentWaypoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        	
    	aiCooldown -= Time.deltaTime;
    	//move or attack 
		animator.SetBool("Walking", isWalking);

        Attacking();
        //Movement
		//Direction ();
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
	
	void OnTriggerEnter2D(Collider2D otherCollider2D)
	{
        animator.SetTrigger("Hit");
	}

    void Attacking()
    {
        GameObject laserPosition = GameObject.Find("LaserPoint");
        GameObject player = GameObject.Find("Player");
        var laser = Physics2D.Raycast(laserPosition.transform.position, player.transform.position, Mathf.Infinity);
    }
}
