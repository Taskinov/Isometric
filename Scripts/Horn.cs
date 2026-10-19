using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{
    [SerializeField] private AudioSource hornSoundEffect;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hornSoundEffect.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hornSoundEffect.Pause();
        }
    }
}
