using UnityEngine;
using System.Collections;

namespace AirStrikeKit
{
	[RequireComponent (typeof(Light))]

	public class FlashLight : MonoBehaviour
	{
	
		public float LightMult = 2;
		private float lightIntenTmp;

		void Awake ()
		{
			if (!this.GetComponent<Light>())
				return;
			lightIntenTmp = this.GetComponent<Light>().intensity;
		}

		void OnEnable ()
		{
			this.GetComponent<Light>().intensity = lightIntenTmp;
		}

		void Update ()
		{
			if (!this.GetComponent<Light>())
				return;
		
			this.GetComponent<Light>().intensity -= LightMult * Time.deltaTime;
		}
	}
}