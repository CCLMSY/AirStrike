using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	public class EnemySpawner : MonoBehaviour
	// 敌人生成器。通过Objectman[]的随机索引自动重新生成敌人
	{
	
		public bool Enabled = true;
		public GameObject[] Objectman;
		// object to spawn
		public float timeSpawn = 3;
		public int enemyCount = 10;
		public int radius = 10;
		public string Tag = "Enemy";
		public string Type = "Enemy";
		private float timetemp = 0;
		private int indexSpawn;

		void Start ()
		{
			indexSpawn = Random.Range (0, Objectman.Length);
			timetemp = Time.time;
		}

		void Update ()
		{
		
			if (!Enabled)
				return;
		
		
			var gos = GameObject.FindGameObjectsWithTag (Tag);
			if (gos.Length < enemyCount && Time.time > timetemp + timeSpawn) {
				// 在Objectman[]的随机索引处生成敌人
				timetemp = Time.time;
				GameObject obj = (GameObject)GameObject.Instantiate (Objectman [indexSpawn], transform.position + new Vector3 (Random.Range (-radius, radius), 0, Random.Range (-radius, radius)), Quaternion.identity);
				obj.tag = Tag;
				indexSpawn = Random.Range (0, Objectman.Length);
			}
		}
	}
}