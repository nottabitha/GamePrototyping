﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
	public float speed  = 5; 
	public bool isSwinging;
    public float maxVelocity;
    public float attackTagCooldown;
    private Rigidbody2D player;
    public health healthScript;
    public bool isAttacking;
    public Animator animator;
	public AudioSource playerAudioSource;
	public AudioClip playerHurtSound;
    public GameObject playerPivot;
    public GameObject whipObject;
    public Animation whipColliderAnimation;
	public AudioClip playerDeath;
    public GameObject whip;
    public GameObject anubis;
    public GameObject whipCheck;
    public GameObject canvas;

    private anubisScript bossScript;
    //public AudioSource footsteps;
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
        canvas = GameObject.Find("Canvas");
        whipColliderAnimation = whipObject.GetComponent<Animation>();
        currentScene = SceneManager.GetActiveScene();

        player = this.GetComponent<Rigidbody2D>();
        healthScript = canvas.GetComponent<health>();
        if (currentScene.name == "Level Boss")
        {
            bossCutsceneObj = GameObject.Find("CameraWaypoint");
            bossCutsceneScript = bossCutsceneObj.GetComponent<bossStartCutscene>();
        }

        bossScript = anubis.GetComponent<anubisScript>();


        //footsteps = GetComponent<AudioSource>();
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
			//footsteps.Play();
            isWalking = true;
            if (playerPivot.transform.localScale.x != 0.39101f)
            {
                playerPivot.transform.localScale = new Vector3(0.39101f, 0.39101f, 0f);
            }
            animator.SetBool("Walking", isWalking);
            playerPivot.transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
           // footsteps.Play();
			isWalking = true;
            if (playerPivot.transform.localScale.x != -0.39101f)
            {
                playerPivot.transform.localScale = new Vector3(-0.39101f, 0.39101f, 0f);
            }
            animator.SetBool("Walking", isWalking);
            playerPivot.transform.position += Vector3.left * speed * Time.deltaTime;
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
				playerAudioSource.PlayOneShot(playerHurtSound);
                StartCoroutine(Invulnerability());
				StartCoroutine(this.knockback(0.03f, 1000, player.transform.position));
            }
        if (invulnerable == false)
        {
            if (collision.gameObject.tag == "Boss")
            {
                if (bossScript.isAttacking == true)
                {
                    healthScript.Health -= 1;
                    playerAudioSource.PlayOneShot(playerHurtSound);
                    StartCoroutine(Invulnerability());
                    StartCoroutine(this.knockback(0.03f, 1000, player.transform.position));
                }
            }
        }

        if (collision.gameObject.tag == "Spike")
        {
            healthScript.Health -= 3;
			playerAudioSource.PlayOneShot(playerHurtSound);
			StartCoroutine(Invulnerability());
			StartCoroutine(this.knockback(0.02f, 1000, player.transform.position));
        }

		if (healthScript.Health <= 0) 
		{
			playerAudioSource.PlayOneShot(playerDeath);
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
        attackTagCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attackTagCooldown = 1f;
            whip.tag = "Whip";
            whipCheck.tag = "Whip";
            whipObject.SetActive(true);
            isAttacking = true;
            animator.SetBool("Attack", isAttacking);
            whipColliderAnimation.Play();
        }
        else
        {
            isAttacking = false;
            animator.SetBool("Attack", isAttacking);
            //whipColliderWait();
            //whipColliderAnimation.Stop();
            //whipObject.SetActive(false);
        }
        if (attackTagCooldown <= 0)
        {
            whipCheck.tag = "Player";
            whip.tag = "Player";
        }
    }

    private IEnumerator whipColliderWait()
    {
        yield return new WaitForSeconds(2f);
    }

	public IEnumerator knockback(float knockDuration, float knockPower, Vector3 knockDirection) 
	{
		float timer = 0;

		while(knockDuration > timer) {
			timer += Time.deltaTime;
			player.velocity = new Vector2 (0, 0);
			player.AddForce(new Vector3(-knockDirection.x +10, -knockDirection.y + knockPower, transform.position.z));
		}

		yield return 0;
	}
}