using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class Spawner : MonoBehaviour
	{
		private Transform Objectman = null;
		// 生成器
		private float timeSpawn = 0;
		private int timeSpawnMax = 0;
		private float enemyCount = 0;
		public int radian = 0;

		private void Start ()
		{
			if (GetComponent<Renderer> ())
				GetComponent<Renderer> ().enabled = false;

		}

		private void Update ()
		{
			if (Objectman == null)
				return;
			
			// 生成后找到所有的敌人
			GameObject[] gos = GameObject.FindGameObjectsWithTag (Objectman.tag);
			timeSpawn += 1;
			if (gos.Length < enemyCount) {
				int timespawnmax = timeSpawnMax;
				if (timespawnmax <= 0) {
					timespawnmax = 10;
				}
				if (timeSpawn >= timespawnmax) {
					GameObject enemyCreated = (GameObject)Instantiate (Objectman.gameObject,transform.position +new Vector3 (Random.Range (-radian, radian), 20, Random.Range (-radian, radian)),Quaternion.identity);

					enemyCreated.transform.localScale = new Vector3 (Random.Range (5, 20), enemyCreated.transform.localScale.x,
						enemyCreated.transform.localScale.x);

					timeSpawn = 0;

				}
			}

		}
	}
}
