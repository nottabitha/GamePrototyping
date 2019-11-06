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
    public int health = 2;
    public GameObject player;
    public rageMeterScript rageMeterScript;
    public GameObject rageMeter;

    private playerController playerScript;
    private Transform target;
    private float aiCooldown;
	private bool isWalking;
    private Transform currentWaypoint;
    private float nextFire = 0.0f;


    void Awake()
    {
        playerScript = player.GetComponent<playerController>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        //boneThrowScript.enabled = false;

        //collider2D.enabled = false;
        currentWaypointIndex = 0;
        currentWaypoint = waypoints[currentWaypointIndex];
        isWalking = false;
        transform.position = currentWaypoint.position;
    }

    // Update is called once per frame
    void Update()
    {
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
        Death();
    }
    
	void Direction ()
	{
        if (Vector3.Distance(transform.position, currentWaypoint.position) < .5f)
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
			if (transform.localScale.x != -1f)
			{
				transform.localScale = new Vector3(-1f, 1f, 0f);
			}
			transform.position += Vector3.left * speed * Time.deltaTime;
			animator.SetBool("Walking", isWalking);
		}
        else if (currentWaypointIndex == 1)
        {
            isWalking = true;
            if (transform.localScale.x != 1f)
            {
                transform.localScale = new Vector3(1f, 1f, 0f);
            }
            animator.SetBool("Walking", isWalking);
            transform.position += Vector3.right * speed * Time.deltaTime;
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
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                animator.SetTrigger("Hit");
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
