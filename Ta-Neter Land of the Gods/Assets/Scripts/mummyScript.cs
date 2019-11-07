using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mummyScript : MonoBehaviour
{
    private Animator animator;

    public float minAttackCooldown = 0.5f;
    public float maxAttackCooldown = 2f;
    public float speed = 5;
    public Transform[] waypoints;
    public int health = 1;
    int currentWaypointIndex;
    public GameObject player;
    public rageMeterScript rageMeterScript;
    public GameObject rageMeter;

    private playerController playerScript;
    private float aiCooldown;
    private bool isAttacking;
    private bool isWalking;
    private Transform currentWaypoint;

    void Awake()
    {
        playerScript = player.GetComponent<playerController>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //collider2D.enabled = false;
        currentWaypointIndex = 0;
        currentWaypoint = waypoints[currentWaypointIndex];
        isWalking = false;
        isAttacking = false;
        aiCooldown = maxAttackCooldown;
        transform.position = currentWaypoint.position;
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
            Direction();
        }

        Death();
    }

    void Direction()
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
            if (transform.localScale.x != 1f)
            {
                transform.localScale = new Vector3(1f, 1f, 0);
            }
            transform.position += Vector3.left * speed * Time.deltaTime;
            animator.SetBool("Walking", isWalking);
        }
        else if (currentWaypointIndex == 1)
        {
            isWalking = true;
            if (transform.localScale.x != -1f)
            {
                transform.localScale = new Vector3(-1f, 1f, 0);
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
        if (collision.gameObject.tag == "Whip")
        {
            health -= 1;
            animator.SetTrigger("Hit");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Whip")
        {
            health -= 1;
            animator.SetTrigger("Hit");
        }
    }

    private void Death()
    {
        if (health <= 0)
        {
            rageMeterScript.sizeNormalized =+ .0769f;
            Destroy(gameObject);
        }
    }
}