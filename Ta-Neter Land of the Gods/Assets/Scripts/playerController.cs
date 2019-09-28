using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
	public float speed  = 5; 
	public bool isSwinging;
    public float maxVelocity;
    private Rigidbody2D player;

    // Update is called once per frame
    private void Awake()
    {
        player = this.GetComponent<Rigidbody2D>();
    }
    void Update()
	{
		if (Input.GetKey(KeyCode.D))
		{
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
	}

    private void FixedUpdate()
    {
        player.velocity = Vector3.ClampMagnitude(player.velocity, maxVelocity);
    }
}