
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



#pragma warning disable 0618
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

	/// <summary>
	/// Class in charge to all the game logic.
	///
	/// It's a singleton.
	///
	/// Attached to the "GameManager + SpawnManager" GameObject in the scene hierarchy.
	/// </summary>
	public class GameManager : MonoBehaviour
	{

		static GameManager _instance;

		static public bool isActive {
			get {
				return _instance != null;
			}
		}
		/// <summary>
		/// Instance of GameManager
		/// </summary>
		static public GameManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = UnityEngine.Object.FindObjectOfType(typeof(GameManager)) as GameManager;
				}
				return _instance;
			}
		}

		/// <summary>
		/// Event who notified all listeners when a point is added
		/// </summary>
		public delegate void AddPoint();
		public static event AddPoint OnAddPoint;
		/// <summary>
		/// Event who notified all listeners when the player failed
		/// </summary>
		public delegate void GameOverEvent();
		public static event GameOverEvent OnGameOver;
		/// <summary>
		/// Event who notified all listeners when the game start
		/// </summary>
		public delegate void GameStartEvent();
		public static event GameStartEvent OnGameStart;
		/// <summary>
		/// Reference to point on the top of the screen during the game
		/// </summary>
		public Text pointText;
		/// <summary>
		/// Reference to the game over menu
		/// </summary>
		public RectTransform menuGameOver;
		/// <summary>
		/// Reference to the image to fill out (bonus)
		/// </summary>
		public Image[] imageCountdown;
		/// <summary>
		/// Reference to the image to fill out (bonus)
		/// </summary>
		public GameObject arrowParent;
		/// <summary>
		/// Reference to the spawn points for the shapes, ie. the dots around the center
		/// </summary>
		public RectTransform[] spawnPoints;
		/// <summary>
		/// Reference to the parent of the spawn points for the shapes, ie. the dots around the center
		/// </summary>
		public RectTransform spawnPointsParent;
		/// <summary>
		/// Current score
		/// </summary>
		int score = 0;
		/// <summary>
		/// The bonus or malus the player earn when he gets a TURTLE BONUS or a RABBIT MALUS.
		/// TURTLE  // slow down the game
		/// RABBIT // spped up the game
		/// </summary>
		[SerializeField] private int speedBonusMalus;
		/// <summary>
		/// Default ratio = 1. When the player get a x2 BONUS, ratio = 2, ie. each time the player have a good move, he earns 2 points instead of 1
		/// </summary>
		int ratioScoreBonus = 1;
		/// <summary>
		/// Getter of ratioScoreBonus
		/// </summary>
		public int GetRatioScoreBonus()
		{
			return ratioScoreBonus;
		}
		/// <summary>
		/// Setter of ratioScoreBonus
		/// </summary>
		public void SetRatioScoreBonus(int i)
		{
			ratioScoreBonus = i;
		}
		/// <summary>
		/// Local reference of the speed
		/// </summary>
		[SerializeField] private float m_speed = 6;
		/// <summary>
		/// Getter of the speed. Change the formula inside to custmize it
		/// </summary>
		public float speed
		{
			get
			{
				float scorePow = Mathf.Pow(Mathf.Max(score - speedBonusMalus,0),0.5f);

				m_speed = 6 - scorePow;
				//Mathf.Min(10,10 + speedBonusMalus - scorePow);

				if(m_speed < 1)
				{
					m_speed = 1;
				}
				if(m_speed >= 10)
				{
					m_speed = 10;
					//	speedBonusMalus = 0;
				}

				return m_speed;
			}
		}
		/// <summary>
		/// How much bonus the player earn when he gets a TURTLE BONUS. Default = -5
		/// </summary>
		public void SpeedBonus()
		{
			speedBonusMalus += 3;
		}
		/// <summary>
		/// How much malus the player gets when he gets a RABBIT MALUS. Default = -5
		/// </summary>
		public void SpeedMalus()
		{
			speedBonusMalus -= 1;
		}
		/// <summary>
		/// Awake function. We cean some garbage, target frame rate to 60 FPS, time scale to 1 and instantiate the AdsManager instance if it's not already done previously
		/// </summary>
		void Awake()
		{
			GC.Collect();

			Physics2D.gravity = Vector2.zero;

			Application.targetFrameRate = 60;

			Time.timeScale = 1f;

			if(Time.realtimeSinceStartup < 5)
			{
				DOTween.Init();
			}
		}
		/// <summary>
		/// Start function. Activates game.
		/// </summary>
		void Start()
		{
			Scene scene = SceneManager.GetActiveScene();

			if(scene.name == "Game")
			{
				this.score = 50;

				this.ratioScoreBonus = 1;

				//menuGameOver.gameObject.SetActive(false);
				//pointText.gameObject.SetActive(false);
				//arrowParent.SetActive(false);

				//DisplayPoints(false);DOTween.KillAll();

				Application.targetFrameRate = 60;

				if(OnGameStart != null)
					OnGameStart();

		    var anim = gameObject.GetComponentsInChildren<AnimHierarchy>(false);
				float time = 0;



	      foreach(var a in anim)
	      {
	        time = Mathf.Max(time,a.DoAnimOut());
	      }

				var canvasGroup = gameObject.GetComponentsInChildren<CanvasGroup>();

				DOVirtual.DelayedCall(time,() => {
					pointText.gameObject.SetActive(true);

					DOVirtual.DelayedCall(1f, () => {

						SpawnManager.instance.SpawnTargets();

					});

					arrowParent.SetActive(true);

					DisplayPoints(true);

					DOVirtual.Float(0,1,1,(float f) => {
						DoAlphaCanvasGroup(canvasGroup,f);
					});

					score = 0;

					SetScoreText();
				});
			}

		}
		/// <summary>
		/// Method called when the player click on restart on the GameOver screen. It reload the scene.
		/// </summary>
		public void OnClickedSubmit()
		{
			DOTween.KillAll();

			float time = menuGameOver.GetComponent<AnimHierarchy>().DoAnimOut();

			DOVirtual.DelayedCall(time, () => {
				//Loads Menu
				//This will call load skillz
				//SceneManager.LoadSceneAsync("Menu");
				if (SkillzCrossPlatform.IsMatchInProgress ())
				{
				    SkillzCrossPlatform.ReportFinalScore(score);
				}
			});
		}
		/// <summary>
		/// Method to animate the alpha of multiple canvas group at the same time.
		/// </summary>
		void DoAlphaCanvasGroup(CanvasGroup[] cg, float a)
		{
			foreach(var c in cg)
			{
				c.alpha = a;
			}
		}
		/// <summary>
		/// Method called when the player earn 1 point.
		/// </summary>
		public void Add1Point()
		{

			score = score + GetRatioScoreBonus();

			SetScoreText();



			if (OnAddPoint != null)
				OnAddPoint();

			StartCoroutine(SpawnNow());
		}
		/// <summary>
		/// Spawn new targets and player after last targets and player are despawned
		/// </summary>
		IEnumerator SpawnNow()
		{
			yield return 0;

			while(FindObjectsOfType<ShapeItem>().Length != 0)
			{

				yield return 0;
			}

			SpawnManager.instance.SpawnTargets();
		}
		/// <summary>
		/// Set the score UI Text
		/// </summary>
		void SetScoreText()
		{
			pointText.text = score.ToString();
			if (SkillzCrossPlatform.IsMatchInProgress ())
			{
			    SkillzCrossPlatform.UpdatePlayersCurrentScore(score);
			}

		}
		/// <summary>
		/// Game Over logic called when the player touch a shape with a different color, and called the method ShowAdsGameOver from AdsManager (the logic - display or not - is inside the AdsManager)
		/// </summary>
		public void GameOver()
		{
			Time.timeScale = 1;

			if(OnGameOver != null)
				OnGameOver();

			PlayerPrefs.SetInt("LAST_SCORE",score);

			int bestScore = PlayerPrefs.GetInt("BEST_SCORE");

			DisplayPoints(false);

			if(score > bestScore)
			{
				PlayerPrefs.SetInt("BEST_SCORE",score);
			}

			ReportScoreToLeaderboard(bestScore);

			StartCoroutine(_GameOver());
		}
		/// <summary>
		/// If using Very Simple Leaderboard by App Advisory, report the score : http://u3d.as/qxf
		/// </summary>
		void ReportScoreToLeaderboard(int p)
		{
			#if APPADVISORY_LEADERBOARD
			LeaderboardManager.ReportScore(p);
			#else
			print("Get very simple leaderboard to use it : http://u3d.as/qxf");
			#endif
		}
		/// <summary>
		/// Wait to display menu Game Over : we wait for all shapes are despaned
		/// </summary>
		IEnumerator _GameOver()
		{
			while(FindObjectsOfType<ShapeItem>().Length != 0)
			{

				yield return 0;
			}

			pointText.rectTransform.SetParent(menuGameOver,false);

			pointText.text = "GAME OVER!";

			menuGameOver.gameObject.SetActive(true);
			arrowParent.SetActive(false);
		}
		/// <summary>
		/// Activate or desactivate the points from where the shapes appear and move
		/// </summary>
		void DisplayPoints(bool display)
		{
			spawnPointsParent.gameObject.SetActive(display);
		}

		public void ShowAds()
		{
			int count = PlayerPrefs.GetInt("GAMEOVER_COUNT",0);
			count++;
			PlayerPrefs.SetInt("GAMEOVER_COUNT",count);
			PlayerPrefs.Save();

			#if APPADVISORY_ADS
			if(count > numberOfPlayToShowInterstitial)
			{
			#if UNITY_EDITOR
			print("count = " + count + " > numberOfPlayToShowINterstitial = " + numberOfPlayToShowInterstitial);
			#endif
			if(AdsManager.instance.IsReadyInterstitial())
			{
			#if UNITY_EDITOR
				print("AdsManager.instance.IsReadyInterstitial() == true ----> SO ====> set count = 0 AND show interstial");
			#endif
				PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
				AdsManager.instance.ShowInterstitial();
			}
			else
			{
			#if UNITY_EDITOR
				print("AdsManager.instance.IsReadyInterstitial() == false");
			#endif
			}

		}
		else
		{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
		}
		PlayerPrefs.Save();
			#else
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
		PlayerPrefs.Save();
			#endif

	}
}
