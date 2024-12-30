using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class CameraSway : MonoBehaviour
	// 相机摇摆。 这个脚本将沿着父对象移动/摇摆一个对象。
	{
		public float SwaySensitivity = 10;
	
		private float parentMagnitude;
		private Vector3 positionTemp;
		private Quaternion rotationTemp;
		private Vector3 parentPositionMagnitude;
		private Vector3 parentPositionTemp;
		private Vector3 parentRotationMagnitude;
		private Quaternion parentRotationTemp;

	
		void Awake ()
		{
			// 保存初始位置和旋转
			positionTemp = this.transform.localPosition;
			rotationTemp = this.transform.localRotation;
		}

		void FixedUpdate ()
		{
			float swayMagX = Mathf.Cos (Time.time * parentMagnitude) * Time.fixedDeltaTime;
			float swayMagZ = Mathf.Sin (Time.time * parentMagnitude) * Time.fixedDeltaTime;
		
			if (transform.parent) {
				// 寻找旧位置和新位置之间的差异，然后移动
				parentPositionMagnitude = Vector3.ClampMagnitude (transform.parent.position - parentPositionTemp, 1);
				parentRotationMagnitude = transform.parent.localRotation.eulerAngles - parentRotationTemp.eulerAngles;
			
				if (transform.parent.GetComponent<Rigidbody>())
					parentMagnitude = transform.parent.GetComponent<Rigidbody>().velocity.magnitude * 0.05f;
	
		
				this.transform.localPosition = positionTemp + (SwaySensitivity * new Vector3 (swayMagX, parentPositionMagnitude.y, swayMagZ)) * Time.fixedDeltaTime;
				this.transform.localRotation = Quaternion.Lerp (this.transform.localRotation, Quaternion.Euler ((rotationTemp.eulerAngles.x + parentRotationMagnitude.x + swayMagX), (rotationTemp.eulerAngles.y + parentRotationMagnitude.y), (rotationTemp.eulerAngles.z + parentRotationMagnitude.z + swayMagZ)), Time.fixedDeltaTime * SwaySensitivity);

				parentPositionTemp = transform.parent.position;
				parentRotationTemp = transform.parent.localRotation;
			}
		}

	}
}