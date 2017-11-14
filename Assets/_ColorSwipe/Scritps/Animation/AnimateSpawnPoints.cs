
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

	/// <summary>
	/// Class in charge to animate the 8 points from where the shapes are spawn during the game.
	/// </summary>
	public class AnimateSpawnPoints : MonoBehaviour
	{
		float time = 0.5f;

		void Awake()
		{
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector2.zero;
		}

		void OnEnable()
		{
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector2.zero;
			DOAnimation();
		}

		void OnDisable()
		{
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector2.zero;
		}
		/// <summary>
		/// The animation of the spawns point.
		/// </summary>
		void DOAnimation()
		{
			transform.DOScale(Vector2.one, 0.7f)
				.SetEase(Ease.OutBack);

			transform.DOLocalRotate(Vector3.forward * 360, time, RotateMode.FastBeyond360);
		}
	}
