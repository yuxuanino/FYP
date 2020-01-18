using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideParticle : MonoBehaviour
{
    public GameObject particlePrefab;
    bool particleActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!particleActive)
        {
            Instantiate(particlePrefab);
        }
    }


}
