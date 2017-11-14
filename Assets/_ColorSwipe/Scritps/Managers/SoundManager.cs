
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

	/// <summary>
	/// Class in charge to all the sounds in the game.
	///
	/// It's a singleton.
	///
	/// Attached to the "SoundManager" GameObject in the scene hierarchy.
	/// </summary>
	public class SoundManager : MonoBehaviour
	{
		static SoundManager _instance;

		static public bool isActive {
			get {
				return _instance != null;
			}
		}
		/// <summary>
		/// Instance of SoundManager
		/// </summary>
		static public SoundManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Object.FindObjectOfType(typeof(SoundManager)) as SoundManager;
				}
				return _instance;
			}
		}
		/// <summsary>
		/// Reference to the audio source for the music
		/// </summary>
		public AudioSource m_music;
		/// <summary>
		/// Reference to the audio source for the fx
		/// </summary>
		public AudioSource m_fx;
		/// <summary>
		/// Reference to the clip plays when shapes are created
		/// </summary>
		public AudioClip fxCreate;
		/// <summary>
		/// Reference to the clip plays when there is a collision with two shapes with different colors, ie when game is over
		/// </summary>
		public AudioClip fxImpact;
		/// <summary>
		/// Reference to the clip plays when player gets 1 point
		/// </summary>
		public AudioClip fxPoint;
		/// <summary>
		/// Reference to the clip plays when player swipes
		/// </summary>
		public AudioClip fxSwipe;
		/// <summary>
		/// Reference to the music clip
		/// </summary>
		public AudioClip music;
		/// <summary>
		/// Subscribe to the event
		/// GameManager.OnAddPoint
		/// GameManager.OnGameOver
		/// GameManager.OnGameStart
		/// </summary>
		void OnEnable()
		{
			GameManager.OnAddPoint += PlayFXPoint;
			GameManager.OnGameOver += PlayFXImpact;
			GameManager.OnGameOver += StopMusic;
			GameManager.OnGameStart += PlayMusic;
		}
		/// <summary>
		/// Unsubscribe to the event
		/// GameManager.OnAddPoint
		/// GameManager.OnGameOver
		/// GameManager.OnGameStart
		/// </summary>
		void OnDisable()
		{
			GameManager.OnAddPoint -= PlayFXPoint;
			GameManager.OnGameOver -= PlayFXImpact;
			GameManager.OnGameOver -= StopMusic;
			GameManager.OnGameStart -= PlayMusic;
		}
		/// <summary>
		/// Play the music when OnGameStart is triggered
		/// </summary>
		void PlayMusic()
		{
			m_music.clip = music;
			m_music.loop = true;
			m_music.Play();
			DOVirtual.Float(0,1,1, (float f) => {
				m_music.volume = f;
			});
		}
		/// <summary>
		/// Stop the music
		/// </summary>
		void StopMusic()
		{
			DOVirtual.Float(1,0,3, (float f) => {
				m_music.volume = f;
			})
				.OnComplete(() => {
					m_music.Stop();
					m_music.clip = null;
					m_music.loop = false;
				});
		}
		/// <summary>
		/// Play FX for shapes creation
		/// </summary>
		public void PlayFXCreate()
		{
			m_fx.PlayOneShot(fxCreate);
		}
		/// <summary>
		/// Play FX for impact, when OnGameOver is triggered
		/// </summary>
		void PlayFXImpact()
		{
			m_fx.PlayOneShot(fxImpact);
		}
		/// <summary>
		/// Play FX for points, when OnAddPoint is triggered
		/// </summary>
		void PlayFXPoint()
		{
			m_fx.PlayOneShot(fxPoint);
		}
		/// <summary>
		/// Play FX when player swipe
		/// </summary>
		public void PlayFXSwipe()
		{
			m_fx.PlayOneShot(fxSwipe);
		}

		public void MuteAllMusic()
		{
			m_music.volume = 0;
			m_fx.volume = 0;
		}

		public void UnmuteAllMusic()
		{
			m_music.volume = 1;
			m_fx.volume = 1;
		}
	}
