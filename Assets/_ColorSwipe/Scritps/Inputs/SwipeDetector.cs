
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using System.Collections;

	public enum Swipes { None, Up, Down, Left, TopLeft, BottomLeft, Right, TopRight,  BottomRight};

	/// <summary>
	/// Class attached to the GameObject "SwipeDetector" in the scene hierarchy.
	/// In charge to handle swipe on the screen.
	/// </summary>
	///
	public class SwipeDetector : MonoBehaviour
	{
		public int force;

		public float minSwipeLength = 200f;
		Vector2 currentSwipe;

		private Vector2 fingerStart;
		private Vector2 fingerEnd;

		public static Swipes direction;

		/// <summary>
		/// Delegate triggered when swipe event is detected.
		/// </summary>
		public delegate void OnSwipe(Swipes direction);
		public static event OnSwipe OnSwipeEnd;
		public static void ForceOnSwipeEnd(Swipes direction)
		{
			direction = Swipes.None;

			if(OnSwipeEnd != null)
				OnSwipeEnd(direction);
		}

		void Awake()
		{
			var screenV = new Vector2(Screen.width,Screen.height);
			print("screen magnitude = " + screenV.magnitude);

			minSwipeLength = screenV.magnitude * 0.10f;
		}

		void Update ()
		{
			if(OnSwipeEnd == null)
			{
				fingerStart = fingerEnd;
				return;
			}

			var invokList =  OnSwipeEnd.GetInvocationList();

			var l = invokList.Length;

			if(l <= 0)
			{
				direction = Swipes.None;
				fingerStart = Vector2.zero;
				fingerEnd = Vector2.zero;



				return;
			}

			SwipeDetection();
		}

		/// <summary>
		/// Swipe detection logic
		/// </summary>
		public void SwipeDetection ()
		{
			if (Input.GetMouseButtonDown(0))
			{
				fingerStart = Input.mousePosition;
				fingerEnd  = Input.mousePosition;
			}


			if(Input.GetMouseButton(0))
			{
				fingerEnd = Input.mousePosition;

				currentSwipe = new Vector2 (fingerEnd.x - fingerStart.x, fingerEnd.y - fingerStart.y);

				// Make sure it was a legit swipe, not a tap
				if (currentSwipe.magnitude < minSwipeLength)
				{
					direction = Swipes.None;
					if(OnSwipeEnd != null)
						OnSwipeEnd(direction);
					return;
				}

				float angle = (Mathf.Atan2(currentSwipe.y, currentSwipe.x) / (Mathf.PI));

				if (angle>0.375f && angle<0.625f)
				{
					direction = Swipes.Up;
				}
				else if (angle<-0.375f && angle>-0.625f)
				{
					direction = Swipes.Down;

				}
				else if (angle<-0.875f || angle>0.875f)
				{
					direction = Swipes.Left;

				}
				else if (angle>-0.125f && angle<0.125f)
				{
					direction = Swipes.Right;

				}
				else if(angle>0.125f && angle<0.375f)
				{
					direction = Swipes.TopRight;

				}
				else if(angle>0.625f && angle<0.875f)
				{
					direction = Swipes.TopLeft;

				}
				else if(angle<-0.125f && angle>-0.375f)
				{
					direction = Swipes.BottomRight;

				}
				else if(angle<-0.625f && angle>-0.875f)
				{
					direction = Swipes.BottomLeft;
				}

				if(OnSwipeEnd != null)
					OnSwipeEnd(direction);
			}

			if(Input.GetMouseButtonUp(0)) {
				direction = Swipes.None;

				if(OnSwipeEnd != null)
					OnSwipeEnd(direction);
			}
		}
	}
