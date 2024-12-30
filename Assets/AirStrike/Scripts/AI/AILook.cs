using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

namespace AirStrikeKit
{
	[RequireComponent (typeof(WeaponController))]

	public class AILook : MonoBehaviour
	{
		public string[] TargetTag = new string[1]{ "Enemy" };
		// 这个AI只会射击与TargetTag[]中相同标签的对象。
		private int indexWeapon;
		private GameObject target;
		private WeaponController weapon;
		private float timeAIattack;
		private float delay;
		public float RandomDelay = 10;

		void Start ()
		{
			delay = Random.Range (0, RandomDelay);
			weapon = (WeaponController)this.GetComponent<WeaponController> ();
		}

	
		void Update ()
		{
			if (Time.time < delay) {
				return;
			}
			// 如果目标存在
			if (target) {
				// 朝向目标
				Quaternion targetlook = Quaternion.LookRotation (target.transform.position - this.transform.position);
				this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetlook, Time.deltaTime * 3);
				// 计算目标方向
				Vector3 dir = (target.transform.position - transform.position).normalized;
				float direction = Vector3.Dot (dir, transform.forward);
				// 如果目标在前方
				if (direction > 0.9f) {
					if (weapon) {
						// 射击目标！！
						weapon.LaunchWeapon (indexWeapon);
					}
				}
				// AI 攻击目标3秒后忘记目标，寻找新目标
				if (Time.time > timeAIattack + 3) {
					target = null;	
				}
			} else {
				for (int t = 0; t < TargetTag.Length; t++) {
					// AI 寻找目标

					if (AirStrike.AIPool == null) {
						Debug.LogError ("需要AIManager放置在当前场景中");
						return;
					}
					TargetCollector targetCollector = AirStrike.AIPool.FindTargetTag (TargetTag [t]);
				
					if (targetCollector != null && targetCollector.Targets.Length > 0) {
						
						// 找到最近的目标
						float distance = int.MaxValue;
						for (int i = 0; i < targetCollector.Targets.Length; i++) {
							if (targetCollector.Targets [i] != null) {
								// 计算目标距离
								float dis = Vector3.Distance (targetCollector.Targets [i].transform.position, transform.position);
								// 如果目标在视野范围内
								if (distance > dis) {
									// 保存目标
									distance = dis;
									target = targetCollector.Targets [i];
									if (weapon) {
										// 随机选择武器
										indexWeapon = Random.Range (0, weapon.WeaponLists.Length);
									}
									timeAIattack = Time.time;
								}
							}
						}
					}
				}	
			}
		}
	}
}