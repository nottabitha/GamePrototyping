using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour {

	[SerializeField]
	GameObject dustCloud;

	bool coroutineAllowed, grounded;

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
		if (grounded && playerController.rb.velocity.x != 0 && coroutineAllowed) {
				StartCoroutine ("SpawnCloud");
				coroutineAllowed = false;
		}

		if (playerController.rb.velocity.x == 0 || !grounded) {
				StopCoroutine ("SpawnCloud");
				coroutineAllowed = true;
		}
	}


	IEnumerator SpawnCloud()
	{
		while (grounded) {
			Instantiate (dustCloud, transform.position, dustCloud.transform.rotation);
			yield return new WaitForSeconds (0.25f);
		}
	}

}
