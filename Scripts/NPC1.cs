using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class NPC1 : MonoBehaviour
{
    // Start is called before the first frame update
    //private Rigidbody2D rb;
    private Animator anim;
    private Animator animL;
    private UnityEngine.AI.NavMeshAgent agent;
    public bool target = false;
    public int argX;
    public int argY;
    public float xDif;
    public Vector2 bodyDir;
    public float yDif;
    public float h;
    //public GameObject enemy;
    public bool targetPrev = false;
    public bool punchCD = false;
    [SerializeField] private GameObject PunchTrigger;

    //public float[] NPCMovement = { 3, -3 };
    public static int[] idleDir = { 1, -1 };
    public string newState;
    public string newPunchState;
    public string currentPunchState;
    [SerializeField] private AudioSource stepSoundEffect;
    public bool isWalking = false;
    [SerializeField] GameObject Legs;
    [SerializeField] GameObject enemy;
    private string[,] movementArray = { { "Walk_LD", "Walk_left", "Walk_LT" }, { "Walk_down", "IDLE", "Walk_top" }, {"Walk_LD", "Walk_left", "Walk_LT"} };
    private string[,] idleArray = { { "IDLE_LD", "IDLE_left", "IDLE_LT" }, { "IDLE_down", "IDLE", "IDLE_top" }, {"IDLE_LD", "IDLE_left", "IDLE_LT"} };
    private string[,] punchArray = { { "Punch_LD", "Punch_left", "Punch_LT" }, { "Punch_down", "IDLE", "Punch_top" }, {"Punch_LD", "Punch_left", "Punch_LT"} };
    public float dirXround = 0f;
    public float dirYround = 0f;
    public float prevDirX = -1;
    public float prevDirY = -1;
    public static string currentState;
    public Vector2 velocity;
    public float currentFrame;
    public int startFlag = 1;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (Legs != null)
        {
            animL = Legs.GetComponent<Animator>();
        }
        anim.SetBool("Damage", false);
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity[0] = agent.velocity[0];
        velocity[1] = agent.velocity[1];
        dirXround = (float)Math.Round(agent.velocity[0] / 2);
        dirYround = (float)Math.Round(agent.velocity[1] / 2);
        switch (dirXround)
        {
            case > 1:
                dirXround = 1;
                break;
            case < -1:
                dirXround = -1;
                break;
            default:
                break;
        }
        if (dirYround > 1)
        {
            dirYround = 1;
        }
        if (dirYround < -1)
        {
            dirYround = -1;
        }
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
        target = anim.GetBool("Damage");
        if (target)
        {
            TargetEnabled();
            // if (target != targetPrev)
            // {
            //     enemy = GetComponent<Combat>().enemy;
            // }
            targetPrev = true;
        }
        UpdateAnimationState();
    }

    public void TargetEnabled()
    {
        xDif = transform.position.x - enemy.transform.position.x;
        yDif = transform.position.y - enemy.transform.position.y;
        h = (Math.Abs(xDif) > Math.Abs(yDif)) ? Math.Abs(xDif) : Math.Abs(yDif);
        bodyDir = new Vector2((float)Math.Round(xDif/h), (float)Math.Round(yDif/h));
        if (h > 10)
        {
            anim.SetBool("Damage", false);
        } else if ((h < 2) && (!anim.GetBool("isPunching")) && (!punchCD))
        {
            anim.SetBool("isPunching", true);
            startFlag = 0;
            punchCD = true;
        }
    }
    //async Task PunchEnd()
    private void PunchEnd()
    {
        anim.SetBool("isPunching", false);
        //await Task.Delay(300);
        Invoke("PunchCDEnd", 1.5f);
        
        //isPunchingAnim = false;
        //currentPunchState = "";
    }
    private void PunchCDEnd()
    {
        punchCD = false;
    }

    private void UpdateAnimationState()
    {
        // newState = movementArray[(int)dirXround+1,(int)dirYround+1];
        // if (newState == "IDLE")
        // {
        //     newState = idleArray[(int)prevDirX + 1, (int)prevDirY + 1];
        //     if (newState == "IDLE")
        //     {
        //         newState = currentState;
        //     }
        // }
        // if (dirXround > 0)
        // {
        //     GetComponent<SpriteRenderer>().flipX = true;
        //     if (Legs != null)
        //     {
        //         Legs.GetComponent<SpriteRenderer>().flipX = true;
        //     }
        // } else if (dirXround < 0)
        // {
        //     GetComponent<SpriteRenderer>().flipX = false;
        //     if (Legs != null)
        //     {
        //         Legs.GetComponent<SpriteRenderer>().flipX = false;
        //     }
        // }

        if (!target)
        {
            argX = (int)prevDirX + 1;
            argY = (int)prevDirY + 1;
            if (anim.GetBool("isPunching"))
            {
                newPunchState = punchArray[(int)dirXround+1,(int)dirYround+1];
            } 
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
                if (Legs != null)
                {
                    Legs.GetComponent<SpriteRenderer>().flipX = true;
                }
            } else if (dirXround < 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (Legs != null)
                {
                    Legs.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
        else
        {
            argX = -(int)bodyDir[0]+1;
            argY = -(int)bodyDir[1]+1;
            if ((Math.Abs(dirXround-bodyDir[0]) <= 1) && ((Math.Abs(dirYround-bodyDir[1]) <= 1)))
            {
                animL.SetFloat ("Dir", -1f);
            } else
            {
                animL.SetFloat ("Dir", 1f);
            }
            if (xDif > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                Legs.GetComponent<SpriteRenderer>().flipX = false;
            } else
            {
                GetComponent<SpriteRenderer>().flipX = true;
                Legs.GetComponent<SpriteRenderer>().flipX = true;
            }
            if (anim.GetBool("isPunching"))
            {
                newPunchState = punchArray[argX, argY];
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

        if (newState != currentState)
        {
            if (anim.GetBool("isPunching"))
            {
                if ((newPunchState != currentPunchState) || (startFlag == 0))
                {
                    PunchTrigger.GetComponent<CapsuleCollider2D>().enabled = true;
                    PunchTrigger.GetComponent<Transform>().localPosition = new Vector3(argX-1,(argY-1) * 0.7f,0);
                    currentFrame = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                    anim.Play(newPunchState, -1, currentFrame * startFlag);
                    startFlag = 1;
                    currentPunchState = newPunchState;
                }
            } else 
            {
                PunchTrigger.GetComponent<CapsuleCollider2D>().enabled = false;
                anim.Play(newState);
            }
            
            if (Legs != null)
            {
                animL.Play(newState);
            }
            currentState = newState;
        }
    }
}
