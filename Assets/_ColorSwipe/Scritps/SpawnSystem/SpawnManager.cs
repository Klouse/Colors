
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
	/// Class in charge to spawn all GameObject in the game.
	///
	/// It's a singleton.
	///
	/// Attached to the "GameManager + SpawnManager" GameObject in the scene hierarchy.
	/// </summary>
	public class SpawnManager : MonoBehaviour
	{
		static SpawnManager _instance;

		static public bool isActive {
			get {
				return _instance != null;
			}
		}
		/// <summary>
		/// Instance of GameManager
		/// </summary>
		static public SpawnManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Object.FindObjectOfType(typeof(SpawnManager)) as SpawnManager;
				}
				return _instance;
			}
		}
		/// <summary>
		/// List of shape color + sprite (cf. AASprite class). More colors = more difficult
		/// </summary>
		public AASprite[] aaSprites;

		public RectTransform shapeItemPrefab;

		public RectTransform SquareCenter;

		public RectTransform bonusItemPrefab;

		public RectTransform BonusItemsGrid;

		public RectTransform scoreItemPrefab;


		/// <summary>
		/// Store all the shapeItems inside
		/// </summary>
		public List<ShapeItem> shapeItems = new List<ShapeItem>();
		/// <summary>
		/// Store all the BonusItem inside
		/// </summary>
		List<BonusItem> bonusItems = new List<BonusItem>();
		/// <summary>
		/// Store all the ScoreItem inside
		/// </summary>
		List<ScoreItem> scoreItems = new List<ScoreItem>();



		void Awake()
		{
			for(int i = 0; i < aaSprites.Length; i++)
			{
				aaSprites[i].num = i;
			}
		}

		/// <summary>
		/// Get a random color from the list colors
		/// </summary>
		AASprite GetRandomAASprite()
		{
			int count = this.aaSprites.Length;

			AASprite c = this.aaSprites[Random.Range(0,count)];

			return c;
		}
		/// <summary>
		/// Each time we have to create shapes, we call this method.
		/// </summary>
		public void SpawnTargets()
		{
			SoundManager.instance.PlayFXCreate();

			var listAASprites = new List<AASprite>();
			for(int i = 0; i <  GameManager.instance.spawnPoints.Length; i++)
			{
				listAASprites.Add(this.GetRandomAASprite());
			}

			List<ShapeItem> listShapeCreated = new List<ShapeItem>();

			for(int i = 0; i <  GameManager.instance.spawnPoints.Length; i++)
			{
				var rSpawnPoint = GameManager.instance.spawnPoints[i];

				ShapeItem si = GetShapeItems();

				var r = si._rectTransform;

				si.transform.position = rSpawnPoint.position;
				r.localScale = Vector2.zero;

				listShapeCreated.Add(si);
			}

			listShapeCreated = Util.Shuffle(listShapeCreated);

			AASprite aaSpritePlayer = listAASprites[Random.Range(0,listAASprites.Count)];

			SpawnPlayer(aaSpritePlayer);

			for(int i = 0; i < listShapeCreated.Count; i++)
			{
				var s = listShapeCreated[i];
				var b =  GetRandomBonus();
				var c = listAASprites[i];
				if(Util.CompareColor(s.aaSprite.color,aaSpritePlayer.color))
				{
					if(Random.Range(0,4) == 0)
					{
						b = ShapeType.bonus_turtle;
						c = aaSpritePlayer;
					}
				}
				s.Create(c,b);
				s._rectTransform.DOScale(Vector2.one,0.1f)
					.SetUpdate(true)
					.SetEase(Ease.OutBack)
					.OnComplete(() => {
						s.MoveItem(SquareCenter.position);
					});
			}

			//speed is less than 5: we will add more shapes with the same color as player
			if(GameManager.instance.speed < 5)
			{
				int numColorEqualPlayer = GetNumOfShapeWithColorEqualToPlayer(listShapeCreated, aaSpritePlayer);
				if(numColorEqualPlayer < 3)
				{
					listShapeCreated = Util.Shuffle(listShapeCreated);

					for(int i = 0; i < 3 - numColorEqualPlayer; i++)
					{
						if(Random.Range(0,4) == 0)
						{
							listShapeCreated[i].Create(aaSpritePlayer,ShapeType.bonus_turtle);
						}
						else
						{
							listShapeCreated[i].Create(aaSpritePlayer, GetRandomBonus());
						}

					}
				}
			}
			else
			{
				//speed is less than 2: we will add more shapes with the same color as player
				if(GameManager.instance.speed < 2)
				{
					int numColorEqualPlayer = GetNumOfShapeWithColorEqualToPlayer(listShapeCreated, aaSpritePlayer);

					if(numColorEqualPlayer < 5)
					{
						listShapeCreated = Util.Shuffle(listShapeCreated);

						for(int i = 0; i < 5 - numColorEqualPlayer; i++)
						{
							if(Random.Range(0,4) == 0)
							{
								listShapeCreated[i].Create(aaSpritePlayer,ShapeType.bonus_turtle);
							}
							else
							{
								listShapeCreated[i].Create(aaSpritePlayer, GetRandomBonus());
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Return the number of shapes with the same color as the player
		/// </summary>
		int GetNumOfShapeWithColorEqualToPlayer(List<ShapeItem> listShapeItems, AASprite aaSpritePlayer)
		{
			var lc = listShapeItems.FindAll(o => Util.CompareColor(o.aaSprite.color,aaSpritePlayer.color) == true);

			if(lc == null)
				return 0;

			return lc.Count;
		}
		/// <summary>
		/// Get Random Bonus.
		///
		/// TURTLE  // if it's the moving shape with slow down bonus
		///
		/// RABBIT // if it's the moving shape with speed up bonus
		///
		/// POINT // if it's the moving shape with multiple by 2 the score bonus
		/// </summary>
		ShapeType GetRandomBonus()
		{
			int max = 7;
			//SkillzCrossPlatform.Random.Range(minPos, maxPos)
			int rand = Random.Range(0,max);

			ShapeType bonus = ShapeType.noBonus;

			if(rand == 0)
			{
				bonus = ShapeType.bonus_turtle;
			}

			if(GameManager.instance.speed <= 2)
			{
				if(rand == 1)
				{
					bonus = ShapeType.bonus_turtle;
				}

			}
			if(rand == max - 2)
			{
				bonus = ShapeType.bonus_rabbit;
			}

			if(rand == max - 1)
			{
				bonus = ShapeType.bonus_mult2;
			}

			return bonus;
		}

		/// <summary>
		/// Spawn the shape in the center of the screen.
		/// </summary>
		public void SpawnPlayer(AASprite aaSpritePlayer)
		{
			RectTransform r = null;
			ShapeItem s = GetShapeItems();

			s.Create(aaSpritePlayer,ShapeType.player);

			r = s._rectTransform;
			r.localPosition = Vector3.zero;
			r.localScale = Vector2.zero;
			r.DOScale(Vector2.one,0.1f)
				.SetUpdate(true)
				.SetEase(Ease.OutBack)
				.OnComplete(() => {
				});
		}
		/// <summary>
		/// Get (and instantiate if no inactive ScoreItem is available in the list scoreItems) a new ScoreItem and add it to the spawn list scoreItems
		/// </summary>
		public ScoreItem GetScoreItems()
		{
			while(scoreItems.Count < 10)
			{
				RectTransform r = null;
				var v = Instantiate(scoreItemPrefab.gameObject) as GameObject;
				r = v.GetComponent<RectTransform>();
				r.SetParent(GetComponent<RectTransform>(),false);
				r.name += scoreItems.Count.ToString();
				r.localPosition = Vector3.zero;
				r.gameObject.SetActive(false);
				scoreItems.Add(r.GetComponent<ScoreItem>());
			}

			var s = scoreItems.Find(o => o.gameObject.activeInHierarchy == false);

			s.transform.DOKill();
			s.GetComponent<RectTransform>().DOKill();

			s.gameObject.SetActive(true);

			return s;
		}

		/// <summary>
		/// Get (and instantiate if no inactive ScoreItem is available in the list shapeItems) a new ShapeItem and add it to the spawn list shapeItems
		/// </summary>
		ShapeItem GetShapeItems()
		{
			while(shapeItems.Count < 20)
			{
				CreateShapeItem();
			}

			var s = shapeItems.Find(o => o.gameObject.activeInHierarchy == false);

			s.transform.DOKill();
			s.GetComponent<RectTransform>().DOKill();

			s.gameObject.SetActive(true);

			return s;
		}
		/// <summary>
		/// Instantiate a new ShapeItem and add it to the spawn list shapeItems
		/// </summary>
		ShapeItem CreateShapeItem()
		{
			RectTransform r = null;
			var v = Instantiate(shapeItemPrefab.gameObject) as GameObject;
			r = v.GetComponent<RectTransform>();
			r.SetParent(GetComponent<RectTransform>(),false);
			r.name += shapeItems.Count.ToString();
			r.localPosition = Vector3.zero;
			r.localScale = Vector2.zero;
			r.GetComponent<ShapeItem>().Create(aaSprites[0], ShapeType.player);
			r.gameObject.SetActive(false);
			shapeItems.Add(r.GetComponent<ShapeItem>());
			return r.GetComponent<ShapeItem>();
		}
		/// <summary>
		/// Spawn new BonusItem
		/// </summary>
		public void SpawnItemBonus(ShapeType bonus)
		{
			var b = GetItemBonus();

			b.Create(bonus);
		}
		/// <summary>
		/// Get (and instantiate if no inactive BonusItem is available in the list bonusItems) a new BonusItem and add it to the spawn list bonusItems
		/// </summary>
		BonusItem GetItemBonus()
		{
			while(bonusItems.Count < 10)
			{
				RectTransform r = null;
				var v = Instantiate(bonusItemPrefab.gameObject) as GameObject;
				r = v.GetComponent<RectTransform>();
				r.SetParent(GetComponent<RectTransform>(),false);
				r.name += bonusItems.Count.ToString();
				r.localPosition = Vector3.zero;
				r.localScale = Vector2.zero;
				r.SetParent(BonusItemsGrid,false);
				r.gameObject.SetActive(false);
				bonusItems.Add(r.GetComponent<BonusItem>());
			}

			var b = bonusItems.Find(o => o.gameObject.activeInHierarchy == false);
			b.transform.DOKill();
			b.GetComponent<RectTransform>().DOKill();
			b.gameObject.SetActive(true);
			return b;
		}
		/// <summary>
		/// Desactivate all active ShapeItem
		/// </summary>
		void DespawnAllShapeItems()
		{
			ShapeItem[] si = FindObjectsOfType<ShapeItem>();

			foreach(ShapeItem s in si)
			{
				s.gameObject.SetActive(false);
			}
		}
		/// <summary>
		/// Desactivate all active BonusItem
		/// </summary>
		void DespawnAllItemBonus()
		{
			foreach(Transform t in BonusItemsGrid)
			{
				t.gameObject.SetActive(false);
			}
		}
	}
