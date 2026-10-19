using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObjects : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("shaked"))
        {
            if (collision.gameObject.GetComponent<Animator>())
            {
                collision.gameObject.GetComponent<Animator>().Play("Shake");
            }
            collision.gameObject.GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("shaked"))
        {
            if (collision.gameObject.GetComponent<Animator>())
            {
                collision.gameObject.GetComponent<Animator>().Play("Shake");
            }
            collision.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
