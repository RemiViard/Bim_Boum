using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WeatherAndTime;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    #region Fields
    public Vector3 awakePos;

    [SerializeField]
    List<Interactible> interacts = new List<Interactible>();
    public List<Interactible> Interacts
    {
        get
        {
            interacts.Sort((x, y) => Vector3.Distance(transform.position, x.gameObject.transform.position) < Vector3.Distance(transform.position, y.gameObject.transform.position) ? 1 : 0);
            return interacts;
        }
    }

    PickUp currentPickUp = null;
    public PickUp CurrentPickUp
    {
        get => currentPickUp;
        set
        {
            if (value != null)
            {
                isCarryingSomething = true;
            }
            else
            {
                Debug.Log("setter");
                isCarryingSomething = false;
                currentPickUp?.Unregister(this);
            }
            currentPickUp = value;
        }
    }
    [SerializeField]
    bool isCarryingSomething = false;
    public bool IsCarryingSomething
    {
        get => isCarryingSomething;
    }
    [SerializeField]
    Transform carryingPoint;
    public Transform CarryingPoint
    {
        get => carryingPoint;
    }

    [SerializeField]
    bool hasBomb = false;
    public bool HasBomb
    {
        get => hasBomb;
    }

    [SerializeField]
    Bomb bomb = null;
    public int Id;
    public Bomb Bomb
    {
        get => bomb;
        set
        {
            if (value == null)
            {
                hasBomb = false;
                bomb = value;
            }
        }
    }
    Animator animator;
    [SerializeField] Transform fist;
    public Transform Fist
    {
        get => fist;
    }
    [SerializeField]
    Transform spawnPoint;

    public Transform SpawnPoint
    {
        get => spawnPoint;
    }
    [SerializeField] SkinnedMeshRenderer mr;
    [SerializeField] GameObject Fx;
    bool canPunch = true;

    #endregion

    #region Methods
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        awakePos = transform.position;
        SceneManager.sceneLoaded += MyReset;
        DontDestroyOnLoad(gameObject);
    }
    public void LoadMesh()
    {
        mr.sharedMesh = MeshManager.Instance.meshes[Id];
        mr.material = MeshManager.Instance.materials[Id];
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!hasBomb)
            {
                animator.SetTrigger("Use");
            }
        }
    }
    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (IsCarryingSomething)
            {
                CurrentPickUp = null;
            }
        }
    }

    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!hasBomb && canPunch && !GetComponent<PlayerMovement>().isStun)
            {
                animator.SetTrigger("Punch");
                canPunch = false;
            }
        }
    }
    public void Use()
    {
        Debug.Log("Use");
        if (isCarryingSomething && currentPickUp != null && !currentPickUp.InUse)
            currentPickUp.Use(this);
        else if (interacts.Count > 0 && !isCarryingSomething)
        {
            Interacts[0]?.Interact(this);
        }
    }
    public void Punch()
    {
        Fx.SetActive(true);

        TimeManager.CreateNewTimer(ResetFx, 1f, this, true);
        foreach (var collider in Physics.OverlapSphere(fist.position + transform.forward, 1))
        {
            if (collider.TryGetComponent(out PlayerInteract otherPlayer) && collider != GetComponent<Collider>())
            {
                Debug.Log("Boom");
                collider.attachedRigidbody.AddForce(transform.forward * 100, ForceMode.Impulse);
                collider.GetComponent<PlayerMovement>().CanMove = false;
                TimeManager.CreateNewTimer(collider.GetComponent<PlayerMovement>().ResetMovement, 2f, collider.gameObject, true);
                if (otherPlayer.hasBomb)
                {
                    otherPlayer.ReleaseBomb();
                }
            }
        }
    }
    public void RegisterInteract(Interactible interactible)
    {
        interacts.Add(interactible);
        interactible.Register(this);
    }

    public void UnregisterInteract(Interactible interactible)
    {
        interacts.Remove(interactible);
        //interactible.Unregister(this);
    }

    public void ReleaseBomb()
    {
        this.bomb.Release(this);
        this.bomb = null;
        hasBomb = false;
        CurrentPickUp = null;
    }
    public void ResetFx()
    {
        Fx.SetActive(false);
        canPunch = true;
    }

    private async void MyReset(Scene scene, LoadSceneMode loadSceneMode)
    {
        transform.position = awakePos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Bomb bomb)
            && bomb.CanBePicked
            && !bomb.onCoolDown
            && currentPickUp == null)
        {
            bomb.Get(this);
            this.hasBomb = true;
            this.bomb = bomb;
        }
    }
    public bool Cudgel()
    {
        foreach (var collider in Physics.OverlapSphere(fist.position + transform.forward, 1))
        {
            if (collider.TryGetComponent(out PlayerInteract otherPlayer) && collider != GetComponent<Collider>())
            {

                collider.attachedRigidbody.AddForce(transform.forward * 100, ForceMode.Impulse);
                collider.GetComponent<PlayerMovement>().CanMove = false;
                TimeManager.CreateNewTimer(collider.GetComponent<PlayerMovement>().ResetMovement, 2f, collider.gameObject, true);
                if (otherPlayer.hasBomb)
                {
                    otherPlayer.ReleaseBomb();
                }
                Fx.SetActive(true);
                TimeManager.CreateNewTimer(ResetFx, 1f, this, true);
                otherPlayer.GetComponent<PlayerMovement>().Stun();
                TimeManager.CreateNewTimer(otherPlayer.GetComponent<PlayerMovement>().ResetStun, 4f, otherPlayer.gameObject, true);
                return true;
            }

        }
        return false;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(fist.position + transform.forward, 1f);
    }
}
