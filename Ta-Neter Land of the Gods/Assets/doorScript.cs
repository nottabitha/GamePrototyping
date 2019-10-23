using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    public Animator doorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
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
            doorAnimator.SetBool("isOpening", true);
        }
    }
}
