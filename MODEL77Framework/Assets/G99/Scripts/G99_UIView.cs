using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G99_UIView : MonoBehaviour {

	[SerializeField]
	private Text _count;
	[SerializeField]
	private Text _gameOver;
	
	public void SetTargetCount(int targetCount)
	{
		_count.text = targetCount.ToString();
	}

	public void DisplayGameEnd()
	{
		_gameOver.enabled = true;
	}
}
