using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
	public float speed  = 5; 
	public bool isSwinging;
    public float maxVelocity;
    private Rigidbody2D player;
    public health healthScript;

    private Animator animator;
    private bool isWalking;
    private bool invulnerable = false;
    private GameObject bossCutsceneObj;
    private bossStartCutscene bossCutsceneScript;

    private void Start()
    {
        isWalking = false;
    }

    // Update is called once per frame
    private void Awake()
    {
        player = this.GetComponent<Rigidbody2D>();
        healthScript = this.GetComponent<health>();
        bossCutsceneObj = GameObject.Find("CameraWaypoint");
        bossCutsceneScript = bossCutsceneObj.GetComponent<bossStartCutscene>();

        animator = GetComponent<Animator>();
    }
    void Update()
	{
        if (bossCutsceneScript.cutsceneActive == false)
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
}