using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BoomerangObject : MonoBehaviour
{
	#region Fields
	[SerializeField]
	GameObject boomerangPickUp;

	[SerializeField, Min(0.1f)]
	float time = 1f;

	[SerializeField, Min(0.2f)]
	float range = 2f;

	Vector3 startPos;
	public Vector3 StartPos
	{
		get => startPos;
		set => startPos = value;
	}

	Vector3 direction = Vector3.forward;
	public Vector3 Direction
	{
		get => direction;
		set => direction = value;
	}

	float clock = 0;

	[SerializeField]
	Transform carryingPoint;

	bool hasBomb = false;
	#endregion

	#region Methods
	private void Update()
	{
		if(clock >= time)
		{
			Destroy(gameObject);
			return;
		}
		float factor = Mathf.Sin(clock/time * Mathf.PI);
		transform.position = Vector3.Lerp(StartPos, startPos + direction * range, factor);

		clock += Time.deltaTime;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out PlayerInteract player)
			&& player.HasBomb)
		{
			Bomb playerBomb = player.Bomb;

			playerBomb.transform.parent = carryingPoint;
			playerBomb.transform.localPosition = Vector3.zero;
			hasBomb = true;
			player.Bomb = null;
		}
		if (collision.gameObject.TryGetComponent(out Bomb bomb))
		{
			bomb.transform.parent = carryingPoint;
			bomb.Get();
			hasBomb = true;
		}
	}

	private void OnDestroy()
	{
		if(hasBomb)
		{
			carryingPoint.GetChild(0).GetComponent<Bomb>().Release();
		}
		Instantiate(boomerangPickUp, transform.position, Quaternion.identity);
	}
	#endregion
}
