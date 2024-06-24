using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform playerPos;//make use of player position
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localPosition = new Vector3(-playerPos.position.x, transform.localPosition.y,transform.localPosition.z);
    }
}
