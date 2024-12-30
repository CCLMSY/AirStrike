using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class PlayerDead : FlightOnDead
	{
		void Start ()
		{
		}
	
		// 玩家死亡时调用
		public override void OnDead ()
		{
			AirStrikeGame.gameManager.GameOver ();

			base.OnDead ();
		}
	}
}