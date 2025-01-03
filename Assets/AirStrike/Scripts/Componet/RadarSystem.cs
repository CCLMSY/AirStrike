using UnityEngine;
using System.Collections;
using HWRcomponent;

namespace HWRcomponent
{
	public enum Alignment
	{
		None,
		LeftTop,
		RightTop,
		LeftBot,
		RightBot,
		MiddleTop,
		MiddleBot
	}

	public class RadarSystem : MonoBehaviour
	//雷达系统。使用标签[]检测对象并在小地图上显示
	{

		private Vector2 inposition;
		public float Size = 400;
		// 雷达图大小
		public float Distance = 100;
		// 最大距离
		public float Alpha = 0.5f;
		public Texture2D[] Navtexture;
		public string[] EnemyTag;
		// 对象Tag列表
		public Texture2D NavCompass;
		public Texture2D NavBG;
		public Vector2 PositionOffset = new Vector2 (0, 0);
		public float Scale = 1;
		public Alignment PositionAlignment = Alignment.None;
		public bool MapRotation;
		public GameObject Player;
		public bool Show = true;
		public Color ColorMult = Color.white;

		void Start ()
		{
	
		}

		void Update ()
		{
			if (!Player) {
				Player = this.gameObject;
			}
		
			if (Scale <= 0) {
				Scale = 1;
			}
			// 定义雷达图位置
			switch (PositionAlignment) {
			case Alignment.None:
				inposition = PositionOffset;
				break;
			case Alignment.LeftTop:
				inposition = Vector2.zero + PositionOffset;
				break;
			case Alignment.RightTop:
				inposition = new Vector2 (Screen.width - Size, 0) + PositionOffset;
				break;
			case Alignment.LeftBot:
				inposition = new Vector2 (0, Screen.height - Size) + PositionOffset;
				break;
			case Alignment.RightBot:
				inposition = new Vector2 (Screen.width - Size, Screen.height - Size) + PositionOffset;
				break;
			case Alignment.MiddleTop:
				inposition = new Vector2 ((Screen.width / 2) - (Size / 2), Size) + PositionOffset;
				break;
			case Alignment.MiddleBot:
				inposition = new Vector2 ((Screen.width / 2) - (Size / 2), Screen.height - Size) + PositionOffset;
				break;
			}
		
		}
		// 将3D坐标转换为2D坐标
		Vector2 ConvertToNavPosition (Vector3 pos)
		{
			Vector2 res = Vector2.zero;
			if (Player) {
				res.x = inposition.x + (((pos.x - Player.transform.position.x) + (Size * Scale) / 2f) / Scale);
				res.y = inposition.y + ((-(pos.z - Player.transform.position.z) + (Size * Scale) / 2f) / Scale);
			}
			return res;
		}

		void DrawNav (GameObject[] enemylists, Texture2D navtexture)
		{
			if (Player) {
				for (int i = 0; i < enemylists.Length; i++) {
					if (Vector3.Distance (Player.transform.position, enemylists [i].transform.position) <= (Distance * Scale)) {
						Vector2 pos = ConvertToNavPosition (enemylists [i].transform.position);
				
						if (Vector2.Distance (pos, (inposition + new Vector2 (Size / 2f, Size / 2f))) + (navtexture.width / 2) < (Size / 2f)) {
							float navscale = Scale;
							if (navscale < 1) {
								navscale = 1;
							}
							GUI.DrawTexture (new Rect (pos.x - (navtexture.width / navscale) / 2, pos.y - (navtexture.height / navscale) / 2, navtexture.width / navscale, navtexture.height / navscale), navtexture);
						}
					}
				}
			}
		}

		float[] list;

		void OnGUI ()
		{
			if (!Show)
				return;
		
			GUI.color = new Color (ColorMult.r, ColorMult.g, ColorMult.b, Alpha);
			if (MapRotation) {
				GUIUtility.RotateAroundPivot (-(this.transform.eulerAngles.y), inposition + new Vector2 (Size / 2f, Size / 2f)); 
			}
	
			for (int i = 0; i < EnemyTag.Length; i++) {
				DrawNav (GameObject.FindGameObjectsWithTag (EnemyTag [i]), Navtexture [i]);
			}
			if (NavBG)
				GUI.DrawTexture (new Rect (inposition.x, inposition.y, Size, Size), NavBG);
			GUIUtility.RotateAroundPivot ((this.transform.eulerAngles.y), inposition + new Vector2 (Size / 2f, Size / 2f)); 
			if (NavCompass)
				GUI.DrawTexture (new Rect (inposition.x + (Size / 2f) - (NavCompass.width / 2f), inposition.y + (Size / 2f) - (NavCompass.height / 2f), NavCompass.width, NavCompass.height), NavCompass);

		}
	}


}

