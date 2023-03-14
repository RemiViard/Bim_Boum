using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cudgel : PickUp
{
	#region Fields
	#endregion

	#region Methods
	public override void Use(PlayerInteract player)
	{
		if (player.Cudgel())
        {
			player.CurrentPickUp = null;
			Destroy(gameObject);
		}
	}
	#endregion
}
