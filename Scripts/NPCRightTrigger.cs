using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRightTrigger : MonoBehaviour
{
    public bool right = true;
    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D collision)
    {
        if ((right) && (!collision.gameObject.CompareTag("Transparent")))
        {
            right = false;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Transparent"))
        {
            right = true;
        }
    }
}
