using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float speed = 25f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.parent.transform.right * Time.deltaTime * speed;
        transform.rotation = transform.parent.transform.rotation;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        /*
        if(other.tag == "Player")
        {
            print("Hit player");
            Destroy(gameObject);
        }
        */

        if(other.tag == "Box")
        {
            Destroy(gameObject);
        }
    }

}
