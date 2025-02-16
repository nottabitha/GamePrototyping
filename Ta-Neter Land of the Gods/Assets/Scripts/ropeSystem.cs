﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class ropeSystem : MonoBehaviour
{
	//https://www.raywenderlich.com/348-make-a-2d-grappling-hook-game-in-unity-part-1
	public GameObject ropeHingeAnchor;
	public DistanceJoint2D ropeJoint;
	public Transform crosshair;
	public SpriteRenderer crosshairSprite;
	public playerController playerMovement;
    public Rigidbody2D playerRb;
    public float speed;
    public bool hookAnimation = false;
    public Animator animator;
    public GameObject ropeWorldPosition;

    private bool ropeAttached;
	private Vector2 playerPosition;
	private Rigidbody2D ropeHingeAnchorRb;
	private SpriteRenderer ropeHingeAnchorSprite;
	
	public LineRenderer ropeRenderer;
	public LayerMask ropeLayerMask;
	private float ropeMaxCastDistance = 20f;
	private List<Vector2> ropePositions = new List<Vector2>();
	private bool distanceSet;
	private Camera cam;

    private GameObject bossCutsceneObj;
    private bossStartCutscene bossCutsceneScript;

    private Scene currentScene;

    void Awake()
	{
        currentScene = SceneManager.GetActiveScene();

        cam = Camera.main;
		ropeJoint.enabled = false;
		playerPosition = transform.position;
		ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
		ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();

        if (currentScene.name == "Level Boss")
        {
            bossCutsceneObj = GameObject.Find("CameraWaypoint");
            bossCutsceneScript = bossCutsceneObj.GetComponent<bossStartCutscene>();
        }
    }
	
	void Update()
	{
		var worldMousePosition =
			Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
		var facingDirection = worldMousePosition - transform.position;
		var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
		if (aimAngle < 0f)
		{
			aimAngle = Mathf.PI * 2 + aimAngle;
		}
		
		// 4
		var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
		// 5
		playerPosition = transform.position;
		
		// 6
		if (!ropeAttached)
		{
			SetCrosshairPosition();
		}
		else
		{
			crosshairSprite.enabled = false;
		}

        if (playerMovement.isSwinging)
        {
            float step = speed * Time.fixedDeltaTime;
            Vector2 ropePosition = Vector2.MoveTowards(playerRb.position, ropeHingeAnchorRb.position, step);
            playerRb.MovePosition(ropePosition);
        }


        HandleInput(aimDirection);
		UpdateRopePositions();
	}
	
	private void SetCrosshairPosition()
	{
        if (currentScene.name == "Level Boss")
        {
            if (bossCutsceneScript.cutsceneActive == false)
            {
                if (!crosshairSprite.enabled)
		        {
			        crosshairSprite.enabled = true;
		        }
		        Vector3 mouse = Input.mousePosition;
		        Vector3 mousePosition = cam.ScreenToWorldPoint(mouse);
		
		        //var x = transform.position.x + 5f * Mathf.Cos(aimAngle);
		        //var y = transform.position.y + 5f * Mathf.Sin(aimAngle);
		
		        var crossHairPosition = new Vector3(mousePosition.x, mousePosition.y, 0);
		        crosshair.transform.position = crossHairPosition;
            }
        }
        else
        {
            if (!crosshairSprite.enabled)
            {
                crosshairSprite.enabled = true;
            }
            Vector3 mouse = Input.mousePosition;
            Vector3 mousePosition = cam.ScreenToWorldPoint(mouse);

            //var x = transform.position.x + 5f * Mathf.Cos(aimAngle);
            //var y = transform.position.y + 5f * Mathf.Sin(aimAngle);

            var crossHairPosition = new Vector3(mousePosition.x, mousePosition.y, 0);
            crosshair.transform.position = crossHairPosition;
        }
	}
	
	// 1
	private void HandleInput(Vector2 aimDirection)
	{
        if (currentScene.name == "Level Boss")
        {
            if (bossCutsceneScript.cutsceneActive == true)
            {
                crosshairSprite.enabled = false;
            }


            if (bossCutsceneScript.cutsceneActive == false)
            {
                if (Input.GetMouseButton(0))
                {
                    hookAnimation = true;
                    animator.SetBool("Hook", hookAnimation);
                    // 2
                    if (ropeAttached) return;
                    ropeRenderer.enabled = true;

                    var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

                    // 3
                    if ((hit.collider.gameObject.CompareTag("Hook")) || (hit.collider.gameObject.CompareTag("Bat")))
                    {
                        ropeAttached = true;
                        if (!ropePositions.Contains(hit.point))
                        {
                            // 4
                            // Jump slightly to distance the player a little from the ground after grappling to something.
                            //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
                            //float step = speed * Time.fixedDeltaTime;
                            //Vector2 ropePosition = Vector2.MoveTowards(playerRb.position, ropeHingeAnchorRb.position, step);
                            //playerRb.MovePosition(ropePosition);
                            ropePositions.Add(hit.point);
                            ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                            ropeJoint.enabled = true;
                            ropeHingeAnchorSprite.enabled = true;
                            playerMovement.isSwinging = true;
                        }
                    }
                    // 5
                    else
                    {
                        hookAnimation = false;
                        animator.SetBool("Hook", hookAnimation);
                        ropeRenderer.enabled = false;
                        ropeAttached = false;
                        ropeJoint.enabled = false;
                    }
                }

                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {
                    ResetRope();
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                // 2
                hookAnimation = true;
                animator.SetBool("Hook", hookAnimation);
                if (ropeAttached) return;
                ropeRenderer.enabled = true;

                var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

                // 3
                if ((hit.collider.gameObject.CompareTag("Hook")))
                {
                    ropeAttached = true;
                    if (!ropePositions.Contains(hit.point))
                    {
                        // 4
                        // Jump slightly to distance the player a little from the ground after grappling to something.
                        //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
                        //float step = speed * Time.fixedDeltaTime;
                        //Vector2 ropePosition = Vector2.MoveTowards(playerRb.position, ropeHingeAnchorRb.position, step);
                        //playerRb.MovePosition(ropePosition);
                        ropePositions.Add(hit.point);
                        ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                        ropeJoint.enabled = true;
                        ropeHingeAnchorSprite.enabled = true;
                        playerMovement.isSwinging = true;
                    }
                }
                // 5
                else
                {
                    hookAnimation = false;
                    ropeRenderer.enabled = false;
                    ropeAttached = false;
                    ropeJoint.enabled = false;
                }
            }

            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                ResetRope();
            }
        }
	}
	
	// 6
	private void ResetRope()
	{
        hookAnimation = false;
        animator.SetBool("Hook", hookAnimation);
        ropeJoint.enabled = false;
		ropeAttached = false;
		playerMovement.isSwinging = false;
		ropeRenderer.positionCount = 2;
		ropeRenderer.SetPosition(0, ropeWorldPosition.transform.position);
		ropeRenderer.SetPosition(1, ropeWorldPosition.transform.position);
		ropePositions.Clear();
		ropeHingeAnchorSprite.enabled = false;
	}
	
	private void UpdateRopePositions()
	{
		// 1
		if (!ropeAttached)
		{
			return;
		}
		
		// 2
		ropeRenderer.positionCount = ropePositions.Count + 1;
		
		// 3
		for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
		{
			if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
			{
				ropeRenderer.SetPosition(i, ropePositions[i]);
				
				// 4
				if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
				{
					var ropePosition = ropePositions[ropePositions.Count - 1];
					if (ropePositions.Count == 1)
					{
						ropeHingeAnchorRb.transform.position = ropePosition;
						if (!distanceSet)
						{
							ropeJoint.distance = Vector2.Distance(ropeWorldPosition.transform.position, ropePosition);
							distanceSet = true;
						}
					}
					else
					{
						ropeHingeAnchorRb.transform.position = ropePosition;
						if (!distanceSet)
						{
							ropeJoint.distance = Vector2.Distance(ropeWorldPosition.transform.position, ropePosition);
							distanceSet = true;
						}
					}
				}
				// 5
				else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
				{
					var ropePosition = ropePositions.Last();
					ropeHingeAnchorRb.transform.position = ropePosition;
					if (!distanceSet)
					{
						ropeJoint.distance = Vector2.Distance(ropeWorldPosition.transform.position, ropePosition);
						distanceSet = true;
					}
				}
			}
			else
			{
				// 6
				ropeRenderer.SetPosition(i, ropeWorldPosition.transform.position);
			}
		}
	}
}