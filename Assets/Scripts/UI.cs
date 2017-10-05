using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour 
{
	[SerializeField] private Text numberOfLives;
	[SerializeField] private Text numberOfWaves;

	public GameObject youWinScreen;
	public GameObject youLoseScreen;
	public GameObject generalUI;

	public void updateNumberOfLives(int newNum)
	{
		numberOfLives.text = "Lives: " + newNum.ToString();
	}
	public void updateNumberOfWaves(int newNum)
	{
		numberOfWaves.text = "Wave: " + newNum.ToString();
	}

	public void quitGame()
	{
		Debug.Log("Quitting Game..");
	}

	public void loadScene(int index)
	{
		SceneManager.LoadScene(index);
	}
}
