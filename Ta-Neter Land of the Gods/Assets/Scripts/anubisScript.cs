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
    public GameObject weapon;


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
    private Transform weaponPivot;
    private Transform attackPoint;
    private Transform weaponAttackPoint;
    private bool anubisAttackStart = true;

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

        weaponPivot = GameObject.Find("WeaponPivot").transform;
        attackPoint = GameObject.Find("AttackPoint").transform;
        weaponAttackPoint = GameObject.Find("WeaponAttackPoint").transform;

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
            //Phase2();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Boss")
        {
            TakeDamage();
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

    private void Phase1()
    {
        anubisRoar.Play();
        if (!anubisRoar.isPlaying)
        {
            phase1Roar = true;
        }
        if (phase1Roar)
        {
            healthBarObject.SetActive(true);

            Attacking();
        }
    }

    private void Attacking()
    {
        if (anubisAttackStart)
        {
            if (weaponPivot.transform.rotation != Quaternion.Euler(0,0,0))
            {
                float speed = 5f;
                weaponPivot.transform.rotation = Quaternion.Slerp(weaponPivot.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed);
            }
            else
            {
                anubisAttackStart = false;
            }
        }


        if (!anubisAttackStart)
        {
            if (weaponAttackPoint.transform.position != attackPoint.transform.position)
            {
                float speed = 8f;
                weaponPivot.transform.rotation = Quaternion.Slerp(weaponPivot.transform.rotation, Quaternion.Euler(0, 0, 116.5f), Time.deltaTime * speed);
            }
        }
        


    }

    IEnumerator Phase2BossBreak(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
