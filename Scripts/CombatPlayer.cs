using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    private Animator anim;
    private string[,] damageArray = { { "Damage_LT", "Damage_left", "Damage_LD" }, { "Damage_top", "IDLE", "Damage_down" }, {"Damage_LT", "Damage_left", "Damage_LD"} };
    public Vector3 direction;
    private SpriteRenderer sr;
    public GameObject enemy;
    private Color baseColor;
    public float hp = 10;
    public float dp = 1;
    [SerializeField] private AudioSource damageSoundEffect;
    [SerializeField] private GameObject legs;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Punch"))
        {
            Invoke("Damage", 0.1f);
            enemy = collision.gameObject.transform.parent.gameObject;
        }
    }
    private void Damage()
    {
        direction = new Vector3(enemy.GetComponent<Movement_test>().argX, enemy.GetComponent<Movement_test>().argY, 0);
        if ((enemy.transform.position.x - transform.position.x) > 0)
        {
            sr.flipX = true;
        } else 
        {
            sr.flipX = false;
        }
        hp -= dp;
        if (hp <= 0)
        {
            anim.Play("Knock_out");
            GetComponent<NPC1>().enabled = false;
            GetComponent<WaypointMover1>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder = 3;
            legs.GetComponent<SpriteRenderer>().enabled = false;
        } else 
        {
            //anim.SetBool("isPunching", true);
            anim.SetBool("Damage", true);
            anim.Play(damageArray[(int)direction[0], (int)direction[1]]);
        }
        sr.color = new Color(1f, 1f, 1f, 1f);
        damageSoundEffect.Play();
        Invoke("ColorReset", 0.2f);
    }
    private void ColorReset()
    {
        sr.color = baseColor;
    }
}
