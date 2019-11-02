using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public GameObject spawnerPos1;
    public GameObject spawnerPos2;
    public GameObject spawnerPos3;
    public GameObject spawnerPos4;

    public GameObject spikes;

    public bool spikesShot;

    public float speed = 20f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!spikesShot)
        {
            spikesShot = true;
            StartCoroutine(SpikeSpawnTimer());
        }

    }


    public IEnumerator SpikeSpawnTimer()
    {
        yield return new WaitForSeconds(1f);
        GameObject spawnedSpikes1 = Instantiate(spikes, spawnerPos1.transform.position, Quaternion.identity);
        GameObject spawnedSpikes2 = Instantiate(spikes, spawnerPos2.transform.position, Quaternion.identity);
        GameObject spawnedSpikes3 = Instantiate(spikes, spawnerPos3.transform.position, Quaternion.identity);
        GameObject spawnedSpikes4 = Instantiate(spikes, spawnerPos4.transform.position, Quaternion.identity);

        spawnedSpikes1.transform.parent = gameObject.transform;
        spawnedSpikes2.transform.parent = gameObject.transform;
        spawnedSpikes3.transform.parent = gameObject.transform;
        spawnedSpikes4.transform.parent = gameObject.transform;

        Destroy(spawnedSpikes1, 4f);
        Destroy(spawnedSpikes2, 4f);
        Destroy(spawnedSpikes3, 4f);
        Destroy(spawnedSpikes4, 4f);
        spikesShot = false;



        yield return null;
    }


        
}
