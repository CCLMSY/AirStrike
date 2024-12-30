using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

namespace AirStrikeKit
{
	[RequireComponent (typeof(FlightSystem))]

	public class FlightOnDead : MonoBehaviour
	{

		protected FlightSystem flight;

		void Awake ()
		{
			flight = this.GetComponent<FlightSystem> ();
		}

		void Start ()
		{
		
		}

		public virtual void OnDead ()
		{
		
		}
	}
}