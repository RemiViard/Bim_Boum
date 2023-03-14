using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickUp : Interactible
{
	#region Fields
	protected bool inUse = false;
	public bool InUse
	{
		get => inUse;
	}
	#endregion

	#region Methods
	public override void Interact(PlayerInteract player)
	{
		if(!player.IsCarryingSomething)
		{
			player.CurrentPickUp = this;
			transform.localPosition = Vector3.zero;
			this.transform.parent = player.CarryingPoint;
			transform.localPosition = Vector3.zero;
			player.UnregisterInteract(this);
			foreach (Collider collider in GetComponentsInChildren<Collider>())
			{
				collider.enabled = false;
			}
		}
	}
	public virtual void Use(PlayerInteract player) { }

	public override void Unregister(PlayerInteract player)
	{
		Debug.Log("Here");
		base.Unregister(player);
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
		transform.parent = null;
		foreach (Collider collider in GetComponentsInChildren<Collider>())
		{
			collider.enabled = true;
		}
	}
	#endregion
}