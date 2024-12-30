using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

namespace AirStrikeKit
{
	public class DamageDecay : MonoBehaviour
	{

		private DamageManager damage;
		public int[] DamageLowerThan = { 10 };
		public GameObject[] DecayObject;

		void Start ()
		{
			damage = this.GetComponent<DamageManager> ();
		}

		void Update ()
		{
			if (damage == null || DecayObject.Length != DamageLowerThan.Length || DecayObject.Length <= 0)
				return;

			for (int i = 0; i < DecayObject.Length; i++) {
				if (damage.HP > DamageLowerThan [i]) {
					DecayObject [i].SetActive (false);
				}
			}

			for (int i = 0; i < DecayObject.Length; i++) {
				if (damage.HP < DamageLowerThan [i]) {
					DecayObject [i].SetActive (true);
				}
			}


		}
	}
}