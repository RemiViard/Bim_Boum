using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerControls playerControls;
    Animator animator;
    [SerializeField] public float Speed;
    [SerializeField] GameObject StunFx;
    Rigidbody rg;
    Vector2 movementInput = Vector2.zero;
    public bool CanMove = true;
    public bool isStun = false;
    public void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
       
        
    }
    private void FixedUpdate()
    {
        if (CanMove && !isStun)
        {
            Vector2 velocity = movementInput;
            transform.LookAt(transform.position + new Vector3(velocity.x, 0, velocity.y));
            //Debug.Log(velocity);
            if (velocity.x == 0 && velocity.y == 0)
            {
                rg.velocity = Vector3.zero;
            }
            else
            {
                velocity.Normalize();
                rg.velocity = new Vector3(velocity.x,0, velocity.y) * Speed;
            }
            animator.SetFloat("Speed", velocity.magnitude);
        }
    }
    public void StopMovement()
    {
        CanMove = false;
        rg.velocity = Vector3.zero;
    }
    public void ResetMovement()
    {
        CanMove = true;
    }
    public void Stun()
    {
        StunFx.SetActive(true);
        isStun = true;
    }
    public void ResetStun()
    {
        StunFx.SetActive(false);
        isStun = false;
        
    }
}
