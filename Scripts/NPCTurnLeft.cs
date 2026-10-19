using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTurnLeft : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    [SerializeField] private GameObject NPC;
    
    private void OnTriggerEnter2D()
    {
        NPC.GetComponent<NPC>().NPCMovement[1] = 0f;
    }
    private void OnTriggerExit2D()
    {
        NPC.GetComponent<NPC>().NPCMovement[1] = -3f;
    }
}
