using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCarEngine : MonoBehaviour
{
    [SerializeField] MeshCollider starCollider;
    [SerializeField] GameObject boom;
    private Rigidbody enemyCarRB;
    private CapsuleCollider capsuleCollider;
    [SerializeField] BoxCollider roadCollider;
    [SerializeField] Collider[] colliders;
    [SerializeField] Vector3 speed;
    // Start is called before the first frame update
    void Start()
    {
        enemyCarRB = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Physics.IgnoreCollision(capsuleCollider, starCollider);
    }
    // Update is called once per frame
    void Update()
    {
        MoveCar();
        DestroyWhenNotOnRoad();
    }
    private void DestroyWhenNotOnRoad()
    {
        colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f, Quaternion.identity);
        if (Array.IndexOf(colliders, roadCollider).Equals(-1) && colliders.Length < 3)
        { 
            Invoke("DestroyCar",0.5f);
        }

    }
    private void DestroyCar()
    {
        if (Array.IndexOf(colliders, roadCollider).Equals(-1))
        {
            Destroy(gameObject);
        }
    }
    private void MoveCar()
    {
        if (transform.rotation.y == 0)
        {
            speed = new Vector3(0f, 0f, 0.75f*PlayerMovment.currentSpeed);
        }
        else
        {
            speed = new Vector3(0f, 0f, -0.75f * PlayerMovment.currentSpeed);
        }
        enemyCarRB.velocity = speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("car"))
        {
            Instantiate(boom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
