
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

	/// <summary>
	/// Class attached the UI TEXT last score to display the best score at game over
	/// </summary>
	public class SetLastScore : MonoBehaviour
	{
		void OnEnable()
		{
			GetComponent<Text>().text = PlayerPrefs.GetInt("LAST_SCORE").ToString();
		}

	}
