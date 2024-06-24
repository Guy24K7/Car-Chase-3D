using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;
    GameObject existingStar;
    GameObject[] allObstacles;
    private Transform randomPosition;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        allObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        if (allObstacles.Length > 0 && GameObject.FindWithTag("Star") == null && !PlayerMovment.starOn)
        {
            randomPosition = allObstacles[Random.Range(0, allObstacles.Length)].transform;
            existingStar = Instantiate(starPrefab, new Vector3(randomPosition.position.x, randomPosition.position.y, randomPosition.transform.position.z - 2f), Quaternion.identity);
            existingStar.transform.Rotate(55f, 90f, 90f, Space.World);
        }
        if (randomPosition.IsDestroyed() && existingStar != null)
        {
            Destroy(existingStar);
        }
        if (existingStar != null)
        {
            existingStar.transform.Rotate(0, 1f, 0,Space.World);
        }
    }
}
