using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform laserHit;
    public Transform laserTarget;
    public float speed;

    private GameObject player;
    private Vector3 target;
    Vector3 playerTarget;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        lineRenderer = GetComponent<LineRenderer>();
        laserTarget = GetComponent<Transform>();
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LaserRender();
    }

    private void LaserRender()
    {
        playerTarget = player.transform.position;
        direction = (playerTarget - transform.position);

        target += direction * speed * Time.deltaTime;

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position, Mathf.Infinity);
        //laserHit.position = hit.point;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target);

        lineRenderer.enabled = true;
    }
}
