using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	[RequireComponent (typeof(Camera))]

	public class FlightView : MonoBehaviour
	// 跟随飞机的相机
	{

		public GameObject Target;
		// 玩家
		public GameObject[] Cameras;
		// 场景中的所有相机，用于切换视角
		public float FollowSpeedMult = 0.5f;
		// 跟随速度
		public float TurnSpeedMult = 5;
		// 旋转速度
		private int indexCamera;
		// 当前相机索引
		public Vector3 Offset = new Vector3 (10, 0.5f, 10);
		// 相机和飞机之间的偏移量
	
		public void SwitchCameras ()
		// 切换相机
		{
			indexCamera += 1;
			if (indexCamera >= Cameras.Length) {
				indexCamera = 0;
			}
			for (int i = 0; i < Cameras.Length; i++) {
				if (Cameras [i] && Cameras [i].GetComponent<Camera>())
					Cameras [i].GetComponent<Camera>().enabled = false;
				if (Cameras [i] && Cameras [i].GetComponent<AudioListener> ())
					Cameras [i].GetComponent<AudioListener> ().enabled = false;
			}
			if (Cameras [indexCamera]) {
				if (Cameras [indexCamera] && Cameras [indexCamera].GetComponent<Camera>())
					Cameras [indexCamera].GetComponent<Camera>().enabled = true;
				if (Cameras [indexCamera] && Cameras [indexCamera].GetComponent<AudioListener> ())
					Cameras [indexCamera].GetComponent<AudioListener> ().enabled = true;
			}
		}

		void Awake ()
		{
			// 添加相机到主相机
			AddCamera (this.gameObject);
		}

		public void AddCamera (GameObject cam)
		{
			GameObject[] temp = new GameObject[Cameras.Length + 1];
			Cameras.CopyTo (temp, 0);
			Cameras = temp;
			Cameras [temp.Length - 1] = cam;
		}

		void Start ()
		{
			// 如果玩家没有被包含，尝试找到玩家
			if (!Target) {
				PlayerManager player = (PlayerManager)GameObject.FindObjectOfType (typeof(PlayerManager));	
				if (player)
					Target = player.gameObject;
			}
		}

		Vector3 positionTargetUp;

		void FixedUpdate ()
		{
			if (!Target)
				return;
		
			// 旋转，沿着玩家移动
			// Quaternion lookAtRotation = Quaternion.LookRotation (Target.transform.position);
			this.transform.LookAt (Target.transform.position + (Target.transform.forward * Offset.x));
			positionTargetUp = Vector3.Lerp (positionTargetUp, (-Target.transform.forward + (Target.transform.up * Offset.y)), Time.fixedDeltaTime * TurnSpeedMult);
			Vector3 positionTarget = Target.transform.position + (positionTargetUp * Offset.z);
			float distance = Vector3.Distance (positionTarget, this.transform.position);
			this.transform.position = Vector3.Lerp (this.transform.position, positionTarget, Time.fixedDeltaTime * (distance * FollowSpeedMult));
		
		}

		void Update ()
		{
		
			bool activecheck = false;
			for (int i = 0; i < Cameras.Length; i++) {
				if (Cameras [i] && Cameras [i].GetComponent<Camera>().enabled) {
					activecheck = true;
					break;	
				}
			}
			if (!activecheck) {
				this.GetComponent<Camera>().enabled = true;
				if (this.gameObject.GetComponent<AudioListener> ())
					this.gameObject.GetComponent<AudioListener> ().enabled = true;
			}
		}
	}
}