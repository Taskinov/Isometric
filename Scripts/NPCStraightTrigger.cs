using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStraightTrigger : MonoBehaviour
{
    public bool straight = true;
    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D collision)
    {
        if ((straight) && (!collision.gameObject.CompareTag("Transparent")))
        {
            straight = false;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Transparent"))
        {
            straight = true;
        }
        
    }
}
