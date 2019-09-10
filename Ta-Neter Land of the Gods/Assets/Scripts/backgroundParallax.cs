using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundParallax : MonoBehaviour
{
	public bool scrolling, parallax;
	
	public float backgroundSize;
	public float parallaxSpeed;
	
	
	private Transform cameraTransform;
	private Transform[] layers;
	private float viewZone = 10;
	private int leftIndex;
	private int rightIndex;
	private float lastCameraX;
	
	private void Start()
	{
		//camera values equal to main camera
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		//layers array size is equal to number of children 
		layers = new Transform[transform.childCount];
		
		for (int i = 0; i < transform.childCount; i++)
		{
			//assigns the transform of child at index - for example, layer 1 transform will be equal to background 1 transform
			layers[i] = transform.GetChild(i);
			
			//for clarity, array counts 0 so its fine
			leftIndex = 0;
			//last object in array, minus one because index counts the 0
			rightIndex = layers.Length - 1;
		}
	}
	
	private void Update()
	{	
		
		if (parallax)
		{
			float deltaX = cameraTransform.position.x - lastCameraX;
			transform.position += Vector3.right * (deltaX * parallaxSpeed);
		}
		
		lastCameraX = cameraTransform.position.x;
		
		if (scrolling)
		{
			if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
			{
				ScrollLeft();
			}
			if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
			{
				ScrollRight();
			}
		}
	}
	
	private void ScrollLeft()
	{
		//retrieve background image from far right and move it to the left
		int lastRight = rightIndex;
		//retrieve left index position and take away backgroundsize to determine placement of new background
		layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0)
		{
			//starts rotation over
			rightIndex = layers.Length - 1;
		}
	}
	
	private void ScrollRight()
	{
		int lastLeft = leftIndex;
		layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length)
		{
			leftIndex = 0;
		}
	}
}
