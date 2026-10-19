using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [SerializeField] private Transform followingObject;

    void Update()
    {
        transform.position = new Vector3(followingObject.position.x, followingObject.position.y, transform.position.z);
    }
}
