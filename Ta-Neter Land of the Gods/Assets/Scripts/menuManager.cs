using System.Collections;
using UnityEngine;

public class menuManager : MonoBehaviour
{
	public void StartGame() 
	{
		Application.LoadLevel(1);
	}

	public void Quit() 
	{
		Application.Quit();
	}
}
