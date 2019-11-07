using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int speed = 10;
    public Rigidbody2D rb;

    public GameObject laserPoint;
    public GameObject laserParticle;

    private GameObject player;
    private health healthScript;
    private Vector3 target;
    private Vector3 laserHitPoint;
    private Vector3 direction;

    private RaycastHit2D hit;

    private bool raycastDone = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        laserPoint = GameObject.Find("LaserPoint");
        healthScript = player.GetComponent<health>();

        //lineRenderer = GetComponent<LineRenderer>();
        //laserTarget = GetComponent<Transform>();
        //lineRenderer.enabled = false;
        //lineRenderer.useWorldSpace = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (raycastDone == false)
        {
            Attacking();
        }

        LaserUpdate();

        if ( Vector3.Distance(transform.position, laserHitPoint) < .5f)
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
        transform.Translate(direction * Time.deltaTime * speed);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position, Mathf.Infinity);
        //laserHit.position = hit.point;

        //lineRenderer.SetPosition(0, transform.position);
        //lineRenderer.SetPosition(1, target);

        //lineRenderer.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Boss") 
        {
            if (collision.gameObject.tag == "Player")
            {
                healthScript.Health -= 1;
                Destroy(gameObject);
            }
            else
            {
                Instantiate(laserParticle, transform.position, Quaternion.LookRotation(new Vector3(0, 180, 0)));
                Destroy(gameObject);
            }
        }
    }

    void Attacking()
    {
        direction = (player.transform.position - laserPoint.transform.position).normalized;
        hit = Physics2D.Raycast(laserPoint.transform.position, direction, Mathf.Infinity);
        Debug.DrawRay(laserPoint.transform.position, player.transform.position - laserPoint.transform.position);
        laserHitPoint = hit.point;

        raycastDone = true;
    }
}
