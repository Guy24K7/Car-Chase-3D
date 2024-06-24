using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadManager : MonoBehaviour
{
    /// <summary>
    ///
    [SerializeField] GameObject[] roadTypes;
    [SerializeField] GameObject[] cars;
    [SerializeField] GameObject roadPrefab;
    [SerializeField] Transform playerPos; 
    /// </summary>
    /// <summary>
    /// 
    private float [] possibleXpos;
    private List<GameObject> roads;
    private int roadsCount;
    private float roadLength;
    private int startingRoads;
    private Mesh planeMesh;
    private Bounds bounds;
    /// </summary>
    void Start()
    {
        possibleXpos =new float []{-2.5f,0,2.5f };
        startingRoads = 5;
        roads = new List<GameObject>();
        roadsCount = 0;
        planeMesh = roadPrefab.GetComponent<MeshFilter>().sharedMesh;
        bounds = planeMesh.bounds;
        roadLength = roadPrefab.transform.localScale.y * bounds.size.y;
        for (int i = 0; i < startingRoads; i++)
        {
            SpawnRoad(Random.Range(0, roadTypes.Length));
        }
        InvokeRepeating("SpawnCars", 5f, 4f);
    }
    // Update is called once per frame
    void Update()
    {
        SpawnRoadRate();
    }
    private void SpawnRoadRate()
    {
        if (playerPos.position.z - (roadLength+5) > roadsCount*roadLength - (startingRoads*roadLength))
        {
            SpawnRoad(Random.Range(0, roadTypes.Length));
            DeleteRoad();
        }
    }
    public void SpawnRoad(int roadTypeIndex)
    {
        GameObject temp = Instantiate(roadTypes[roadTypeIndex],transform.forward*roadLength* roadsCount, Quaternion.identity);
        roads.Add(temp);
        roadsCount++;
    }
    private void SpawnCars()
    {
        GameObject temp = Instantiate(cars[Random.Range(0, cars.Length)],new Vector3(possibleXpos[Random.Range(0, possibleXpos.Length)], 0.2f, RandomZInForthRoad()), Quaternion.identity);
        if (Mathf.Round(temp.transform.position.z) % 2 == 0)
        {
            temp.transform.Rotate(0, 180f,0);
        }
        else
        {
            temp.transform.Rotate(0, 0, 0);
        }
    }
    private float RandomZInForthRoad()
    {
        return Random.Range(roads[roads.Count - 2].transform.position.z, roads[roads.Count - 2].transform.position.z + 31f);
    }
    private void DeleteRoad()
    {
        Destroy(roads[0]);
        roads.RemoveAt(0);
    }
}
