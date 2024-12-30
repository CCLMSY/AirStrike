using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class GameManager : MonoBehaviour
	{
		// 游戏分数
		public int Score = 0;
		public int Killed = 0;

		void Awake ()
		{
			AirStrikeGame.gameManager = this;
		}

		void Start ()
		{
			Score = 0;
			Killed = 0;
		}
	
		// 每帧调用一次Update函数
		void Update ()
		{
		
		}
		// 加分
		public void AddScore (int score)
		{
			Score += score;
			Killed += 1;
		}

		void OnGUI ()
		{
			//GUI.Label(new Rect(20,20,300,30),"击杀："+Score);
		}
		// 游戏结束
		public void GameOver ()
		{
			if (AirStrikeGame.gameUI) {
				AirStrikeGame.gameUI.Mode = 1;	
			}
		}
	}

	public static class AirStrikeGame
	{
		public static GameManager gameManager;
		public static GameUI gameUI;
		public static PlayerController playerController;
	}
}