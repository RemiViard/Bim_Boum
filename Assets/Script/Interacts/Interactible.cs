using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Interactible : MonoBehaviour
{
	#region Fields
	protected Rigidbody rigidbody;
	protected Collider collider;
	#endregion

	#region Methods
	protected void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		collider.isTrigger = true;
	}

	public virtual void Interact(PlayerInteract player) { }
	public virtual void Register(PlayerInteract player) { }
	public virtual void Unregister(PlayerInteract player) { }

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out PlayerInteract player))
		{
			player.RegisterInteract(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.TryGetComponent(out PlayerInteract player))
		{
			player.UnregisterInteract(this);
		}
	}
	#endregion
}