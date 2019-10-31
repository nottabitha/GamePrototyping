using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour {

	[SerializeField]
	GameObject dustCloud;

	bool coroutineAllowed, grounded;

    public Rigidbody2D playerRB;
    public GameObject player;

    private float lastPosition = 0;

    void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag.Equals ("TileBase"))
			grounded = true;
			coroutineAllowed = true;
			Instantiate (dustCloud, transform.position, dustCloud.transform.rotation);
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.gameObject.tag.Equals ("TileBase")) {
			grounded = false;
			coroutineAllowed = false;
		}

	}


    void Update()
	{

		if (grounded && player.transform.position.x != lastPosition && coroutineAllowed) {
				StartCoroutine ("SpawnCloud");
				coroutineAllowed = false;
		}

		if (player.transform.position.x == lastPosition || !grounded) {
				StopCoroutine ("SpawnCloud");
				coroutineAllowed = true;
		}

        lastPosition = player.transform.position.x;
    }


	IEnumerator SpawnCloud()
	{
		while (grounded && player.transform.position.x != lastPosition) {
			Instantiate (dustCloud, transform.position, dustCloud.transform.rotation);
			yield return new WaitForSeconds (0.25f);
		}
	}

}
