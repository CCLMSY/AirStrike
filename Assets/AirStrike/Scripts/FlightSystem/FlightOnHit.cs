using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

namespace AirStrikeKit
{
	public class FlightOnHit : MonoBehaviour
	// 飞行中受到攻击。如果被列表中标记的对象击中，将对飞机造成伤害。
	{
	
		public string[] Tag = new string[1]{ "Scene" };
		public string AirportTag = "Airport";
		public int Damage = 100;
		public AudioClip[] SoundOnHit;

		void Start ()
		{
		
		}

		private void OnCollisionEnter (Collision collision)
		{
			bool hit = false;
		
			for (int i = 0; i < Tag.Length; i++) {
				if (collision.gameObject.tag == Tag [i]) {
					hit = true;
				}
				if (collision.gameObject.tag == AirportTag) {
					hit = false;
				}

			}
		
			if (hit) {
				if (SoundOnHit.Length > 0)
					AudioSource.PlayClipAtPoint (SoundOnHit [Random.Range (0, SoundOnHit.Length)], this.transform.position);
				this.transform.root.SendMessage ("ApplyDamage", Damage, SendMessageOptions.DontRequireReceiver);

			}	
		}

		private void OnCollisionStay (Collision collision)
		{
			if (collision.gameObject.tag == AirportTag) {
				this.transform.root.SendMessage ("Landing", SendMessageOptions.DontRequireReceiver);
			}
		}

	}
}