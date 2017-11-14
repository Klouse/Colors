
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

	/// <summary>
	/// Class in charge to animate all the UI elements in childs.
	/// </summary>
	public class AnimHierarchy : MonoBehaviour
	{
		/// <summary>
		/// Delay before starting animation.
		/// </summary>
		public float delay = 0;
		/// <summary>
		/// True if the animation is horizontal.
		/// </summary>
		public bool isHorizontal = true;
		/// <summary>
		/// True if the aimation is vertical AND from top.
		/// </summary>
		public bool isVerticalFromTop = false;
		/// <summary>
		/// True if the aimation is vertical AND from bottom.
		/// </summary>
		public bool isVerticalBottom = false;
		/// <summary>
		/// True if the aimation is vertical (from bottom or from top).
		/// </summary>
		bool isVertical
		{
			get
			{
				return isVerticalFromTop || isVerticalBottom;
			}
		}

		void OnEnable()
		{
			DoAnimIn();

		}
		/// <summary>
		/// The animation IN of the UI elements.
		/// </summary>
		public void DoAnimIn()
		{
			int mult = 1;

			int i = 0;

			foreach(var b in gameObject.GetComponentsInChildren<Button>(true))
			{
				b.interactable = false;
			}


			foreach(Transform t in transform)
			{
				if(isHorizontal)
					AnimHorizontalIn(t,mult);
				else if(isVertical)
					AnimVerticalIn(t,i);

				i++;
				mult *= -1;
			}
		}
		/// <summary>
		/// The animation IN of the UI elements, horizontaly.
		/// </summary>
		void AnimHorizontalIn(Transform t, int mult)
		{
			var pos = t.GetComponent<RectTransform>().anchoredPosition;

			float xOrigin =0;

			pos.x = mult *  2 * Screen.width;

			t.GetComponent<RectTransform>().anchoredPosition = pos;
			t.GetComponent<RectTransform>().DOLocalMoveX(xOrigin, 1)
				.SetDelay(delay)
				.SetEase(Ease.OutBack,0.6f,1)
				.OnComplete(()=>{
					foreach(var b in gameObject.GetComponentsInChildren<Button>(true))
					{
						b.interactable = true;
					}
				});
		}
		/// <summary>
		/// The animation IN of the UI elements, verticaly.
		/// </summary>
		void AnimVerticalIn(Transform t, int i)
		{
			var pos = t.GetComponent<RectTransform>().anchoredPosition;

			float yOrigin = pos.y;

			int mult = -1;

			if(isVerticalBottom == false)
				mult = 1;

			pos.y = mult * Screen.height ;

			t.GetComponent<RectTransform>().anchoredPosition = pos;
			t.GetComponent<RectTransform>().DOLocalMoveY(yOrigin, 1)
				.SetDelay(i * delay)
				.SetEase(Ease.OutBack,0.6f,1)
				.OnComplete(()=>{
					foreach(var b in gameObject.GetComponentsInChildren<Button>(true))
					{
						b.interactable = true;
					}
				});
		}
		/// <summary>
		/// The animation OUT of the UI elements.
		/// </summary>
		public float DoAnimOut()
		{
			int mult = 1;

			int i = 0;

			foreach(Transform t in transform)
			{
				if(isHorizontal)
					AnimHorizontalOut(t,mult);
				else if(isVertical)
					AnimVerticalOut(t,i);

				i++;
				mult *= -1;
			}

			return 0.6f + transform.childCount*delay;
		}
		/// <summary>
		/// The animation OUT of the UI elements, horizontaly.
		/// </summary>
		void AnimHorizontalOut(Transform t, int mult)
		{

			foreach(var b in gameObject.GetComponentsInChildren<Button>(true))
			{
				b.interactable = false;
			}


			var pos = t.GetComponent<RectTransform>().anchoredPosition;

			float xOrigin = pos.x;


			t.GetComponent<RectTransform>().DOLocalMoveX(mult *  2 * Screen.width, 1)
				.SetDelay(delay)
				.SetEase(Ease.OutBack,0.6f,1)
				.OnComplete( () => {
					gameObject.SetActive(false);
					var p = t.GetComponent<RectTransform>().anchoredPosition;
					p.x = xOrigin;
					t.GetComponent<RectTransform>().anchoredPosition = pos;
				});

		}
		/// <summary>
		/// The animation OUT of the UI elements, verticaly.
		/// </summary>
		void AnimVerticalOut(Transform t, int i)
		{

			foreach(var b in gameObject.GetComponentsInChildren<Button>(true))
			{
				b.interactable = false;
			}


			var pos = t.GetComponent<RectTransform>().anchoredPosition;

			float yOrigin = pos.y;

			int mult = 1;

			if(isVerticalBottom)
				mult = -1;

			t.GetComponent<RectTransform>().DOLocalMoveY(mult * Screen.height, 1)
				.SetDelay(i * delay)
				.SetEase(Ease.OutBack,0.6f,1)
				.OnComplete( () => {
					gameObject.SetActive(false);
					var p = t.GetComponent<RectTransform>().anchoredPosition;
					p.y = yOrigin;
					t.GetComponent<RectTransform>().anchoredPosition = pos;
				});

		}
	}
