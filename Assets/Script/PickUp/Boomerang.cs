using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : PickUp
{
	#region Fields
	[SerializeField]
	GameObject boomerangObject;
    #endregion

    #region Methods
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
		BoomerangObject boomObj = Instantiate(boomerangObject, player.CarryingPoint.position + parentTrans.forward, Quaternion.identity).GetComponent<BoomerangObject>();
		boomObj.StartPos = boomObj.transform.position;
		boomObj.Direction = parentTrans.forward;
		Destroy(gameObject);
		player.CurrentPickUp = null;
	}
	#endregion
}