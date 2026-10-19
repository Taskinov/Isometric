using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMachine : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource[] playlist;
    [SerializeField] private AudioSource shift;
    private int currentSong = 0;

    // Update is called once per frame
    void Update()
    {
        if (!playlist[currentSong].isPlaying)
        {
            currentSong++;
            if (currentSong >= playlist.Length)
            {
                currentSong = 0;
            }
            shift.Play();
            playlist[currentSong].Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playlist[currentSong].Stop();
            currentSong++;
            if (currentSong >= playlist.Length)
            {
                currentSong = 0;
            }
            shift.Play();
            playlist[currentSong].Play();
            GetComponent<Animator>().Play("Shake");
        }
    }
}
