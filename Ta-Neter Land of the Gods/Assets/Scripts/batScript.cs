using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batScript : MonoBehaviour
{
	private Animator animator;
	
	public float speed = 5;
    public Transform[] waypoints;
    int currentWaypointIndex;
    public rageMeterScript rageMeterScript;
    public GameObject rageMeter;
    public int health = 1;


    private float aiCooldown;
	private bool isWalking;
    private Transform currentWaypoint;

    void Awake()
	{
		animator = GetComponent<Animator>();
	}		
    // Start is called before the first frame update
    void Start()
    {
        //collider2D.enabled = false;
        currentWaypointIndex = 0;
        currentWaypoint = waypoints[currentWaypointIndex];
        isWalking = false;
        transform.parent.position = currentWaypoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        	
    	aiCooldown -= Time.deltaTime;
    	//move or attack 
        if (aiCooldown <= 0f)
        {
			isWalking = false;
			animator.SetBool("Walking", isWalking);
        }
        
        //Movement
		Direction ();
    }
    
	void Direction ()
	{
        if (Vector3.Distance(transform.parent.position, currentWaypoint.position) < .5f)
        {
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
			if (transform.parent.localScale.x != 17.01f)
			{
				transform.parent.localScale = new Vector3(1f, 1f, 0);
			}
			transform.parent.position += Vector3.left * speed * Time.deltaTime;
			animator.SetBool("Walking", isWalking);
		}
        else if (currentWaypointIndex == 1)
        {
            isWalking = true;
            if (transform.parent.localScale.x != 17.01f)
            {
                transform.parent.localScale = new Vector3(-1f, 1f, 0);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                health -= 1;
            }
        }
    }

    private void Death()
    {
        if (health <= 0)
        {
            rageMeterScript.sizeNormalized = +.0769f;
            Destroy(gameObject);
        }
    }
}
