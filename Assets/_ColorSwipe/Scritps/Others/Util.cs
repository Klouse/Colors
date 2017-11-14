
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

	/// <summary>
	/// Utilities class, to compare color or shuffle List
	/// </summary>
	public static class Util
	{
		public static bool CompareColor(Color c1, Color c2)
		{
			float red = c1.r;
			float green = c1.g;
			float blue = c1.b;

			float redOther = c2.r;
			float greenOther = c2.g;
			float blueOther = c2.b;

			return (red == redOther) && (green == greenOther) && (blue == blueOther);
		}

		private static  System.Random rng = new System.Random();

		public static List<T> Shuffle<T>(List<T> list)
		{
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}

			return list;
		}

		public static bool CollideWithSameAASprite(this ShapeItem shapeItem, ShapeItem other)
		{
			//		bool isOk = CompareColor(other.aaSprite.color, shapeItem.aaSprite.color);

			bool isOk = shapeItem.aaSprite.num == other.aaSprite.num;

			return isOk;
		}

		public static Vector2 GetDirection(this Swipes direction)
		{
			switch(direction)
			{
			case Swipes.None :
				return Vector2.zero;
			case Swipes.Up :
				return Vector2.up;
			case Swipes.Down :
				return -Vector2.up;
			case Swipes.Left :
				return -Vector2.right;
			case Swipes.Right :
				return Vector2.right;
			case Swipes.TopLeft :
				return (Vector2.up + -Vector2.right);
			case Swipes.TopRight :
				return (Vector2.up + Vector2.right);
			case Swipes.BottomLeft :
				return (-Vector2.up + -Vector2.right);
			case Swipes.BottomRight :
				return (-Vector2.up + Vector2.right);

			}

			return Vector2.zero;
		}
	}
