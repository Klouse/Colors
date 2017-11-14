
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

	/// <summary>
	/// Class in charge to animate the item icon in the shape when the player get one.
	/// </summary>
	public class AnimationBonusIcon : MonoBehaviour
	{
		public float originalScale = 0.7f;
		public Image i;
		public Text t;

		void Awake()
		{
			i = GetComponent<Image>();
			t = GetComponent<Text>();
		}

		void OnEnable()
		{
			Reset();
		}

		void OnDisable()
		{
			Reset();
		}

		void Reset()
		{
			transform.DOKill();

			if(i != null)
				i.DOKill();

			if(t != null)
				t.DOKill();

			transform.localScale = Vector3.one * originalScale;

			if(i != null)
				i.color = Color.white;

			if(t != null)
				t.color = Color.white;
		}
		/// <summary>
		/// The animation of the icon.
		/// </summary>
		public void DoAnim(Color c)
		{
			gameObject.SetActive(true);
			transform.localScale = Vector3.one * originalScale;
			transform.DOScale(Vector3.one * originalScale * 5 , 0.5f).SetEase(Ease.OutBack,1,2);

			if(i != null)
				i.DOColor(c,0.3f);

			if(t != null)
				t.DOColor(c,0.3f);

		}

		//	void OnDisable()
		//	{
		//		transform.DOKill();
		//
		//		if(i != null)
		//			i.DOKill();
		//
		//		if(t != null)
		//			t.DOKill();
		//	}
	}
