using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody rb;
    public Slider staminaSlider;
    public float speed = 3f;
    public float rotateSpeed = 0.5f;
    public float stamina = 100f;
    public float staminaTotal = 100f;
    public float staminaDrain = 20f;
    public float staminaRegen = 10f;
    public PlayerState state;
    public Animator anim;
    public Transform cameraPivot;
    void Start()
    {
        anim.GetComponent<Animator>();
        stamina = staminaTotal;
        staminaSlider.maxValue = staminaTotal;
        staminaSlider.value = stamina;
        state = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = cameraPivot.forward;
        Vector3 right = cameraPivot.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        float currentSpeed = speed;
        PlayerState s = PlayerState.Idle;
        Vector3 direction = Vector3.zero;
        Vector3 vel = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            direction += forward;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction -= right;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            direction -= forward;
            s = PlayerState.Walking;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            direction += right;
            s = PlayerState.Walking;
        }

        if (s == PlayerState.Walking && Keyboard.current.shiftKey.isPressed && stamina > 0f)
        {
            currentSpeed = 5f;
            s = PlayerState.Running;
            anim.SetBool("isRunning", true);
            stamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isRunning", false);
            stamina += staminaRegen * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0f, staminaTotal);
        staminaSlider.value = stamina;
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
