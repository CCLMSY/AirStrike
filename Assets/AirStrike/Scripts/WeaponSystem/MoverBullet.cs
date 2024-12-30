using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class MoverBullet : WeaponBase
	{
		public int Lifetime;
		public float Speed = 80;
		public float SpeedMax = 80;
		public float SpeedMult = 1;
		private float speedTemp;
	
		private void Awake ()
		{
			speedTemp = Speed;
			objectPool = this.GetComponent<ObjectPool> ();
		}
	
		private void Start ()
		{
			if (objectPool && WeaponSystem.Pool != null) {
				objectPool.SetDestroy (Lifetime);
			} else {
				Destroy (gameObject, Lifetime);
			}
		}
	
		public void OnEnable ()
		{
			Speed = speedTemp;
			if (objectPool && WeaponSystem.Pool != null) {
				objectPool.SetDestroy (Lifetime);
			}
		}

		private void FixedUpdate ()
		{
			if (WeaponSystem.Pool != null && objectPool != null && !objectPool.Active) 
				return;
		
			if (!this.GetComponent<Rigidbody>())
				return;
		
			if (!RigidbodyProjectile) {
				GetComponent<Rigidbody>().velocity = transform.forward * Speed;
			} else {
				if (this.GetComponent<Rigidbody>().velocity.normalized != Vector3.zero)
					this.transform.forward = this.GetComponent<Rigidbody>().velocity.normalized;	
			}
			if (Speed < SpeedMax) {
				Speed += SpeedMult * Time.fixedDeltaTime;
			}
		}
	}
}
