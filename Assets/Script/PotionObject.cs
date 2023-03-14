using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PotionObject : MonoBehaviour
{
	[SerializeField, Min(0.1f)]
	float time = 1f;

	[SerializeField, Min(0.2f)]
	float rangeThrow = 2f;

	[SerializeField, Min(0.2f)]
	float rangeEffect = 1f;

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

    private void Awake()
    {
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		Collider coll = GetComponent<Collider>();
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;
		coll.isTrigger = false;
		rigidbody.AddForce(direction * rangeThrow);
    }

    private void Update()
	{
		if (clock >= time)
		{
			Destroy(gameObject);
			return;
		}

		clock += Time.deltaTime;
	}

    private void OnCollisionEnter(Collision collision)
    {
		Destroy(gameObject);
    }

    private void OnDestroy()
    {
        foreach(Collider coll in Physics.OverlapSphere(transform.position, rangeEffect))
        {
			if(coll.TryGetComponent(out PlayerMovement player))
            {
				//TODO STUN / invert control
            }
        }
    }
    private void OnDrawGizmos()
    {
		Gizmos.DrawWireSphere(transform.position, 2f);
    }

}
