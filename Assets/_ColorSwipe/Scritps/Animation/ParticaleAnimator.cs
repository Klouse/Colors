
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using System.Collections;


	public class ParticaleAnimator : MonoBehaviour
	{
		private void Awake()
		{
			particle = GetComponent<ParticleEmitter>();
		}

		// Use this for initialization
		void Start ()
		{
			lastTime = Time.realtimeSinceStartup;
		}

		// Update is called once per frame
		void Update ()
		{

			float deltaTime = Time.realtimeSinceStartup - (float)lastTime;

			particle.Simulate(deltaTime) ;//, true, false); //last must be false!!

			lastTime = Time.realtimeSinceStartup;
		}

		private double lastTime;
		private ParticleEmitter particle;

	}
