using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class DamageManager : MonoBehaviour
	{
		public GameObject Effect;
		public int HP = 100;
		private ObjectPool objPool;
		[HideInInspector]
		public GameObject LatestHit;

		private void Awake ()
		{
			objPool = this.GetComponent<ObjectPool> ();	
		}

		private void Start ()
		{

		}

		public void ApplyDamage (int damage)
		{
			if (HP < 0)
				return;

			HP -= damage;
			if (HP <= 0) {
				Dead ();
			}
		}

		public void ApplyDamage (DamagePack damage)
		{
			if (HP < 0)
				return;

			LatestHit = damage.Owner;
			HP -= damage.Damage;
			if (HP <= 0) {
				Dead ();
			}
		}

		public void Dead ()
		{
			if (Effect) {
				GameObject deadobj = null;
				if (WeaponSystem.Pool != null && Effect.GetComponent<ObjectPool> ()) {
					deadobj = (GameObject)WeaponSystem.Pool.Instantiate (Effect, transform.position, transform.rotation);
				} else {
					deadobj = (GameObject)GameObject.Instantiate (Effect, transform.position, transform.rotation);
				}
				if(deadobj!=null){
					if(deadobj.GetComponent<Rigidbody>() != null && this.GetComponent<Rigidbody>() != null){
						deadobj.GetComponent<Rigidbody>().AddForce(this.GetComponent<Rigidbody>().velocity,ForceMode.VelocityChange);
					}
				}
			}
			if (objPool != null) {
				objPool.Destroying ();
			} else {
				Destroy (this.gameObject);
			}
			this.gameObject.SendMessage ("OnDead", SendMessageOptions.DontRequireReceiver);
		}

	}
}
