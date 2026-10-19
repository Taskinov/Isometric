using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    private Animator anim;
    //public Joystick joystick;
    //public VirtualDPadDirection dpad;


    private float dirX = 0f;
    public float dirY = 0f;
    public static int[] idleDir = { 1, -1 };
    private bool isWalking = false;
    //private bool isPunching = false;
    public bool DoublePunchCD = false;


    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private AudioSource stepSoundEffect;

    //private enum MovementState { idle, running, jumping, falling, dash, idleL, runningL, jumpingL, fallingL, dashL }
    public static string currentState;
    private string newState;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");  // PC
        dirY = Input.GetAxisRaw("Vertical");  //PC
        if (Input.GetKeyDown("left shift"))
        {
            anim.speed = 1.5f;
            moveSpeed = moveSpeed * 1.5f;
        }
        if (Input.GetKeyUp("left shift"))
        {
            anim.speed = 1f;
            moveSpeed = moveSpeed / 1.5f;
        }
        if (dirX != 0f && dirY != 0f)
        {
            rb.velocity = new Vector2(dirX * moveSpeed * 0.75f, dirY * moveSpeed * 0.6f * 0.75f);
        }
        else
        {
            rb.velocity = new Vector2(dirX * moveSpeed, dirY * moveSpeed * 0.6f);
        }
        
        if (dirX < 0)
            {
                idleDir[0] = -1;
            }
        else if (dirX > 0)
        {
            idleDir[0] = 1;
        }
        if (dirY < 0)
        {
            idleDir[1] = -1;
        }
        else if (dirY > 0)
        {
            idleDir[1] = 1;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            if (DoublePunchCD)
            {
                anim.SetBool ("DoublePunch", true);
            } else if (!anim.GetBool("isPunching"))
            {
                DoublePunchCD = true;
                Invoke("DoublePunchCDEnd",0.3f);
                anim.SetBool ("isPunching", true);
            }
        }
        
        UpdateAnimationState();
    }
   
    public void RightDirButtonDown()
    {
        dirX = 1;
    }
    public void LeftDirButtonDown()
    {
        dirX = -1;
    }
    public void UpDirButtonDown()
    {
        dirY = 1;
    }
    public void DownDirButtonDown()
    {
        dirY = -1;
    }
    public void XDirButtonUp()
    {
        dirX = 0;
    }
    public void YDirButtonUp()
    {
        dirY = 0;
    }

    public void PunchEnd()
    {
        anim.SetBool ("isPunching", false);
    }

    public void DoublePunchCDEnd()
    {
        DoublePunchCD = false;
    }
    public void DoublePunchEnd()
    {
        anim.SetBool("DoublePunch", false);
    }

    private void UpdateAnimationState()
    {
        //MovementState state;
        if (dirX > 0f && dirY > 0f)
        {
            newState = "Walk_LT";
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (dirX < 0f && dirY < 0f)
        {
            newState = "Walk_LD";
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (dirX < 0f && dirY > 0f)
        {
            newState = "Walk_LT";
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (dirX > 0f && dirY < 0f)
        {
            newState = "Walk_LD";
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (dirX == 0f && dirY < 0f)
        {
            newState = "Walk_down";
        }
        else if (dirX == 0f && dirY > 0f)
        {
            newState = "Walk_top";
        }
        else if (dirX < 0f && dirY == 0f)
        {
            newState = "Walk_left";
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (dirX > 0f && dirY == 0f)
        {
            newState = "Walk_left";
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (idleDir[0] == 1 && idleDir[1] == 1)
        {
            newState = "IDLE_LT";
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (idleDir[0] == -1 && idleDir[1] == 1)
        {
            newState = "IDLE_LT";
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (idleDir[0] == -1 && idleDir[1] == -1)
        {
            if (anim.GetBool("isPunching"))
            {
                if (anim.GetBool("DoublePunch"))
                {
                    newState = "TwoPunch_LD";
                } else
                {
                    newState = "Punch_LD";
                }
            } else
            {
                newState = "IDLE_LD";
            }
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            if (anim.GetBool("isPunching"))
            {
                if (anim.GetBool("DoublePunch"))
                {
                    newState = "TwoPunch_LD";
                } else
                {
                    newState = "Punch_LD";
                }
            } else
            {
                newState = "IDLE_LD";
            }
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if ((newState != "IDLE_RT") && (newState != "IDLE_LT") && (newState != "IDLE_LD") && (newState != "IDLE_RD") && !isWalking)
        {
            stepSoundEffect.Play();
            isWalking = true;
        }
        else if ((newState == "IDLE_RT") || (newState == "IDLE_LT") || (newState == "IDLE_LD") || (newState == "IDLE_RD"))
        {
            stepSoundEffect.Pause();
            isWalking = false;
        }

        if (newState != currentState)
        {
            anim.Play(newState);
            currentState = newState;
        }



    }
    
}
