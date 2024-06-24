using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEngine : MonoBehaviour
{
    // Start is called before the first frame update
    private float totalDuration;
    void Start()
    {
        totalDuration = GetComponent<ParticleSystem>().main.duration + GetComponent<ParticleSystem>().main.startLifetimeMultiplier;
        Invoke("DestroyBoom", totalDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DestroyBoom()
    {
        Destroy(gameObject);
    }
}
