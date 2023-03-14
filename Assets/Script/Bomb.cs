using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherAndTime;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bomb : MonoBehaviour
{

	#region Fields
	float BombTimer;
	[SerializeField] TMPro.TMP_Text text;
	[SerializeField] GameManager gameManager;
	[SerializeField] Vector3 offset;
	[SerializeField] GameObject Fx;
    [SerializeField] GameObject Visual;
	Rigidbody rb;
	SphereCollider collider;
	public bool onCoolDown = false;
	public int PlayerId = -1;
	public PlayerInteract playerInteract;
	bool canBePicked = true;
	bool hasExplode = false;
	public bool CanBePicked
	{
		get => canBePicked;
	}
	#endregion

	#region Methods
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();
		rb.useGravity = true;
		rb.isKinematic = false;
		collider.isTrigger = false;
		BombTimer = Random.Range(20, 45);
	}
	private void Update()
	{
		if (!hasExplode)
		{
			if (BombTimer <= 0)
			{
				Explode();
				BombTimer = 0;
				text.text = BombTimer.ToString();
			}
			else
			{
				BombTimer -= Time.deltaTime;
			}
			if (BombTimer >= 10)
			{
				if (BombTimer.ToString().Length > 4)
				{
					text.text = BombTimer.ToString().Substring(0, 4);
				}
			}
			else
			{
				if (BombTimer.ToString().Length > 4)
				{
					text.text = BombTimer.ToString().Substring(0, 3);
				}
			}
		}
	}

	public void Get()
	{
		if(!onCoolDown)
        {
			canBePicked = false;
			rb.useGravity = false;
			rb.isKinematic = true;
			collider.enabled = false;
			transform.localPosition = Vector3.zero;
		}
	}

	public void Get(PlayerInteract player)
	{
		if (!onCoolDown)
		{
			PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
			float baseSpeed = playerMovement.Speed;
			canBePicked = false;
			rb.useGravity = false;
			rb.isKinematic = true;
			collider.enabled = false;
			transform.parent = player.Fist;
			transform.localPosition = offset;
			PlayerId = player.Id;
			playerInteract = player;
			playerMovement.Speed = baseSpeed * 1.2f;
			TimeManager.CreateNewTimer(() => playerMovement.Speed = baseSpeed, 1f, playerMovement, true);
		}
	}

	public void Release()
	{
		canBePicked = true;
		rb.useGravity = true;
		rb.isKinematic = false;
		collider.enabled = true;
		transform.parent = null;
		PlayerId = -1;
		onCoolDown = true;
		TimeManager.CreateNewTimer(() => onCoolDown = false, 1f, this, true);
		playerInteract = null;
	}

	public void Release(PlayerInteract player)
	{
		canBePicked = true;
		rb.useGravity = true;
		rb.isKinematic = false;
		collider.enabled = true;
		transform.parent = null;
		transform.position = player.CarryingPoint.position;
		PlayerId = -1;
		onCoolDown = true;
		TimeManager.CreateNewTimer(() => onCoolDown = false, 1f, this, true);
		playerInteract = null;
	}
	public void Explode()
	{
		
		gameManager.EndRound(PlayerId);
		hasExplode = true;
		Visual.SetActive(false);
		Fx.SetActive(true);
		if (playerInteract != null)
		{
			playerInteract.ReleaseBomb();
		}
		TimeManager.CreateNewTimer(Destroy, 1, this, true);
	}
	public void Destroy()
    {
		Destroy(gameObject);
    }
	#endregion
}
