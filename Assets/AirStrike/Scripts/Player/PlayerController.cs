using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	[RequireComponent (typeof(FlightSystem))]

	public class PlayerController : MonoBehaviour
	{
	
		FlightSystem flight;
		FlightView View;
		public bool Active = true;
		public bool SimpleControl;
		public bool Acceleration;
		public float AccelerationSensitivity = 5;
		private TouchScreenVal controllerTouch;
		private TouchScreenVal fireTouch;
		private TouchScreenVal switchTouch;
		private TouchScreenVal sliceTouch;
		public GUISkin skin;
		public bool ShowHowto;


		void Awake(){
			AirStrikeGame.playerController = this;
		}

		void Start ()
		{
			flight = this.GetComponent<FlightSystem> ();
			View = (FlightView)GameObject.FindObjectOfType (typeof(FlightView));
			controllerTouch = new TouchScreenVal (new Rect (0, 0, Screen.width / 2, Screen.height - 100));
			fireTouch = new TouchScreenVal (new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height));
			switchTouch = new TouchScreenVal (new Rect (0, Screen.height - 100, Screen.width / 2, 100));
			sliceTouch = new TouchScreenVal (new Rect (0, 0, Screen.width / 2, 50));

		}

		void Update ()
		{
			if (!flight || !Active)
				return;
			#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
			DesktopController ();
			#else
			MobileController ();
			#endif
		
		}

		void DesktopController ()
		{
			flight.SimpleControl = SimpleControl;
		
			MouseLock.MouseLocked = true;
		
			flight.AxisControl (new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y")));

			if (SimpleControl) {
				flight.TurnControl (Input.GetAxis ("Mouse X"));
			} 

			flight.TurnControl (Input.GetAxis ("Horizontal"));
			flight.SpeedUp (Input.GetAxis ("Vertical"));
		
		
			if (Input.GetButton ("Fire1")) {
				flight.WeaponControl.LaunchWeapon ();
			}
		
			if (Input.GetButtonDown ("Fire2")) {
				flight.WeaponControl.SwitchWeapon ();
			}
		
			if (Input.GetKeyDown (KeyCode.C)) {
				if (View)
					View.SwitchCameras ();	
			}	
		}

		void MobileController ()
		{
		
			flight.SimpleControl = SimpleControl;
		
			if (Acceleration) {
				Vector3 acceleration = Input.acceleration;
				Vector2 accValActive = new Vector2 (acceleration.x, (acceleration.y + 0.3f) * 0.5f) * AccelerationSensitivity;
				flight.FixedX = false;
				flight.FixedY = false;
				flight.FixedZ = true;
			
				flight.AxisControl (accValActive);
				flight.TurnControl (accValActive.x);
			} else {
				flight.FixedX = true;
				flight.FixedY = false;
				flight.FixedZ = true;
				Vector2 dir = controllerTouch.OnDragDirection (true);
				dir = Vector2.ClampMagnitude (dir, 1.0f);
				flight.AxisControl (new Vector2 (dir.x, -dir.y) * AccelerationSensitivity * 0.7f);
				flight.TurnControl (dir.x * AccelerationSensitivity * 0.3f);
			}
			sliceTouch.OnDragDirection (true);
			flight.SpeedUp (sliceTouch.slideVal.x);
		
			if (fireTouch.OnTouchPress ()) {
				flight.WeaponControl.LaunchWeapon ();
			}	
			if (switchTouch.OnTouchPress ()) {
		
			}	
		}
	
	
		// you can remove this part..
		// void OnGUI ()
		// {
		// 	if (!ShowHowto)
		// 		return;
		
		// 	if (skin)
		// 		GUI.skin = skin;

		// 	if (GUI.Button (new Rect (20, 150, 200, 40), "Gyroscope " + Acceleration)) {
		// 		Acceleration = !Acceleration;
		// 	}
		
		// 	if (GUI.Button (new Rect (20, 200, 200, 40), "Change View")) {
		// 		if (View)
		// 			View.SwitchCameras ();	
		// 	}
		
		// 	if (GUI.Button (new Rect (20, 250, 200, 40), "Change Weapons")) {
		// 		if (flight)
		// 			flight.WeaponControl.SwitchWeapon ();
		// 	}
		
		// 	if (GUI.Button (new Rect (20, 300, 200, 40), "Simple Control " + SimpleControl)) {
		// 		if (flight)
		// 			SimpleControl = !SimpleControl;
		// 	}

		// 	GUI.Label (new Rect (20, 350, 500, 40), "you can remove this in OnGUI in PlayerController.cs");
		// }
	}
}
