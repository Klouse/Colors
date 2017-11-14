
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/


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
	public enum ShapeType
	{
		player,
		noBonus,
		bonus_turtle,
		bonus_rabbit,
		bonus_mult2
	}
