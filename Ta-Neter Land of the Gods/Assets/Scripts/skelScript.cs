using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skelScript : MonoBehaviour
{
	private Animator animator;
	
	public float speed = 5;
    public Transform[] waypoints;
    int currentWaypointIndex;
    public GameObject bone;
    public float fireRate = 1f;

    private boneThrow boneThrowScript;
    private Transform target;
    private float aiCooldown;
	private bool isWalking;
    private Transform currentWaypoint;
    private float nextFire = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        bone = GameObject.FindGameObjectWithTag("bone");
        //boneThrowScript = bone.GetComponent<boneThrow>();
        //boneThrowScript.enabled = false;

        //collider2D.enabled = false;
        currentWaypointIndex = 0;
        currentWaypoint = waypoints[currentWaypointIndex];
        isWalking = false;
        transform.parent.position = currentWaypoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, target.position));

        if ((Vector3.Distance(target.position, transform.position) < 6f) && (Time.time > nextFire))
        {
            nextFire = Time.time + fireRate;
            Instantiate(bone, transform.position, Quaternion.identity);
            //boneThrowScript.enabled = true;
        }
        	
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
    
}
