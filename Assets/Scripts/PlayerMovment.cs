using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private CharacterController playerController;
    private Vector3 direction;
    private float forwardSpeed;
    public static float currentSpeed;
    private int MAX_PRESSES = 300;
    private int currentPresses;
    private Vector3 targetPosition;
    [SerializeField] int currentLane;//left - 0, middle - 1, right - 2;
    private float distanceBetweenLanes;
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] bool canJump;
    [SerializeField] GameObject canvas;
    [SerializeField] int movementSmoothness;
    [SerializeField] GameObject carFire;
    [SerializeField] GameObject boom;
    private bool isDead;
    private bool CloseDodgeBufferOn;
    public static bool starOn;
    private int score;
    private int coolnessBonus;
    private TMP_Text scoreText;
    private TMP_Text coolnessBonusText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("score").GetComponent<TMP_Text>();
        coolnessBonusText = GameObject.Find("Coolness").GetComponent<TMP_Text>(); 
        CloseDodgeBufferOn = CloseDodgeCheck();
        starOn = false;
        currentPresses = 0;
        canJump = true;
        isDead = false;
        currentLane = 1;
        distanceBetweenLanes = 2.5f;
        gravity = -20f;
        forwardSpeed = 5f;
        playerController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (!isDead)
        {
            MovePlayer();
            ShowScore();
        }
    }
    //FixedUpdate is called according to the rate set by the editor
    private void FixedUpdate()
    {
        if (!isDead)
        {
            playerController.Move(direction * Time.fixedDeltaTime);
        }
    }
    private void ShowScore()
    {
        scoreText.text = "SCORE: " + score;
        coolnessBonusText.text = "COOLNESS: " + coolnessBonus;
    }
    private void MovePlayer()
    {
        currentSpeed = forwardSpeed + 1.5f * currentPresses;
        direction.z = currentSpeed;//keep moving forward
        MoveLeftOrRight();
        Jump();

    }
    private void MoveLeftOrRight()
    {
        targetPosition.z = transform.position.z;
        targetPosition.y = transform.position.y;
        if (Input.GetKeyDown(KeyCode.D) && currentLane <= 1)
        {
            currentLane++;
            if (currentPresses < MAX_PRESSES)
            {
                currentPresses++;
            }
            if (CloseDodgeCheck())
            {
                CloseDodgeBufferOn = true;
                score += 10;
                Invoke("CloseDodgeBonusBuffer", 3f - 0.01f * currentPresses);
            }
            targetPosition += Vector3.right * distanceBetweenLanes;
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentLane >= 1)
        {
            currentLane--;
            if (currentPresses < MAX_PRESSES)
            {
                currentPresses++;
            }
            if (CloseDodgeCheck())
            {
                CloseDodgeBufferOn = true;
                score += 10;
                Invoke("CloseDodgeBonusBuffer", 3f - 0.01f*currentPresses);
            }
            targetPosition += Vector3.left * distanceBetweenLanes;
        }
        if (transform.position.Equals(targetPosition))
        {
            return;
        }
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * movementSmoothness * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
        {
            playerController.Move(moveDir);
        }
        else
        {
            playerController.Move(diff);//ensures that the player doesn't go over where he was supposed to go
        }
    }
    private void Jump()
    {
        jumpSpeed = 10f + 0.0825f * currentPresses;
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            direction.y = jumpSpeed;
            canJump = false;
        }
        else if (!canJump)
        {
            direction.y += gravity * Time.deltaTime;
        }
        if (transform.position.y < 0.2f)
        {
            transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        }
    }
    private void StarPower()
    {
        starOn = !starOn;
    }
    private bool CloseDodgeCheck()
    {
        if (CloseDodgeBufferOn)
        {
            return false;
        }
        float closestObstacle =Mathf.Infinity;
        float closestCar = Mathf.Infinity;
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        Transform[] obstaclesPos = new Transform[obstacles.Length];
        Transform[] carsPos = new Transform[cars.Length];
        for (int i = 0; i < obstacles.Length; i++) { obstaclesPos[i] = obstacles[i].GetComponent<Transform>();}
        for (int i = 0;i < cars.Length; i++) { carsPos[i] = cars[i].GetComponent<Transform>();}
        foreach (Transform obstaclePos in  obstaclesPos) 
        {
            if(Mathf.Abs(obstaclePos.position.z - transform.position.z) < closestObstacle && obstaclePos.position.z > transform.position.z )
            {
                closestObstacle = Mathf.Abs(obstaclePos.position.z - transform.position.z);
            }
        }
        foreach (Transform carPos in carsPos)
        {
            if (Mathf.Abs(carPos.position.z - transform.position.z) < closestCar && carPos.position.z > transform.position.z)
            {
                closestCar = Mathf.Abs(carPos.position.z - transform.position.z);
            }
        }
        return closestObstacle - transform.position.z < 0.3f || closestCar - transform.position.z < 0.3f;

    }
    private void CloseDodgeBonusBuffer()
    {
        CloseDodgeBufferOn = !CloseDodgeBufferOn;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Obstacle") || collision.collider.tag.Equals("car"))
        {
            if (!starOn)
            {
                isDead = true;
                carFire.SetActive(true);
                canvas.SetActive(true);
            }
            else
            {
                Destroy(collision.gameObject);
                Instantiate(boom, transform.position, Quaternion.identity);
            }

        }
        if (collision.collider.name.StartsWith("Star"))
        {
            if (!starOn)
            {
                Invoke("StarPower", 5f);
                score += 50;
                coolnessBonus++;
            }
            Destroy(collision.gameObject);
            starOn = !starOn;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag.Equals("Road"))
        {
            canJump = true;
        }
    }
}