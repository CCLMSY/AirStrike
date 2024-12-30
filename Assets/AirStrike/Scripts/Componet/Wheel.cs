using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class Wheel : MonoBehaviour
	{

		public Vector3 Axis = Vector3.one;

		void Start ()
		{
	
		}
		private void FixedUpdate ()
		{
			this.transform.Rotate (Axis * Time.fixedDeltaTime);
		}
	}
}