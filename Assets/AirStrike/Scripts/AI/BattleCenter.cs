using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class BattleCenter : MonoBehaviour
	{
		// 战斗中心位置，AI将围绕这个位置飞行
		// 如果为真，将不允许AI飞行低于这个战斗中心位置
		public bool FixedFloor = true;

		void Start ()
		{
	
		}
	}
}