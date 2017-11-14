
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
using DG.Tweening;


	/// <summary>
	/// Class in charge to animate the coutdown for the bonus item, if needed.
	///
	/// Bonus type, ie ShapeType.
	///
	/// TURTLE  // slow down the game
	///
	/// RABBIT // spped up the game
	///
	/// POINT // multiple by 2 the score
	/// </summary>
	public class BonusItem : MonoBehaviour
	{
		/// <summary>
		/// Class in charge to the filled image.
		/// </summary>
		public Image imageCountdown;
		/// <summary>
		/// Referece to the bonus TURTLE  // slow down the game
		/// </summary>
		public GameObject bonus_turtle;
		/// <summary>
		/// Referece to the bonus RABBIT // speed up the game
		/// </summary>
		public GameObject bonus_rabbit;
		/// <summary>
		/// Referece to the bonus POINT // multiple by 2 the score
		/// </summary>
		public GameObject bonus_mult2; // multiplie les points par deux
		/// <summary>
		/// Bonus type, ie ShapeType.
		/// TURTLE  // slow down the game
		/// RABBIT // spped up the game
		/// POINT // multiple by 2 the score
		/// </summary>
		ShapeType bonus;
		/// <summary>
		/// Subscribe to the event GameManager.OnGameOver
		/// </summary>
		void OnEnable()
		{
			GameManager.OnGameOver += DestroyMe;
		}
		/// <summary>
		/// Unsubscribe to the event GameManager.OnGameOver
		/// </summary>
		void OnDisable()
		{
			GameManager.OnGameOver -= DestroyMe;
		}
		/// <summary>
		/// Desable all the bonus items
		/// </summary>
		void Reset()
		{
			bonus_turtle.SetActive(false);
			bonus_rabbit.SetActive(false);
			bonus_mult2.SetActive(false);
		}
		/// <summary>
		/// Create bonus ShapeType and activate the image
		/// </summary>
		void CreateBonus(ShapeType bonus)
		{
			this.bonus = bonus;

			bonus_turtle.SetActive(this.bonus == ShapeType.bonus_turtle);
			bonus_rabbit.SetActive(this.bonus == ShapeType.bonus_rabbit);
			bonus_mult2.SetActive(this.bonus == ShapeType.bonus_mult2);

			SetBonusActionAtCreation(bonus);
		}
		/// <summary>
		/// Create the shape with bonus type ShapeType
		/// </summary>
		public void Create(ShapeType bonus)
		{
			gameObject.SetActive(true);

			this.CreateBonus(bonus);

			imageCountdown.fillAmount = 1;

			GetComponent<RectTransform>().DOScale(Vector2.one,0.2f)
				.SetEase(Ease.OutBack)
				.SetUpdate(true)
				.OnComplete(() => {
					float fillAmount = 1f;
					imageCountdown.fillAmount = fillAmount;
					DOVirtual.Float(1,0,5, (float f) => {
						imageCountdown.fillAmount = f;
					})
						.OnComplete(DestroyMe)
						.SetUpdate(true);
				});

		}
		/// <summary>
		/// Desactivate the shape and keep it in his pool
		/// </summary>
		void DestroyMe()
		{
			if(gameObject.activeInHierarchy)
			{
				if(bonus == ShapeType.bonus_mult2)
					GameManager.instance.SetRatioScoreBonus(1);

				gameObject.SetActive(false);
			}
		}
		/// <summary>
		/// Bonus action
		/// Bonus type, ie ShapeType.
		/// TURTLE  // slow down the game
		/// RABBIT // spped up the game
		/// POINT // multiple by 2 the score
		/// </summary>
		void SetBonusActionAtCreation(ShapeType bonus)
		{
			if(bonus == ShapeType.bonus_turtle)
			{
				float newTimeScale = Time.timeScale*0.5f;

				Time.timeScale = newTimeScale;

				return;
			}


			if(bonus == ShapeType.bonus_rabbit)
			{
				float newTimeScale = Time.timeScale*2;

				Time.timeScale = newTimeScale;

				return;
			}

			if(bonus == ShapeType.bonus_mult2)
			{

				GameManager.instance.SetRatioScoreBonus(2);

				return;
			}
		}
	}
