
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
	/// Class attached to the prefab "ShapeItem". The is the shape spawn during the game, and swipe by the player
	/// </summary>
	public class ShapeItem : MonoBehaviour
	{
		/// <summary>
		/// Reference to the image
		/// </summary>
		public Image DOT;
		/// <summary>
		/// Reference to the color of the shape
		/// </summary>
		public AASprite aaSprite;
		/// <summary>
		/// The type of shape
		///
		/// player // if it's the center shape, controlled by the player
		///
		/// noBonus // if it's the moving shape with no bonus
		///
		/// TURTLE  // if it's the moving shape with slow down bonus
		///
		/// RABBIT // if it's the moving shape with speed up bonus
		///
		/// POINT // if it's the moving shape with multiple by 2 the score bonus
		/// </summary>
		public ShapeType shapeType = ShapeType.noBonus;
		/// <summary>
		/// Return true if it's the center shape controlled by player, ie shapeType = ShapeType.player
		/// </summary>
		bool isPlayer
		{
			get
			{
				return this.shapeType == ShapeType.player;
			}
		}
		/// <summary>
		/// Reference to the GameObject bonus_turtle
		/// </summary>
		public GameObject bonus_turtle; // ralenti le jeu
		/// <summary>
		/// Reference to the GameObject bonus_rabbit
		/// </summary>
		public GameObject bonus_rabbit; // accelere le jeu
		/// <summary>
		/// Reference to the GameObject bonus_mult2
		/// </summary>
		public GameObject bonus_mult2; // multiplie les points par deux
		/// <summary>
		/// Reference to the Rigidbody2D
		/// </summary>
		public Rigidbody2D _rigidbody;
		/// <summary>
		/// Reference to the Collider2D
		/// </summary>
		public Collider2D _collider;
		/// <summary>
		/// Reference to the RectTransform
		/// </summary>
		public RectTransform _rectTransform;

		void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_collider = GetComponent<Collider2D>();
			_rectTransform = GetComponent<RectTransform>();
			shapeType = ShapeType.noBonus;
		}

		public void DOKILL()
		{
			transform.DOKill();
			_rectTransform.DOKill();

			SwipeDetector.ForceOnSwipeEnd(Swipes.None);

		}

		void OnEnable()
		{
			_rigidbody.velocity = Vector2.zero;
			_rigidbody.isKinematic = false;
			_collider.enabled = true;
			DOKILL();

			GameManager.OnAddPoint += OnAddPoint;
			GameManager.OnGameOver += OnGameOver;

			name = "target";
		}


		void OnDisable()
		{
			shapeType = ShapeType.noBonus;
			DOKILL();
			_rigidbody.velocity = Vector2.zero;
			SwipeDetector.OnSwipeEnd -= OnSwipe;
			GameManager.OnAddPoint -= OnAddPoint;
			GameManager.OnGameOver -= OnGameOver;
		}
		/// <summary>
		/// Remove all listener and despawn the shape if GameManager.OnAddPoint event is triggered from GameManager
		/// </summary>
		void OnAddPoint ()
		{
			GameManager.OnAddPoint -= OnAddPoint;
			GameManager.OnGameOver -= OnGameOver;
			AnimationDespawn();
		}
		/// <summary>
		/// Remove all listener and despawn the shape if GameManager.OnGameOver event is triggered from GameManager
		/// </summary>
		void OnGameOver()
		{
			GameManager.OnAddPoint -= OnAddPoint;
			GameManager.OnGameOver -= OnGameOver;
			ShakeAndDespawn();
		}
		/// <summary>
		/// Method called when SwipeDetector.OnSwipeEnd is triggered. Will move the center shape only in the direction of the player swipes
		/// </summary>
		void OnSwipe(Swipes sw)
		{
			transform.DOKill();

			if(!isPlayer)
			{
				_rigidbody.velocity = Vector2.zero;
			}

			if(!isPlayer || sw == Swipes.None)
				return;

			var rig = _rigidbody;

			if(rig == null || rig.velocity.x != 0 || rig.velocity.y != 0)
				return;

			SoundManager.instance.PlayFXSwipe();

			SwipeDetector.OnSwipeEnd -= OnSwipe;
			SwipeDetector.ForceOnSwipeEnd(Swipes.None);


			name = "player";
			GetComponent<Collider2D>().enabled = false;

			MovePlayer(sw.GetDirection());
		}
		/// <summary>
		/// Method call to move the center shape
		/// </summary>
		void MovePlayer(Vector2 normalizedDirection)
		{
			if(normalizedDirection.x == 0 && normalizedDirection.y == 0)
			{
				print("direction NONE => break!");
				return;
			}

			SwipeDetector.OnSwipeEnd -= OnSwipe;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, normalizedDirection);

			bool isOK = false;


			ShapeItem target = null;

			Vector2 hitPos = Vector2.zero;

			if (hit.collider != null)
			{
				target = hit.transform.GetComponent<ShapeItem>();
				isOK = this.CollideWithSameAASprite(target);
				hitPos = hit.point;
				DOKILL();
				target.DOKILL();
				target._collider.enabled = false;
				target._rigidbody.velocity = Vector2.zero;

				_collider.enabled = false;
				_rigidbody.velocity = Vector2.zero;

				transform.DOMove(hitPos,0.1f)
					.SetUpdate(true)
					.SetEase(Ease.Linear)
					.OnComplete(() =>{
						if(isOK)
						{
							var scoreItem = GetScoreItem(target.transform.position);

							scoreItem.DoAnim(this.aaSprite.color, "+1");

							target.AddBonusToGrid();

							GameManager.instance.Add1Point();
						}
						else
						{
							GameManager.instance.GameOver();
						}
					});
			}
		}
		/// <summary>
		/// Method call to move the shapes to the center
		/// </summary>
		public void MoveItem(Vector2 endPosition)
		{
			DOKILL();
			transform.DOMove(endPosition,GameManager.instance.speed).SetUpdate(false).SetEase(Ease.Linear);
		}
		/// <summary>
		/// Method call to create a new shape
		/// </summary>
		public void Create(AASprite aaSprite, ShapeType type)
		{
			SwipeDetector.OnSwipeEnd -= OnSwipe;

			this.shapeType = type;

			if(	this.shapeType != ShapeType.player)
				this.CreateBonus(type);

			this.aaSprite = aaSprite;

			DOT.color = aaSprite.color;

			DOT.sprite = aaSprite.sprite;

			_collider.enabled = true;

			if(	this.shapeType == ShapeType.player)
			{
				StartCoroutine(AddListenerWithDelay());
				SetLayerRecursively(gameObject,8);
			}
			else
			{
				SetLayerRecursively(gameObject,9);
			}
		}

		IEnumerator AddListenerWithDelay()
		{
			SwipeDetector.ForceOnSwipeEnd(Swipes.None);

			yield return new WaitForSeconds(0.1f);

			SwipeDetector.ForceOnSwipeEnd(Swipes.None);

			while(transform.localScale.x != 1)
			{
				SwipeDetector.ForceOnSwipeEnd(Swipes.None);
				yield return 0;
			}

			yield return 0;

			SwipeDetector.ForceOnSwipeEnd(Swipes.None);

			yield return 0;

			print("add listener");

			SwipeDetector.OnSwipeEnd += OnSwipe;
		}


		void CreateBonus(ShapeType bonus)
		{

			bonus_turtle.SetActive(bonus == ShapeType.bonus_turtle);
			bonus_rabbit.SetActive(bonus == ShapeType.bonus_rabbit);
			bonus_mult2.SetActive(bonus == ShapeType.bonus_mult2);
		}

		void SetLayerRecursively(GameObject obj, int newLayer)
		{
			if (null == obj)
			{
				return;
			}

			obj.layer = newLayer;

			foreach (Transform child in obj.transform)
			{
				if (null == child)
				{
					continue;
				}
				SetLayerRecursively(child.gameObject, newLayer);
			}
		}
		/// <summary>
		/// Collision listener. It will trigger game overif the shapes collider each other do not have the same color. IF the shapes have the
		/// same solor, we will add 1 point, spawn a ScoreItem and eventually a bonus
		/// </summary>
		void OnCollisionEnter2D(Collision2D col)
		{
			if(!isPlayer)
				return;



			GameManager.instance.GameOver();



			//		GameObject obj = col.gameObject;
			//		var pos = col.contacts[0].point;
			//
			//		var otherShapeItem = obj.GetComponent<ShapeItem>();
			//		DOKILL();
			//		otherShapeItem.DOKILL();
			//		otherShapeItem._collider.enabled = false;
			//		otherShapeItem._rigidbody.velocity = Vector2.zero;
			//
			//		_collider.enabled = false;
			//		_rigidbody.velocity = Vector2.zero;
			//
			//		if(gameObject.activeInHierarchy && obj.activeInHierarchy)
			//		{
			//			bool isOk = this.CollideWithSameColor(otherShapeItem);
			//
			//			if(isOk)
			//			{
			//				var scoreItem = GetScoreItem(pos);
			//				scoreItem.DoAnim(this.aaSprite.color, "+1");
			//
			//				otherShapeItem.AddBonusToGrid();
			//
			//				GameManager.instance.Add1Point();
			//			}
			//			else
			//			{
			//				GameManager.instance.GameOver();
			//			}
			//		}
		}
		/// <summary>
		/// Spawn a new ScoreItem
		/// </summary>
		ScoreItem GetScoreItem(Vector2 pos)
		{
			ScoreItem si = SpawnManager.instance.GetScoreItems();

			si.SetParent(transform.parent);

			si.transform.position = pos;
			si.gameObject.SetActive(true);

			return si;
		}
		/// <summary>
		/// Add bonus to the top left of the screen and launch the countdown
		/// </summary>
		void AddBonusToGrid()
		{
			if(this.bonus_turtle.activeInHierarchy)
			{
				bonus_turtle.gameObject.SetActive(true);
				bonus_turtle.GetComponent<AnimationBonusIcon>().DoAnim(this.aaSprite.color);

				CreateScoreItemWithCustomText("Slow down");
				GameManager.instance.SpeedBonus();
				return;
			}

			if(this.bonus_rabbit.activeInHierarchy)
			{
				bonus_rabbit.gameObject.SetActive(true);
				bonus_rabbit.GetComponent<AnimationBonusIcon>().DoAnim(this.aaSprite.color);

				CreateScoreItemWithCustomText("Speed up");
				GameManager.instance.SpeedMalus();
				return;
			}
			if(this.bonus_mult2.activeInHierarchy)
			{
				bonus_mult2.gameObject.SetActive(true);
				bonus_mult2.GetComponent<AnimationBonusIcon>().DoAnim(this.aaSprite.color);

				CreateScoreItemWithCustomText("Score bonus");
				SpawnManager.instance.SpawnItemBonus(ShapeType.bonus_mult2);
				return;
			}
		}
		/// <summary>
		/// To create a custom text ScoreItem
		/// </summary>
		void CreateScoreItemWithCustomText(string text)
		{
			DOVirtual.DelayedCall(0.2f, () => {
				var scoreItem = GetScoreItem(transform.position);
				scoreItem.DoAnim(this.aaSprite.color, text, false);
				scoreItem.transform.localScale = Vector2.one * 0.5f;
			});
		}
		/// <summary>
		/// Animation, and when finished: despawn
		/// </summary>
		void AnimationDespawn()
		{
			DOKILL();

			SwipeDetector.OnSwipeEnd -= OnSwipe;
			GameManager.OnAddPoint -= OnAddPoint;
			GameManager.OnGameOver -= OnGameOver;

			_collider.enabled = false;

			_rigidbody.velocity = Vector2.zero;

			_rectTransform.DOScale(Vector2.one*1.5f,0.15f)
				.SetUpdate(true)
				.OnComplete(() => {
					_rectTransform.DOScale(Vector2.zero,0.15f)
						.SetUpdate(true)
						.OnComplete(() => {
							_rectTransform.gameObject.SetActive(false);
						});
				});
		}
		/// <summary>
		/// Animation, and when finished: despawn
		/// </summary>
		void ShakeAndDespawn()
		{
			DOKILL();


			SwipeDetector.OnSwipeEnd -= OnSwipe;
			GameManager.OnAddPoint -= OnAddPoint;
			GameManager.OnGameOver -= OnGameOver;

			_rectTransform.DOScale(Vector2.one*2f,0.5f)
				.SetUpdate(true)
				.OnComplete(() => {
					_rectTransform.DOScale(Vector2.one*0f,0.5f)
						.SetUpdate(true)
						.OnComplete(() => {
							_rectTransform.gameObject.SetActive(false);
						});
				});
		}


	}
