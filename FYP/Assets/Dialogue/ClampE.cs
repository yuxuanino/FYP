using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampE : MonoBehaviour
{
    public Image emage; 

    void Update()
    {
        Vector3 ePos = Camera.main.WorldToScreenPoint(this.transform.position);
        emage.transform.position = ePos;
    }
}
