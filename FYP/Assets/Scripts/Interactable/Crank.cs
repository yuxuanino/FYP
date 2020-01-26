using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    private float originalRotationZ;

    // Start is called before the first frame update
    void Start()
    {
        originalRotationZ = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        //Well depends on rotation it does something.
    }
}
