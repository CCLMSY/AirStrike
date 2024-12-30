using UnityEngine;
using System.Collections;

public class DeadEffect : MonoBehaviour
{


	void Start ()
	{
		if (this.GetComponent<Rigidbody>()) {
			if (this.GetComponent<Rigidbody>()) {
				this.GetComponent<Rigidbody>().AddTorque (Random.rotation.eulerAngles * Random.Range (100, 2000));
			}
		}
	}

}
