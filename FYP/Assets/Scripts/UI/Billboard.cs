using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform camTrans;

    Quaternion originR;

    private void Start()
    {
        originR = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camTrans.rotation * originR;
    }
}
