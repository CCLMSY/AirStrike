using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class EnemyDead : FlightOnDead
	{
		// 得分
		public int ScoreAdd = 250;

		void Start ()
		{
			
		}
	
		// 如果击杀者是玩家，增加分数
		public override void OnDead ()
		{
			if (flight.LatestHit != null) {
				if (flight.LatestHit.gameObject.GetComponent<PlayerManager> ()) {
					AirStrikeGame.gameManager.AddScore (ScoreAdd);
				}
			}
			base.OnDead ();
		}
	}
}