using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

namespace AirStrikeKit
{
	// 包含飞机的所有必要组件
	[RequireComponent (typeof(Rigidbody))]
	[RequireComponent (typeof(Collider))]
	[RequireComponent (typeof(WeaponController))]


	public class FlightSystem : DamageManager
	// 核心飞机系统
	{
	
		public float AccelerationSpeed = 0.5f;
		// 每帧的加速度
		public float Speed = 50.0f;
		// 初始速度
		public float SpeedMax = 60.0f;
		// 最大速度
		public float RotationSpeed = 10.0f;
		// 转向速度
		public float SpeedTakeoff = 40;
		// 起飞最小速度
		public float SpeedPitch = 2;
		// 旋转X
		public float SpeedRoll = 3;
		// 旋转Z
		public float SpeedYaw = 1;
		// 旋转Y
		public float DampingTarget = 10.0f;
		// 旋转速度
		public bool AutoPilot = false;
		// 自动跟随目标
		private float MoveSpeed = 0;
		// 巡航速度
		public float VelocitySpeed = 0;
		// 加速速度

		[HideInInspector]
		public bool SimpleControl = false;

		[HideInInspector]
		public bool FollowTarget = false;
		[HideInInspector]
		public Vector3 PositionTarget = Vector3.zero;

		[HideInInspector]
		public WeaponController WeaponControl;
		// 武器系统
		private Vector3 positionTarget = Vector3.zero;
		private Quaternion mainRot = Quaternion.identity;
		[HideInInspector]
		public float roll = 0;
		[HideInInspector]
		public float pitch = 0;
		[HideInInspector]
		public float yaw = 0;
		public Vector2 LimitAxisControl = new Vector2 (2, 1);
		// limited of axis rotation magnitude
		public bool FixedX;
		public bool FixedY;
		public bool FixedZ;
		public bool IsLanding;
		private float gravityVelocity = 0;

		void Start ()
		{
			WeaponControl = this.gameObject.GetComponent<WeaponController> ();
			mainRot = this.transform.rotation;
			MoveSpeed = Speed;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

		[HideInInspector]
		public float normalFlySpeed;

		float SignedAngleBetween (Vector3 a, Vector3 b, Vector3 n)
		{
			float angle = Vector3.Angle (a, b);
			float sign = Mathf.Sign (Vector3.Dot (n, Vector3.Cross (a, b)));

			float signed_angle = angle * sign;

			return signed_angle;
		}


		void FixedUpdate ()
		{
			if (!this.GetComponent<Rigidbody>())
				return;

			Quaternion AddRot = Quaternion.identity;
			Vector3 velocityTarget = Vector3.zero;
			normalFlySpeed = Mathf.Clamp (VelocitySpeed / (SpeedTakeoff * 2), 0, 1);

			if (AutoPilot) {// 自动驾驶
				if (FollowTarget) {
					// 转向目标
					Vector3 targetDir = PositionTarget - this.transform.position;
					float directToGo = Vector3.Dot (this.transform.forward.normalized, targetDir.normalized);
					float rotateMult = 1;

			
					if (directToGo > 0.3f) {
						rotateMult = 1;
						positionTarget = Vector3.Lerp (positionTarget, PositionTarget, Time.fixedDeltaTime * DampingTarget);
					} else {
						Vector3 reflectionVector = Vector3.Reflect (targetDir.normalized, this.transform.forward);
						reflectionVector.x *= 0.3f;
						reflectionVector.y *= 0.5f;
						positionTarget = Vector3.Slerp (positionTarget, this.transform.position + reflectionVector, Time.fixedDeltaTime * DampingTarget);
						rotateMult = 0.5f;
						if (directToGo < -0.9) {
							positionTarget.y += 0.1f;
						}
					}

					mainRot = Quaternion.LookRotation (positionTarget - this.transform.position);
					Vector3 relativePoint = this.transform.InverseTransformPoint (positionTarget).normalized;
					this.GetComponent<Rigidbody>().rotation = Quaternion.Lerp (GetComponent<Rigidbody>().rotation, mainRot, rotateMult * 0.1f * RotationSpeed * Time.fixedDeltaTime);
					this.GetComponent<Rigidbody>().rotation *= Quaternion.Euler (0, 0, -relativePoint.x * 150 * SpeedRoll * Time.fixedDeltaTime);

				}
				velocityTarget = (GetComponent<Rigidbody>().rotation * Vector3.forward) * VelocitySpeed;
			} else {
				// 通过输入控制轴
				if (!IsLanding) {
					AddRot.eulerAngles = new Vector3 (pitch + ((1 - normalFlySpeed) * 0.5f), yaw, -roll);
				} else {
					AddRot.eulerAngles = new Vector3 (pitch, yaw, -roll);
				}

				mainRot *= AddRot;

				if (SimpleControl) {
					Quaternion saveQ = mainRot;
				
					Vector3 fixedAngles = new Vector3 (mainRot.eulerAngles.x, mainRot.eulerAngles.y, mainRot.eulerAngles.z);
				
					if (FixedX)
						fixedAngles.x = 1;
					if (FixedY)
						fixedAngles.y = 1;
					if (FixedZ)
						fixedAngles.z = 1;
				
					saveQ.eulerAngles = fixedAngles;
					mainRot = Quaternion.Lerp (mainRot, saveQ, Time.fixedDeltaTime * 2);
				}

				velocityTarget = (GetComponent<Rigidbody>().rotation * Vector3.forward) * VelocitySpeed;
				GetComponent<Rigidbody>().rotation = Quaternion.Lerp (GetComponent<Rigidbody>().rotation, mainRot, Time.fixedDeltaTime * RotationSpeed);
			}

			if (IsLanding) {
				Quaternion saveQ = mainRot;
				Vector3 fixedAngles = new Vector3 (mainRot.eulerAngles.x, mainRot.eulerAngles.y, mainRot.eulerAngles.z);
				fixedAngles.x = 1;
				fixedAngles.z = 1;
				gravityVelocity = 0;	
				saveQ.eulerAngles = fixedAngles;
				mainRot = Quaternion.Lerp (mainRot, saveQ, Time.fixedDeltaTime * 2);
			} else {
				if (GetComponent<Rigidbody>().useGravity) {
					gravityVelocity += (Physics.gravity.y * ((1 - Mathf.Clamp (VelocitySpeed / (SpeedTakeoff * 2), 0, 1)) + Vector3.Dot (Physics.gravity.normalized, velocityTarget.normalized + (Vector3.up * 0.5f)))) * Time.fixedDeltaTime;
					gravityVelocity = Mathf.Clamp (gravityVelocity, -float.MaxValue, 0);
					velocityTarget.y += gravityVelocity;
				}
			}


			yaw = Mathf.Lerp (yaw, 0, Time.deltaTime);
			Vector3 velocityChange = (velocityTarget - GetComponent<Rigidbody>().velocity);
			GetComponent<Rigidbody>().AddForce (velocityChange, ForceMode.VelocityChange);


			if (IsLanding) {
				roll = Mathf.Lerp (roll, 0, 0.5f);
				VelocitySpeed = (0 + MoveSpeed);
				if (speedDelta < 1)
					MoveSpeed = Mathf.Lerp (MoveSpeed, 0, Time.fixedDeltaTime * 0.2f);
			
			} else {
				VelocitySpeed = (Speed + MoveSpeed);
				if (speedDelta < 1)
					MoveSpeed = Mathf.Lerp (MoveSpeed, Speed, Time.fixedDeltaTime * 0.1f);
			
			}
			IsLanding = false;
		}



		public void Landing ()
		{
			IsLanding = true;
		}

		// 输入函数（滚转和俯仰）
		public void AxisControl (Vector2 axis)
		{
			if (SimpleControl) {
				LimitAxisControl.y = LimitAxisControl.x;	
			}
			if (!IsLanding) {
				roll = Mathf.Lerp (roll, Mathf.Clamp (axis.x, -LimitAxisControl.x, LimitAxisControl.x) * SpeedRoll, Time.deltaTime);
			}
			if (VelocitySpeed > SpeedTakeoff || !IsLanding) {
				float pitchVel = Mathf.Clamp (VelocitySpeed / (SpeedTakeoff * 2), 0, 1);
				pitch = Mathf.Lerp (pitch, Mathf.Clamp (axis.y, -LimitAxisControl.y, LimitAxisControl.y) * SpeedPitch, Time.deltaTime * pitchVel);
			}
		}

		//俯仰角度
		public void TurnControl (float turn)
		{
			yaw += turn * Time.deltaTime * SpeedYaw;
		}

		private float speedDelta;
		// 加速
		public void SpeedUp (float delta)
		{
			if (delta < 0)
				delta = 0;

			if (delta > 0)
				MoveSpeed = Mathf.Lerp (MoveSpeed, SpeedMax, Time.deltaTime * AccelerationSpeed);

			speedDelta = delta;
		}

		public void SpeedUp ()
		{
			MoveSpeed = Mathf.Lerp (MoveSpeed, SpeedMax, Time.deltaTime * AccelerationSpeed);
			speedDelta = 1;
		}
	}
}
