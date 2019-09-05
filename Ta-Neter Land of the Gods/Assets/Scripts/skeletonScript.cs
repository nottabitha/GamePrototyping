using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonScript : MonoBehaviour
{
	private Animator animator;
	
	public float minAttackCooldown = 0.5f;
	public float maxAttackCooldown = 2f;
	public float speed = 5;
	
	private float aiCooldown;
	private bool isAttacking;
	private bool isWalking;
	
	void Awake()
	{
		animator = GetComponent<Animator>();
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
		if (Input.GetKey(KeyCode.D))
		{
			isWalking = true;
			if (transform.localScale.x != 4.261748)
			{
				transform.localScale = new Vector3(4.261748f, 4.181867f, 0);
			}
			animator.SetBool("Walking", isWalking);
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.A))
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
		animator.SetTrigger("Hit");
	}
}
