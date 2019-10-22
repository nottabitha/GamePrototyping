using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform laserHit;
    public Transform laserTarget;
    public int speed = 10;
    public Vector3 playerTarget;
    public Rigidbody2D rb;

    public Transform laserPoint;

    private GameObject player;
    private health healthScript;
    private Vector3 target;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        laserPoint = GetComponent<Transform>();
        playerTarget = player.transform.position;
        healthScript = player.GetComponent<health>();


        //lineRenderer = GetComponent<LineRenderer>();
        //laserTarget = GetComponent<Transform>();
        //lineRenderer.enabled = false;
        //lineRenderer.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        LaserUpdate();

        if ( Vector3.Distance(transform.position, playerTarget) < .5f)
        {
            Arrived();
        }
    }

    void Arrived()
    {
        Destroy(gameObject);
    }

    private void LaserUpdate()
    {
        direction = (playerTarget - laserPoint.position).normalized;
        transform.Translate(direction * Time.deltaTime * speed);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position, Mathf.Infinity);
        //laserHit.position = hit.point;

        //lineRenderer.SetPosition(0, transform.position);
        //lineRenderer.SetPosition(1, target);

        //lineRenderer.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            healthScript.Health -= 1;
            Destroy(gameObject);
        }
    }
}
