
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


	/// <summary>
	/// Class attached the prefab ScoreItem. This is the score + comment prompt when the player earn points and bonus
	/// </summary>
	public class ScoreItem : MonoBehaviour
	{
		/// <summary>
		/// The score text
		/// </summary>
		[SerializeField] private Text myText;
		/// <summary>
		/// The score multiplicator text (when the player have a "x2" bonus)
		/// </summary>
		[SerializeField] private Text scoreMulti;
		/// <summary>
		/// Reference to his RectTransform
		/// </summary>
		[SerializeField] private RectTransform _rectTransform;

		void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}
		/// <summary>
		/// Set the Z position
		/// </summary>
		public void OnEnable()
		{
			SetPosZ();
		}
		public void SetParent(Transform t)
		{
			_rectTransform.SetParent(t,false);
		}
		/// <summary>
		/// Do the animation (move up + fade)
		/// </summary>
		public void DoAnim(Color c, string text)
		{
			DoAnim(c,text,GameManager.instance.GetRatioScoreBonus() == 2);
			scoreMulti.color = myText.color;
		}
		/// <summary>
		/// Do the animation (move up + fade)
		/// </summary>
		public void DoAnim(Color c, string text, bool desactivateScoreMulti)
		{
			SetStart();

			myText.text = text;

			myText.color = c;

			scoreMulti.color = myText.color;

			GetComponent<RectTransform>().DOLocalMoveY(GetComponent<RectTransform>().localPosition.y + 500,2);
			scoreMulti.gameObject.SetActive(desactivateScoreMulti);

			myText.DOFade(0, 2)
				.OnUpdate(() => {
					SetPosZ();
				})
				.OnComplete(()=>{
					gameObject.SetActive(false);
				});
		}
		/// <summary>
		/// Set the Z position
		/// </summary>
		void SetPosZ()
		{
			var pos = GetComponent<RectTransform>().localPosition;
			pos.z = 0;
			GetComponent<RectTransform>().localPosition = pos;
		}
		/// <summary>
		/// Set the alpha at start to 1, and initialize UI elements
		/// </summary>
		void SetStart()
		{
			transform.localScale = Vector2.one * 1f;
			myText.text = "+1";
			gameObject.SetActive(true);
			scoreMulti.gameObject.SetActive(false);
			Color c = Color.black;
			c.a = 1;
			myText.color = c;
			scoreMulti.color = c;
		}
	}
