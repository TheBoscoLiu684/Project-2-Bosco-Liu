using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody rb;
    public float speed = 3f;
    public float rotateSpeed = 0.5f;
    public PlayerState state;
    public Animator anim;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim.GetComponent<Animator>();
        state = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        Transform cam = Camera.main.transform;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();


        float currentSpeed = speed;
        PlayerState s = PlayerState.Idle;
        Vector3 direction = Vector3.zero;
        Vector3 vel = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            direction += camForward;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction -= camRight;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            direction -= camForward;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            direction += camRight;
            s = PlayerState.Walking;
        }

        if (s == PlayerState.Walking && Keyboard.current.shiftKey.isPressed)
        {

            currentSpeed = 5f;
            s = PlayerState.Running;
        }

        vel = direction.normalized * currentSpeed;

        

        SetState(s);
        rb.linearVelocity = vel;
        anim.SetFloat("Speed", vel.magnitude);
    }

    public void SetState(PlayerState s)
    {
        if(state == s) return;
        state = s;
        if(state == PlayerState.Idle)
        {
            
        }
        else if(state == PlayerState.Walking)
        {
            
        }
        else if(state == PlayerState.Running)
        {

        }
        else if (state == PlayerState.Crouching)
        {

        }
    }
    public enum PlayerState
    {
        None = 0,
        Idle = 1,
        Walking = 2,
        Running = 3,
        Crouching = 4

    }
}
