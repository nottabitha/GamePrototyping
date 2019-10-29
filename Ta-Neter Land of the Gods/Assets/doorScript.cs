using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    public Animator doorAnimator;
    public bool animDone = false;
    // Start is called before the first frame update
    void Start()
    {
        doorAnimator.SetBool("isOpening", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!animDone)
            {
                Debug.Log("hello");
                doorAnimator.SetBool("isOpening", true);
                animDone = true;
            }
        }
    }
}
