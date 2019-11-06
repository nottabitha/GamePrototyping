using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boneThrow : MonoBehaviour
{
    public Transform target;

    public float speed = 10;

    public float arcHeight = 1;

    Vector3 startPos;

    private Vector3 targetStart;
    private float x0;
    private float x1;
    private float y1;
    // http://luminaryapps.com/blog/arcing-projectiles-in-unity/
    void Start()
    {
        // Cache our start position, which is really the only thing we need
        // (in addition to our current position, and the target).
        startPos = transform.position;
        target = GameObject.Find("boneTarget").transform;

        x0 = startPos.x;
        x1 = target.position.x;
        y1 = target.position.y;

        targetStart = target.position;

    }

    void Update()
    {
        SimulateProjectile();
    }

    void SimulateProjectile()
    {
        // Compute the next position, with arc added in

        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
        float baseY = Mathf.Lerp(startPos.y, y1, (nextX - x0) / dist) ;
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        // Rotate to face the next position, and then move there
        transform.rotation = LookAt2D(nextPos - transform.position);
        transform.position = nextPos;

        // Do something when we reach the target
        if (Vector3.Distance(targetStart, nextPos) <= 5.17)
        {
            Arrived();
        }

    }

    void Arrived()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    /// 
    /// This is a 2D version of Quaternion.LookAt; it returns a quaternion
    /// that makes the local +X axis point in the given forward direction.
    /// 
    /// forward direction
    /// Quaternion that rotates +X to align with forward
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

}

