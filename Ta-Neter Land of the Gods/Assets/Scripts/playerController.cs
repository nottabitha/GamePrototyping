using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
	public float speed  = 5; 
	public bool isSwinging;
    public float maxVelocity;
    private Rigidbody2D player;
    public health healthScript;
    public bool isAttacking;
    public Animator animator;
    public AudioSource footsteps;
	//public static Rigidbody2D rb;


    private bool isWalking;
    private bool invulnerable = false;
    private GameObject bossCutsceneObj;
    private bossStartCutscene bossCutsceneScript;

    private Scene currentScene;


    private void Start()
    {
        isWalking = false;
        isAttacking = false;
    }

    // Update is called once per frame
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();

        player = this.GetComponent<Rigidbody2D>();
        healthScript = this.GetComponent<health>();
        if (currentScene.name == "Level Boss")
        {
            bossCutsceneObj = GameObject.Find("CameraWaypoint");
            bossCutsceneScript = bossCutsceneObj.GetComponent<bossStartCutscene>();
        }

        animator = GetComponent<Animator>();

        footsteps = GetComponent<AudioSource>();
    }
    void Update()
	{
        if (currentScene.name == "Level Boss")
        {
            if (bossCutsceneScript.cutsceneActive == false)
            {
                Movement();
                Attack();
            }
        }
        else
        {
            Movement();
            Attack();
        }
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            isWalking = true;
            if (transform.localScale.x != 0.39101f)
            {
                transform.localScale = new Vector3(0.39101f, 0.39101f, 0.39101f);
            }
            animator.SetBool("Walking", isWalking);
            transform.position += Vector3.right * speed * Time.deltaTime;
			footsteps.Play();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            isWalking = true;
            if (transform.localScale.x != -0.39101f)
            {
                transform.localScale = new Vector3(-0.39101f, 0.39101f, 0.39101f);
            }
            animator.SetBool("Walking", isWalking);
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            isWalking = false;
            animator.SetBool("Walking", isWalking);
        }
    }

    private void FixedUpdate()
    {
        player.velocity = Vector3.ClampMagnitude(player.velocity, maxVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (invulnerable == false)
            if ((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "Bat"))
            {
                healthScript.Health -= 1;
                StartCoroutine(Invulnerability());
            }

        if (collision.gameObject.tag == "Spike")
        {
            healthScript.Health = 0;
        }
    }

    IEnumerator Invulnerability()
    {
        invulnerable = true;
        yield return new WaitForSeconds(3);
        invulnerable = false;
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = true;
            animator.SetBool("Attack", isAttacking);
        }
        else
        {
            isAttacking = false;
            animator.SetBool("Attack", isAttacking);
        }
    }
}