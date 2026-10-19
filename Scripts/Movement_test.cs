using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement_test : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    private Animator anim;
    private Animator anim1;
    //public Joystick joystick;
    //public VirtualDPadDirection dpad;
    public Text Text;
    public int startFlag = 1;
    [SerializeField] private AudioSource stepSoundEffect;

    public float dirX = 0f;
    public float dirY = 0f;
    public float dirXround = 0f;
    public float dirYround = 0f;
    public static float[] angles = { 0.393f, 1.178f, -1.178f, 0,393f};

    private bool isWalking = false;
    //private bool isPunching = false;
    public bool DoublePunchCD = false;
    public Vector2 bodyDir;
    [SerializeField] private GameObject Legs;
    [SerializeField] private GameObject PunchTrigger;
    [SerializeField] private GameObject stick8way;
    [SerializeField] private GameObject rightStick;
    [SerializeField] private GameObject waypointRotate;
    private bool target = false;
    private string[,] movementArray = { { "Walk_LD", "Walk_left", "Walk_LT" }, { "Walk_down", "IDLE", "Walk_top" }, {"Walk_LD", "Walk_left", "Walk_LT"} };
    private string[,] idleArray = { { "IDLE_LD", "IDLE_left", "IDLE_LT" }, { "IDLE_down", "IDLE", "IDLE_top" }, {"IDLE_LD", "IDLE_left", "IDLE_LT"} };
    private string[,] punchArray = { { "Punch_LD", "Punch_left", "Punch_LT" }, { "Punch_down", "IDLE", "Punch_top" }, {"Punch_LD", "Punch_left", "Punch_LT"} };
    private string[,] twoPunchArray = { { "TwoPunch_LD", "TwoPunch_left", "TwoPunch_LT" }, { "TwoPunch_down", "IDLE", "TwoPunch_top" }, {"TwoPunch_LD", "TwoPunch_left", "TwoPunch_LT"} };
    //private string[,] damageArray = { { "Damage_LD", "Damage_left", "Damage_LT" }, { "Damage_down", "IDLE", "Damage_top" }, {"Damage_LD", "Damage_left", "Damage_LT"} };
    [SerializeField] private float moveSpeed = 7f;
    private float baseSpeed;

    //private enum MovementState { idle, running, jumping, falling, dash, idleL, runningL, jumpingL, fallingL, dashL }
    public static string currentState;
    public string newState;
    public static string currentPunchState;
    public string newPunchState;
    public bool isPunchingAnim = false;
    public float currentFrame;
    public float xDif;
    public float yDif;
    public float h;
    public float prevDirX = -1;
    public float prevDirY = -1;
    public GameObject closestTarget;
    public float xSpeedMult = 1;
    public float ySpeedMult = 1;
    Vector2 stickValue;
    public Vector2 rightStickValue;
    private PlayerInput playerInput;
    private bool fireButtonPressed = false;
    public int argX;
    public int argY;
    public bool touch = false;
    public bool flashlightOn = false;
    // private bool runButtonPressed = false;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = Legs.GetComponent<Animator>();
        anim1 = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
        baseSpeed = moveSpeed;
    }
    

    // Update is called once per frame
    private void Update()
    {
        stickValue = (!touch) ? playerInput.actions["Move"].ReadValue<Vector2>() : stick8way.GetComponent<EightWayOnScreenStick>().controls;
        
        dirX = stickValue[0];
        dirY = stickValue[1];
        Text.text = stickValue.ToString() + anim.speed.ToString() + target.ToString() + fireButtonPressed.ToString();
        // dirX = Input.GetAxisRaw("Horizontal");  // PC
        // dirY = Input.GetAxisRaw("Vertical");  //PC
        if (playerInput.actions["Run"].IsPressed())
        {
            
            anim.speed = 1.5f;
            moveSpeed = 10f;
            // runButtonPressed = true;
        } else 
        {
            anim.speed = 1f;
            moveSpeed = baseSpeed;
        }
        // if (dirX != 0f && dirY != 0f)
        // {
        //     rb.velocity = new Vector2(dirX * moveSpeed * 0.75f * xSpeedMult, dirY * moveSpeed * 0.6f * 0.75f * ySpeedMult);
        // }
        // else
        // {
            rb.velocity = new Vector2(dirX * moveSpeed * xSpeedMult, dirY * moveSpeed * 0.6f * ySpeedMult);
        // }
        dirXround = (float)Math.Round(dirX);
        dirYround = (float)Math.Round(dirY);
        
        

        
        if (!((dirXround == 0) && (dirYround == 0)))
        {
            if (prevDirY != dirYround)
            {
                prevDirY = dirYround;
            }
            if (prevDirX != dirXround)
            {
                prevDirX = dirXround;
            }
        }
        if (flashlightOn)
        {
            rightStickValue = rightStick.GetComponent<EightWayOnScreenStick>().delta;
            double radians = Math.Atan2(rightStickValue[0], rightStickValue[1]);
            double degrees = radians * (180.0 / Math.PI);
            waypointRotate.transform.eulerAngles = new Vector3(60, 0, -(float)(degrees));
            FlashlightEnabled();
        }
        
        if (playerInput.actions["Fire"].WasPressedThisFrame())
        {
            fireButtonPressed = true;
            FireButton();
        }
        if (playerInput.actions["Target"].WasPressedThisFrame())
        {
            TargetButton();
        }
        if (target)
        {
            if (closestTarget != null)
            {
                TargetEnabled();
            }
            else
            {
                TargetDisabled();
            }
        }
        
        UpdateAnimationState();
    }
    

    public void TargetDisabled()
    {
        bodyDir = new Vector2(0,0);
        target = false;
        anim.SetFloat ("Dir", 1f);
    }
    public void TargetEnabled()
    {
        xDif = transform.position.x - closestTarget.transform.position.x;
        yDif = transform.position.y - closestTarget.transform.position.y;
        if (((xDif*xDif + yDif*yDif) > 100f) || (!closestTarget.GetComponent<Rigidbody2D>().simulated))
        {
            TargetDisabled();
        }
        if (Math.Abs(xDif) > Math.Abs(yDif))
        {
            h = Math.Abs(xDif);
        } else 
        {
            h = Math.Abs(yDif);
        }
        bodyDir = new Vector2((float)Math.Round(xDif/h), (float)Math.Round(yDif/h));
    }
    public void FlashlightEnabled()
    {
        bodyDir = -rightStick.GetComponent<EightWayOnScreenStick>().bodyPos;
    }
    // public void RightDirButtonDown()
    // {
    //     dirX = 1;
    // }
    // public void LeftDirButtonDown()
    // {
    //     dirX = -1;
    // }
    // public void UpDirButtonDown()
    // {
    //     dirY = 1;
    // }
    // public void DownDirButtonDown()
    // {
    //     dirY = -1;
    // }
    // public void XDirButtonUp()
    // {
    //     dirX = 0;
    // }
    // public void YDirButtonUp()
    // {
    //     dirY = 0;
    // }
    public void Flashlight()
    {
        flashlightOn = !flashlightOn;
        anim1.SetBool("flashlight", flashlightOn);
    }

    public void PunchEnd()
    {
        anim1.SetBool("isPunching", false);
        isPunchingAnim = false;
        currentPunchState = "";
        
    }

    public void DoublePunchCDEnd()
    {
        DoublePunchCD = false;
        fireButtonPressed = false;
    }
    public void DoublePunchEnd()
    {
        anim1.SetBool("DoublePunch", false);
        currentPunchState = "";
    }
    public void FireButton()
    {
        isPunchingAnim = false;
        startFlag = 0;
        if (DoublePunchCD)
        {
            anim1.SetBool ("DoublePunch", true);
        } else if (!anim1.GetBool("isPunching"))
        {
            DoublePunchCD = true;
            Invoke("DoublePunchCDEnd",0.3f);
            anim1.SetBool ("isPunching", true);
        }
    }
    public void TargetButton()
    {
        target = !target;
        if (!target)
        {
            TargetDisabled();
        }
        else
        {
            closestTarget = FindClosestEnemy();
        }
    }
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("NPC");
        GameObject closest = null;
        float distance = 81f;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void UpdateAnimationState()
    {
        //MovementState state;
        if ((!target) && (!flashlightOn))
        {
            argX = (int)prevDirX + 1;
            argY = (int)prevDirY + 1;
            newState = movementArray[(int)dirXround+1,(int)dirYround+1];
            if (newState == "IDLE")
            {
                newState = idleArray[argX, argY];
                if (newState == "IDLE")
                {
                    newState = currentState;
                }
            }
            if (dirXround > 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                Legs.GetComponent<SpriteRenderer>().flipX = true;
            } else if (dirXround < 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                Legs.GetComponent<SpriteRenderer>().flipX = false;
            }
            anim.SetFloat ("Dir", 1f);
        }
        else
        {
            argX = -(int)bodyDir[0]+1;
            argY = -(int)bodyDir[1]+1;
            if ((Math.Abs(dirXround-bodyDir[0]) <= 1) && ((Math.Abs(dirYround-bodyDir[1]) <= 1)))
            {
                anim.SetFloat ("Dir", -1f);
            } else
            {
                anim.SetFloat ("Dir", 1f);
            }
            if (bodyDir[0] > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                Legs.GetComponent<SpriteRenderer>().flipX = false;
            } else
            {
                GetComponent<SpriteRenderer>().flipX = true;
                Legs.GetComponent<SpriteRenderer>().flipX = true;
            }
            newState = movementArray[argX,argY];
            if ((dirXround == 0) && (dirYround == 0))
            {
                newState = idleArray[argX,argY];
            }
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
        if (anim1.GetBool("isPunching"))
        {
            if (!isPunchingAnim)
            {
                PunchTrigger.GetComponent<CapsuleCollider2D>().enabled = true;
                
                PunchTrigger.GetComponent<Transform>().localPosition = new Vector3(argX-1,(argY-1) * 0.7f,0);
                isPunchingAnim = true;
                //startFlag = 0;
            }
            if (anim1.GetBool("DoublePunch"))
            {
                newPunchState = twoPunchArray[argX,argY];
                
                if (newPunchState != currentPunchState) 
                {
                    currentFrame = anim1.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                    anim1.Play(newPunchState, -1, currentFrame * startFlag);
                    startFlag = 1;
                    currentPunchState = newPunchState;
                }
            } else
            {
                newPunchState = punchArray[argX,argY];
                //newPunchState = damageArray[(int)prevDirX + 1,(int)prevDirY + 1];
                if (newPunchState != currentPunchState)
                {
                    currentFrame = anim1.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                    anim1.Play(newPunchState, -1, currentFrame * startFlag);
                    startFlag = 1;
                    currentPunchState = newPunchState;
                }
            }
            
            
        } else
        {
            PunchTrigger.GetComponent<CapsuleCollider2D>().enabled = false;
            if (newState != currentState)
            {
                currentFrame = anim1.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                anim1.Play(newState, -1, currentFrame);
            }
        }

        if (newState != currentState)
        {
            currentFrame = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
            anim.Play(newState, -1, currentFrame);
            currentState = newState;
        }



    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            xSpeedMult = 0.70f;
            ySpeedMult = 1.30f;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            xSpeedMult = 1f;
            ySpeedMult = 1f;
        }
    }
    
}
