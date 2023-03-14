using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Interactible
{
	#region Fields
	[SerializeField]
	float power = 10f;
	#endregion

	#region Methods
	public override void Interact(PlayerInteract player)
	{
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
		rigidbody.AddForceAtPosition((player.transform.forward) * power + Vector3.up * power / 2, player.transform.position, ForceMode.Impulse);
	}
	#endregion
}