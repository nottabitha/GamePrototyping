using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anubisScript : MonoBehaviour
{
	private Animator animator;
	
	public float minAttackCooldown = .5f;
	public float maxAttackCooldown = .2f;
    public float bossCooldown = 10f;
	public float speed = 5;
    public Transform[] waypoints;
    int currentWaypointIndex;
    public healthBar healthBar;
    public GameObject healthBarObject;
    public GameObject laser;
    public GameObject laserPoint;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public GameObject phase2startwaypoint;
    public bool phase1 = false;
    public bool phase2 = false;

    public Rigidbody2D laserRB;

    public AudioSource anubisRoar;

    private float health = 1f;
    private float aiCooldown;
	private bool isAttacking;
	private bool isWalking;
    private Transform currentWaypoint;

    private laser laserScript;
    private GameObject player;
    private float laserCooldown = .7f;
    private bool phase1Roar = false;
    private bool anubisAttackStart = true;
    private bool pointReached = false;
    private Vector3 playerPosition;
    private float maxVelocity = 10f;

    private bool phase1start = true;
    private bool phase2start = true;
    private bool roarDone = false;
    private bool isGrounded;

    public GameObject areaAttack;
    public Rigidbody2D anubisRB;

    public Transform laserHit;

    void Awake()
	{
        //animator = GetComponent<Animator>();
        //healthBarObject = GetComponent<GameObject>();
        healthBar = healthBarObject.GetComponent<healthBar>();
        //laser = GetComponent<GameObject>();
        laserRB = laser.GetComponent<Rigidbody2D>();
        laserPoint = GameObject.Find("LaserPoint");

        laserScript = laser.GetComponent<laser>();
        player = GameObject.Find("Player");

        healthBarObject.SetActive(false);

        anubisRoar = GetComponent<AudioSource>();

        areaAttack.SetActive(false);

        //areaAttack = GetComponent<GameObject>();
        //laserScript.enabled = false;
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

        //Attacking();

        //Movement
        //Direction ();

        if (phase1 == true)
        {
            Phase1();
        }

        if (phase2 == true)
        {
            Phase2();
        }
    }

    private void FixedUpdate()
    {
        anubisRB.velocity = Vector3.ClampMagnitude(anubisRB.velocity, maxVelocity);
    }

    private void TakeDamage()
    {
        if (health > .01)
        {
            health -= .01f;
            healthBar.SetSize(health);
        }
    }

    /*
	void Direction ()
	{
        if (Vector3.Distance(transform.parent.position, currentWaypoint.position) < .05f)
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
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Boss")
        {
            TakeDamage();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    /*
    void Attacking()
    {
        Vector3 direction = (player.transform.position - laserPoint.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(laserPoint.transform.position, direction, Mathf.Infinity);
        laserHit.position = hit.point;
    }
    */
    private void Phase2()
    {
        if (phase2start)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(phase2startwaypoint.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, phase2startwaypoint.transform.position) < 1f)
            {
                phase2start = false;
            }
        }

        if (!phase2start)
        {
            laserCooldown -= Time.deltaTime;
            bossCooldown -= Time.deltaTime;

            if (bossCooldown > 0f)
            {
                if (laserCooldown <= 0f)
                {
                    //laserScript.enabled = false;
                    Instantiate(laser, laserPoint.transform.position, Quaternion.identity);
                    laserCooldown = .7f;
                }
            }
            else if (bossCooldown <= 0f)
            {
                Phase2BossBreak(5f);
                bossCooldown = 10f;
            }
        }
    }

    private void Phase1()
    {
        if (phase1start)
        {
            anubisRoar.Play();
            playerPosition = player.transform.position;
            phase1start = false;
        }

        if (!anubisRoar.isPlaying)
        {
            roarDone = true;
        }

        if (roarDone)
        {
            healthBarObject.SetActive(true);

            if (transform.position.x != playerPosition.x && isGrounded)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPosition.x, transform.position.y), moveSpeed * Time.deltaTime);
            }
            else if (transform.position.x == playerPosition.x && isGrounded)
            {

                Attacking();

                playerPosition = player.transform.position;
            }

            if (health <= .5f)
            {
                phase1 = false;
                phase2 = true;
            }
        }
    }

    private void Attacking()
    {
        anubisAttackStart = true;

        if (anubisAttackStart)
        {
            anubisRB.AddForce(transform.up * 75f, ForceMode2D.Impulse);

            areaAttack.SetActive(true);

            anubisAttackStart = false;
        }
        areaAttack.SetActive(false);
    }

    IEnumerator Phase2BossBreak(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
