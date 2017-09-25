using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour 
{
	[SerializeField] private Text numberOfLives;

	public void updateNumberOfLives(int newNum)
	{
		numberOfLives.text = "Lives: " + newNum.ToString();
	}
}
