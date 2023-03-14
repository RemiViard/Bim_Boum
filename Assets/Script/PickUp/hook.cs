using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hook : PickUp
{
	#region Fields
	PlayerInteract player = null;
	
	[SerializeField, Min(0.1f)]
	float time;

	[SerializeField]
	GameObject ropePop;
	#endregion

	#region Methods
	public override void Use(PlayerInteract player)
	{
		Debug.Log("Here");
		
		if(Physics.Raycast(player.CarryingPoint.position, player.transform.forward, out RaycastHit hit))
		{
			inUse = true;
			Rigidbody rb;
			Vector3 direction;
			Vector3 finalPos;
			
			if (hit.collider.CompareTag("Unhookable"))
			{
				return;
				rb = player.GetComponent<Rigidbody>();
				direction = player.transform.forward;
				finalPos = hit.point;
				
				this.player = player;
			}
			else
			{
				rb = hit.rigidbody;
				direction = -player.transform.forward;
				finalPos = player.CarryingPoint.position;
				
				PlayerInteract interact = hit.collider.GetComponent<PlayerInteract>();
				if(interact != null)
					interact.GetComponent<PlayerMovement>().enabled = false;
				this.player = interact;
			}

			StartCoroutine(Hook(rb, direction, finalPos));
			player.CurrentPickUp = null;
		}
	}

	IEnumerator Hook(Rigidbody rb, Vector3 direction, Vector3 finalPosition)
	{
		Vector2 finalPos2D = new Vector2(finalPosition.x, finalPosition.z);
		float dist = (new Vector2(rb.transform.position.x, rb.transform.position.z) - finalPos2D).magnitude;
		yield return null;

		while ((new Vector2(rb.transform.position.x, rb.transform.position.z) - finalPos2D).sqrMagnitude > 0.2f)
		{
			rb.transform.position += (direction * dist) * Time.deltaTime / time;
			rb.velocity = Vector3.zero;
			yield return null;
		}

		if(!rb.CompareTag("Player"))
		{
			rb.useGravity = true;
			rb.isKinematic = false;
		}

		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if(player != null)
		{
			player.GetComponent<PlayerMovement>().enabled = true;
		}
	}
	#endregion
}