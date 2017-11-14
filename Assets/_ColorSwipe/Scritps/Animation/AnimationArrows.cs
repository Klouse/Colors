
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
	/// Class in charge to animate the 4 arrows when the game start.
	/// </summary>
	public class AnimationArrows : MonoBehaviour
	{
		RectTransform[] listRect;
		float width = 264.4f;

		float time = 0.5f;

		void Awake()
		{
			listRect = gameObject.GetComponentsInChildren<RectTransform>();

			foreach(var r in listRect)
			{
				SetSizeDeltaXTo0(r);
			}
		}


		void OnEnable()
		{


			foreach(var r in listRect)
			{
				SetSizeDeltaXTo0(r);
				DOAnimation(r);
			}
		}

		void OnDisable()
		{
			foreach(var r in listRect)
			{
				SetSizeDeltaXTo0(r);
			}
		}
		/// <summary>
		/// The animation of the arrows.
		/// </summary>
		void DOAnimation(RectTransform r)
		{

			DOVirtual.Float(0,width,time,(float f) => {
				var sizeD = r.sizeDelta;
				sizeD.x = f;
				r.sizeDelta = sizeD;
			})
				.SetEase(Ease.OutBack);
		}
		/// <summary>
		/// Initialize the size delta X = 0 at start.
		/// </summary>
		void SetSizeDeltaXTo0(RectTransform r)
		{
			var sizeD = r.sizeDelta;
			sizeD.x = 0;
			r.sizeDelta = sizeD;
		}
	}
