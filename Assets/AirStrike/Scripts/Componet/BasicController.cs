using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class BasicController : MonoBehaviour
	{
		void Start ()
		{
	
		}
		void Update ()
		{
			if (Input.GetKey (KeyCode.W)) {
				this.transform.position += new Vector3 (0, 0, 1);
			}
			if (Input.GetKey (KeyCode.A)) {
				this.transform.position += new Vector3 (1, 0, 0);
			}
			if (Input.GetKey (KeyCode.S)) {
				this.transform.position += new Vector3 (0, 0, -1);
			}
			if (Input.GetKey (KeyCode.D)) {
				this.transform.position += new Vector3 (-1, 0, 0);
			}
			if (Input.GetKey (KeyCode.Q)) {
				this.transform.position += new Vector3 (0, 1, 0);
			}
			if (Input.GetKey (KeyCode.E)) {
				this.transform.position += new Vector3 (0, -1, 0);
			}
		}
	}
}