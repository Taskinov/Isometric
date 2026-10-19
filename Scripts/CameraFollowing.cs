using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{

    [SerializeField] private GameObject[] following;
    public int i = 0;

    void Update()
    {
        transform.position = (i == 0) ? new Vector3(following[i].transform.position.x - 2, following[i].transform.position.y + 2, transform.position.z) : 
        new Vector3(following[i].transform.position.x, following[i].transform.position.y + 2, transform.position.z);
    }
}
