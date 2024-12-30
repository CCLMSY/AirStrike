using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace AirStrikeKit
{
	public class Mainmeu : MonoBehaviour
	{

		public GUISkin skin;
		public Texture2D Logo;

		void Start ()
		{
			// Camera.main.aspect = (float)Screen.width / Screen.height;
		}

		public void OnGUI ()
		{
			if (skin)
				GUI.skin = skin;
			
			GUI.DrawTexture (new Rect (Screen.width / 2 - Logo.width / 2, Screen.height / 2 - 200, Logo.width, Logo.height), Logo);
		
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2, 300, 40), "未来星际（简单）")) {
				// Application.LoadLevel ("Classic");
				SceneManager.LoadScene("StarFighter");
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 50, 300, 40), "现代模式（中等）")) {
				// Application.LoadLevel ("Modern");
				SceneManager.LoadScene("Modern");
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 100, 300, 40), "现代黑夜（中等）")) {
				// Application.LoadLevel ("Invasion");
				SceneManager.LoadScene("Invasion");
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 150, 300, 40), "近代模式（困难）")) {
				// Application.LoadLevel ("StarFighter");
				SceneManager.LoadScene("Classic");
			}
			if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 200, 300, 40), "退出游戏")) {
				Application.Quit ();
			}
		
			// GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			// GUI.Label (new Rect (0, Screen.height - 90, Screen.width, 50), "Air strike starter kit. by Rachan Neamprasert\n www.hardworkerstudio.com");
		}
	}
}