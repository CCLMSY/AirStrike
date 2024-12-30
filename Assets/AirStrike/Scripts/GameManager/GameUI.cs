using UnityEngine;
using System.Collections;
using HWRWeaponSystem;
using UnityEngine.SceneManagement;

namespace AirStrikeKit
{
	public class GameUI : MonoBehaviour
	{

		public GUISkin skin;
		public Texture2D Logo;
		public int Mode;
		private WeaponController weapon;


		void Awake(){
			AirStrikeGame.gameUI = this;
		}

		void Start ()
		{
			weapon = AirStrikeGame.playerController.GetComponent<WeaponController> ();
		}

		public void OnGUI ()
		{
		
			if (skin)
				GUI.skin = skin;
		
		
			switch (Mode) {
			case 0: // 游戏中
				if (Input.GetKeyDown (KeyCode.Escape)) { // 暂停
					Mode = 2;	
				}
			
				if (AirStrikeGame.playerController) { 
				
					AirStrikeGame.playerController.Active = true; // 激活玩家控制器
			
					GUI.skin.label.alignment = TextAnchor.UpperLeft;
					GUI.skin.label.fontSize = 30;
					GUI.Label (new Rect (20, 20, 200, 50), "击杀数：" + AirStrikeGame.gameManager.Killed.ToString ());
					GUI.Label (new Rect (20, 60, 200, 50), "分  数：" + AirStrikeGame.gameManager.Score.ToString ());
				
					GUI.skin.label.alignment = TextAnchor.UpperRight;
					GUI.Label (new Rect (Screen.width - 220, 20, 200, 50), "生命值：" + AirStrikeGame.playerController.GetComponent<DamageManager> ().HP);
					GUI.skin.label.fontSize = 16;
				
		
					if (weapon.WeaponLists [weapon.CurrentWeapon].Icon)
						GUI.DrawTexture (new Rect (Screen.width - 100, Screen.height - 100, 80, 80), weapon.WeaponLists [weapon.CurrentWeapon].Icon);
				
					GUI.skin.label.alignment = TextAnchor.UpperRight;
					if (weapon.WeaponLists [weapon.CurrentWeapon].Ammo <= 0 && weapon.WeaponLists [weapon.CurrentWeapon].ReloadingProcess > 0) {
						if (!weapon.WeaponLists [weapon.CurrentWeapon].InfinityAmmo)
							GUI.Label (new Rect (Screen.width - 230, Screen.height - 120, 200, 30), "装弹中..." + Mathf.Floor ((1 - weapon.WeaponLists [weapon.CurrentWeapon].ReloadingProcess) * 100) + "%");
					} else {
						if (!weapon.WeaponLists [weapon.CurrentWeapon].InfinityAmmo)
							GUI.Label (new Rect (Screen.width - 230, Screen.height - 120, 200, 30), weapon.WeaponLists [weapon.CurrentWeapon].Ammo.ToString ());
					}
	
					if(SceneManager.GetActiveScene().name == "TakeOff"){
						GUI.skin.label.alignment = TextAnchor.UpperLeft;
						GUI.Label (new Rect (Screen.width-250, Screen.height/2, 250, 250), "【操作方式】\nA/D：左转/右转\nC：切换视角\n鼠标左右平移：左/右翻转\n鼠标前后平移：爬升/降落\n鼠标左键：开火\n鼠标右键：切换武器");
					}

				} else {
					AirStrikeGame.playerController = (PlayerController)GameObject.FindObjectOfType (typeof(PlayerController));
					if (AirStrikeGame.playerController)
						weapon = AirStrikeGame.playerController.GetComponent<WeaponController> ();
				}
				break;
			case 1: // 游戏结束
				if (AirStrikeGame.playerController)
					AirStrikeGame.playerController.Active = false;
			
				MouseLock.MouseLocked = false;
			
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUI.Label (new Rect (0, Screen.height / 2 + 10, Screen.width, 30), "游戏结束");
		
				GUI.DrawTexture (new Rect (Screen.width / 2 - Logo.width / 2, Screen.height / 2 - 150, Logo.width, Logo.height), Logo);
		
				if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 50, 300, 40), "重新开始")) {
					// Application.LoadLevel (Application.loadedLevelName);
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			
				}
				if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 100, 300, 40), "主菜单")) {
					// Application.LoadLevel ("Mainmenu");
					SceneManager.LoadScene("Mainmenu");
				}
				break;
		
			case 2: // 游戏暂停
				if (AirStrikeGame.playerController)
					AirStrikeGame.playerController.Active = false;
			
				MouseLock.MouseLocked = false;
				Time.timeScale = 0;
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUI.Label (new Rect (0, Screen.height / 2 + 10, Screen.width, 30), "游戏暂停");
		
				GUI.DrawTexture (new Rect (Screen.width / 2 - Logo.width / 2, Screen.height / 2 - 150, Logo.width, Logo.height), Logo);
		
				if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 50, 300, 40), "继续游戏")) {
					Mode = 0;
					Time.timeScale = 1;
				}
				if (GUI.Button (new Rect (Screen.width / 2 - 150, Screen.height / 2 + 100, 300, 40), "主菜单")) {
					Time.timeScale = 1;
					Mode = 0;
					// Application.LoadLevel ("Mainmenu");
					SceneManager.LoadScene("Mainmenu");
				}
				break;
			
			}
		
		}
	}
}