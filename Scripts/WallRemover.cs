using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallRemover : MonoBehaviour
{
    [SerializeField] private GameObject[] outside;
    [SerializeField] private GameObject[] inside;
    [SerializeField] private GameObject[] changeSort;
    public bool sort = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < outside.Length; i++)
            {
                if (outside[i].GetComponent<SpriteRenderer>() != null)
                {
                    outside[i].GetComponent<SpriteRenderer>().enabled = false;
                } else 
                {
                    outside[i].GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
                }
            }
            for (int i = 0; i < inside.Length; i++)
            {
                if (inside[i].GetComponent<SpriteRenderer>() != null)
                {
                    inside[i].GetComponent<SpriteRenderer>().enabled = true;
                } else 
                {
                    inside[i].GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = true;
                }
            }
            if (sort)
            {
                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                for (int i = 0; i < changeSort.Length; i++)
                {
                    changeSort[i].GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
            }
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < outside.Length; i++)
            {
                if (outside[i].GetComponent<SpriteRenderer>() != null)
                {
                    outside[i].GetComponent<SpriteRenderer>().enabled = true;
                } else 
                {
                    outside[i].GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = true;
                }
            }
            for (int i = 0; i < inside.Length; i++)
            {
                if (inside[i].GetComponent<SpriteRenderer>() != null)
                {
                    inside[i].GetComponent<SpriteRenderer>().enabled = false;
                } else 
                {
                    inside[i].GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
                }
            }
            if (sort)
            {
                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
                for (int i = 0; i < changeSort.Length; i++)
                {
                    changeSort[i].GetComponent<SpriteRenderer>().sortingOrder = 4;
                }
            }
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().Pause();
            }
        }
    }
}

