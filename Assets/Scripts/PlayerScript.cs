using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody rb;
    public float speed = 3f;
    public PlayerState state;
    void Start()
    {
        state = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerState s = PlayerState.Idle;
        Vector3 vel = Vector3.zero;
        if(Keyboard.current.wKey.isPressed)
        {
            vel += transform.forward * speed;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            vel += transform.right * -speed;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            vel += transform.forward * -speed;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            vel += transform.right * speed;
            s = PlayerState.Walking;
        }
        SetState(s);
        rb.linearVelocity = vel;
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
