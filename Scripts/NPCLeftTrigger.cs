using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLeftTrigger : MonoBehaviour
{
    public bool left = true;
    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D collision)
    {
        if ((left) && (!collision.gameObject.CompareTag("Transparent")))
        {
            left = false;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Transparent"))
        {
            left = true;
        }
        
    }
}
