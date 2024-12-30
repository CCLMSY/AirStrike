using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public enum NavMode
	{
		Third,
		Cockpit,
		None
	}

	public class Indicator : MonoBehaviour
	// 显示敌人、驾驶舱HUD、准星并定义飞机摄像机。
	{
		public string[] TargetTag;
		public Texture2D[] NavTexture;
		public Texture2D Crosshair;
		public Texture2D Crosshair_in;
		public Vector2 CrosshairOffset;
		public Vector2 CrosshairOffset_in;
		public float DistanceSee = 800;
		public float Alpha = 0.7f;
	
		public Camera[] CockpitCamera;
		public int PrimaryCameraIndex;
	
		public bool Show = true;
	
		[HideInInspector]
		public NavMode Mode = NavMode.Third;
		[HideInInspector]
		public FlightSystem flight;

		void Awake ()
		{
			if (CockpitCamera.Length <= 0) {
				if (this.transform.GetComponentsInChildren (typeof(Camera)).Length > 0) {
					var cams = this.transform.GetComponentsInChildren (typeof(Camera));
					CockpitCamera = new Camera[cams.Length];
					for (int i = 0; i < cams.Length; i++) {
						CockpitCamera [i] = cams [i].GetComponent<Camera>();
					}
				}
			}
			flight = this.GetComponent<FlightSystem> ();
		}

		void Start ()
		{

		}

		public void DrawNavEnemy ()
		{
			// 找到所有目标标签
			for (int t = 0; t < TargetTag.Length; t++) {
				if (GameObject.FindGameObjectsWithTag (TargetTag [t]).Length > 0) {
					GameObject[] objs = GameObject.FindGameObjectsWithTag (TargetTag [t]);
					for (int i = 0; i < objs.Length; i++) {
						if (objs [i]) {
							Vector3 dir = (objs [i].transform.position - transform.position).normalized;
							float direction = Vector3.Dot (dir, transform.forward);
							if (direction >= 0.7f) {
								float dis = Vector3.Distance (objs [i].transform.position, transform.position);
								if (DistanceSee > dis) {
									DrawTargetLockon (objs [i].transform, t);
							
								}
							}
						}
					}
				}
			}
		}

		void OnGUI ()
		{
			if (Show) {
				GUI.color = new Color (1, 1, 1, Alpha);
				switch (Mode) {
				case NavMode.Third:
					if (Crosshair)
						GUI.DrawTexture (new Rect ((Screen.width / 2 - Crosshair.width / 2) + CrosshairOffset.x, (Screen.height / 2 - Crosshair.height / 2) + CrosshairOffset.y, Crosshair.width, Crosshair.height), Crosshair);	
					DrawNavEnemy ();
					break;
				case NavMode.Cockpit:
					if (Crosshair_in)
						GUI.DrawTexture (new Rect ((Screen.width / 2 - Crosshair_in.width / 2) + CrosshairOffset_in.x, (Screen.height / 2 - Crosshair_in.height / 2) + CrosshairOffset_in.y, Crosshair_in.width, Crosshair_in.height), Crosshair_in);	
					DrawNavEnemy ();
					break;
				case NavMode.None:
				
					break;
				}

			
			}
		}

		public void DrawTargetLockon (Transform aimtarget, int type)
		{
		
		
			if (CurrentCamera != null) {
				Vector3 dir = (aimtarget.position - CurrentCamera.transform.position).normalized;
				float direction = Vector3.Dot (dir, CurrentCamera.transform.forward);
				if (direction > 0.5f) {
					Vector3 screenPos = CurrentCamera.WorldToScreenPoint (aimtarget.transform.position);
					//float distance = Vector3.Distance (transform.position, aimtarget.transform.position);
				
					GUI.DrawTexture (new Rect (screenPos.x - NavTexture [type].width / 2, Screen.height - screenPos.y - NavTexture [type].height / 2, NavTexture [type].width, NavTexture [type].height), NavTexture [type]);
            	
				}
			}
		}

		public Camera CurrentCamera;

		void Update ()
		{
			if (CurrentCamera == null) {
			
				CurrentCamera = Camera.main;
			
				if (CurrentCamera == null)
					CurrentCamera = Camera.current;
			}
			if (Camera.current != null) {
				if (CurrentCamera != Camera.current) {
					CurrentCamera = Camera.current;
				}
			}
		
			Mode = NavMode.Third;
			for (int i = 0; i < CockpitCamera.Length; i++) {
				if (CockpitCamera [i] != null) {
					if (CockpitCamera [i].GetComponent<Camera>().enabled) {
						if (i == PrimaryCameraIndex)
							Mode = NavMode.Cockpit;	
					} 
				}
			}
		}
	}
}