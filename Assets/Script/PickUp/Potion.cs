using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : PickUp
{
	[SerializeField]
	GameObject potionObject;

	private void Awake()
	{
		base.Awake();
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
	}

	public override void Interact(PlayerInteract player)
	{
		base.Interact(player);
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
	}

	public override void Use(PlayerInteract player)
	{
		Transform parentTrans = player.transform;
		PotionObject potionObj = Instantiate(potionObject, player.CarryingPoint.position + parentTrans.forward, Quaternion.identity).GetComponent<PotionObject>();
		potionObj.StartPos = potionObj.transform.position;
		potionObj.Direction = (parentTrans.forward + Vector3.up / 2).normalized;
		Destroy(gameObject);
		player.CurrentPickUp = null;
	}
}
