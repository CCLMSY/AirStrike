using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class WeaponSystemManager : MonoBehaviour
	{

		void Awake ()
		{
			WeaponSystem.Pool = (ObjectPoolManager)GameObject.FindObjectOfType (typeof(ObjectPoolManager));	
		}
	
		void Start ()
		{
	
		}
	}
	
	public static class WeaponSystem
	{
		public static ObjectPoolManager Pool;
	}
}