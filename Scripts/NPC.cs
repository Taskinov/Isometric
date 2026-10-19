using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator anim;

    public float[] NPCMovement = { 3, -3 };
    public static int[] idleDir = { 1, -1 };
    private string newState;
    [SerializeField] private AudioSource stepSoundEffect;
    private bool isWalking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((NPCMovement[0] > 0) && (NPCMovement[1] > 0))
        {
            rb.velocity = new Vector2(NPCMovement[0] * 0.75f, NPCMovement[1] * 0.6f * 0.75f);
        }
        else if ((NPCMovement[0] == 0) && (NPCMovement[1] == 0))
        {
            rb.velocity = new Vector2(-3f, -3f * 0.6f);
        }
        else
        {
            rb.velocity = new Vector2(NPCMovement[0], NPCMovement[1] * 0.6f);
        }
        if (rb.velocity[0] < 0)
            {
                idleDir[0] = -1;
            }
        else if (rb.velocity[0] > 0)
        {
            idleDir[0] = 1;
        }
        if (rb.velocity[1] < 0)
        {
            idleDir[1] = -1;
        }
        else if (rb.velocity[1] > 0)
        {
            idleDir[1] = 1;
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        //MovementState state;
        if (rb.velocity[0] > 0f && rb.velocity[1] > 0f)
        {
            newState = "Walk_LT";
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (rb.velocity[0] < 0f && rb.velocity[1] < 0f)
        {
            newState = "Walk_LD";
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity[0] < 0f && rb.velocity[1] > 0f)
        {
            newState = "Walk_LT";
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity[0] > 0f && rb.velocity[1] < 0f)
        {
            newState = "Walk_LD";
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (rb.velocity[0] == 0f && rb.velocity[1] < 0f)
        {
            newState = "Walk_down";
        }
        else if (rb.velocity[0] == 0f && rb.velocity[1] > 0f)
        {
            newState = "Walk_top";
        }
        else if (rb.velocity[0] < 0f && rb.velocity[1] == 0f)
        {
            newState = "Walk_left";
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity[0] > 0f && rb.velocity[1] == 0f)
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
            newState = "IDLE_LD";
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            newState = "IDLE_LD";
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
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(newState))
        {
            anim.Play(newState);
        }
    }
}
