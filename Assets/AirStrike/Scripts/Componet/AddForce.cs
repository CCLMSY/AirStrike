/// <summary>
/// Add force. this script will adding force to itself when start.
/// </summary>
using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class AddForce : MonoBehaviour
	{
		public Vector3 Force;
		public bool RandomForce;
		public Vector3 TurqueForce;
		public bool RandomTurque;
		private Rigidbody rigidBody;

		void Start ()
		{
			rigidBody = this.GetComponent<Rigidbody> ();
			if (!rigidBody)
				return;

			if (RandomForce) {
				Force = new Vector3 (Random.Range (-Force.x, Force.x), Random.Range (-Force.y, Force.y), Random.Range (-Force.z, Force.z));
			}
		
			rigidBody.AddForce (Force);
		
			if (RandomTurque) {
				TurqueForce = new Vector3 (Random.Range (-TurqueForce.x, TurqueForce.x), Random.Range (-TurqueForce.y, TurqueForce.y), Random.Range (-TurqueForce.z, TurqueForce.z));
			}
		
			rigidBody.AddTorque (TurqueForce);
		}
	}
}