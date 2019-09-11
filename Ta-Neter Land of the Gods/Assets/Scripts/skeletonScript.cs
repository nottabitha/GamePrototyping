using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonScript : MonoBehaviour
{
	private Animator animator;
	
	public float minAttackCooldown = 0.5f;
	public float maxAttackCooldown = 2f;
	public float speed = 5;
    public Rigidbody2D waypoint1;
    public Rigidbody2D waypoint2;

    private float aiCooldown;
	private bool isAttacking;
	private bool isWalking;
    private bool isWalkingLeft = false;
    private bool isWalkingRight = true;

    void Awake()
	{
		animator = GetComponent<Animator>();
        waypoint1 = GetComponent<Rigidbody2D>();
        waypoint2 = GetComponent<Rigidbody2D>();
	}		
    // Start is called before the first frame update
    void Start()
    {
        //collider2D.enabled = false;
        
        isWalking = false;
		isAttacking = false;
		aiCooldown = maxAttackCooldown;
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
			
        	isAttacking = !isAttacking;
        	aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
        	
        	animator.SetBool("Attack", isAttacking);
        	
        }
        
        //Movement
        if (!isAttacking)
        {
			Direction ();
        }
    }
    
	void Direction ()
	{
		if (isWalkingRight)
		{
			isWalking = true;
			if (transform.localScale.x != 4.261748)
			{
				transform.localScale = new Vector3(4.261748f, 4.181867f, 0);
			}
			animator.SetBool("Walking", isWalking);
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		else if (isWalkingLeft)
		{
			isWalking = true;
			if (transform.localScale.x != -4.261748)
			{
				transform.localScale = new Vector3(-4.261748f, 4.181867f, 0);
			}
			transform.position += Vector3.left * speed * Time.deltaTime;
			animator.SetBool("Walking", isWalking);
		}
		else
		{
        	isWalking = false;
			animator.SetBool("Walking", isWalking);
		}
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider2D)
	{
        if (waypoint1)
        {
            isWalkingLeft = true;
            isWalkingRight = false;
        }
        if (waypoint2)
        {
            isWalkingRight = true;
            isWalkingLeft = false;
        }
        animator.SetTrigger("Hit");
	}
}
