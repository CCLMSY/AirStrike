using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class RayShootAimBot : WeaponBase
	{
		void OnEnable(){

			if(Target != null){
				this.transform.forward = (Target.transform.position - this.transform.position).normalized;
			}
		}

	}
	
}
