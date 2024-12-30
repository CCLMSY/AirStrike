using UnityEngine;
using System.Collections;
using HWRcomponent;
namespace AirStrikeKit
{
	[RequireComponent (typeof(PlayerController))]
	[RequireComponent (typeof(Indicator))]
	[RequireComponent (typeof(RadarSystem))]
	[RequireComponent (typeof(PlayerDead))]

	public class PlayerManager : MonoBehaviour
	{
		[HideInInspector]
		public PlayerController PlayerControl;
		[HideInInspector]
		public Indicator Indicate;

		void Awake ()
		{
			Indicate = this.GetComponent<Indicator> ();
			PlayerControl = this.GetComponent<PlayerController> ();
		}

		void Start ()
		{
			FlightView view = (FlightView)GameObject.FindObjectOfType (typeof(FlightView));
			if (Indicate.CockpitCamera.Length > 0) {
				for (int i = 0; i < Indicate.CockpitCamera.Length; i++) {
					if (Indicate.CockpitCamera [i] != null) {
						view.AddCamera (Indicate.CockpitCamera [i].gameObject);
					}
				}
			}
		}
	}
}