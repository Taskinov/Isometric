using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaLevel : MonoBehaviour
{
    private bool coll;
    private float alpha = 1f;
    private float alphaPrev = 1f;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            coll = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            coll = false;
        }
    }
    private void Update()
    {
        if ((coll) && (alpha > 0.2f))
        {
            alpha = alpha - 1f * Time.deltaTime;
            if (alpha < 0.2f)
            {
                alpha = 0.2f;
            }
        } else if ((!coll) && (alpha < 1f))
        {
            alpha = alpha + 1f * Time.deltaTime;
            if (alpha > 1f)
            {
                alpha = 1f;
            }
        }
        
        if (alpha != alphaPrev)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,alpha);
            alphaPrev = alpha;
        }
        
    }
}
