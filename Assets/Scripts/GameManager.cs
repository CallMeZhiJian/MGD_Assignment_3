using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Reference for blocks
    public GameObject blockPrefab;
    private GameObject blockSpawned;
    private Rigidbody blockRB;

    //Moving Position
    public GameObject movePositionParent;
    public GameObject[] movePosLeft;
    public GameObject[] movePosRight;

    //Indicators
    private float startTime;
    private float endTime;
    private float touchTime;

    public bool changeMovePos;
    private bool changeDirection;

    public static bool isDropped;
    public static bool isSpawned;

    public float speed = 2;
    public int score;
    public int highScore;

    private void Start()
    {
        isDropped = false;
        changeMovePos = true;
        score = 0;

        SpawnBlock();
    }

    private void Update()
    {
        if (isSpawned)
        {
            MoveBlock();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;

                touchTime = endTime - startTime;
            }

            if (touchTime < 0.5f && !isDropped)
            {
                DropBlock();
                isDropped = true;
            }
        }
    }

    public void SpawnBlock()
    {
        movePositionParent.transform.position = new Vector3(movePositionParent.transform.position.x, movePositionParent.transform.position.y + 2, movePositionParent.transform.position.z);

        if (changeMovePos)
        {
            Debug.Log("Spawn Left");
            Vector3 pos = new Vector3(movePosLeft[0].transform.position.x, movePositionParent.transform.position.y, movePosLeft[0].transform.position.z);
            blockSpawned = Instantiate(blockPrefab, pos, Quaternion.identity); 
        }
        else
        {
            Debug.Log("Spawn Right");
            Vector3 pos = new Vector3(movePosRight[0].transform.position.x, movePositionParent.transform.position.y, movePosRight[0].transform.position.z);
            blockSpawned = Instantiate(blockPrefab, pos, Quaternion.identity);
        }

        blockSpawned.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

        blockRB = blockSpawned.GetComponent<Rigidbody>();
        blockRB.useGravity = false;

        isSpawned = true;
    }

    public void MoveBlock()
    {
        if (blockSpawned.transform.position.x <= movePosLeft[0].transform.position.x || blockSpawned.transform.position.z >= movePosRight[0].transform.position.z)
        {
            changeDirection = true;
        }
        else if (blockSpawned.transform.position.x >= movePosLeft[1].transform.position.x || blockSpawned.transform.position.z <= movePosRight[1].transform.position.z)
        {
            changeDirection = false;
        }

        if (changeMovePos)
        {
            Mathf.Clamp(blockSpawned.transform.position.x, movePosLeft[0].transform.position.x, movePosLeft[1].transform.position.x);

            if (changeDirection)
            {
                blockSpawned.transform.position = new Vector3(blockSpawned.transform.position.x + speed * Time.deltaTime, blockSpawned.transform.position.y, blockSpawned.transform.position.z);
            }
            else
            {
                blockSpawned.transform.position = new Vector3(blockSpawned.transform.position.x - speed * Time.deltaTime, blockSpawned.transform.position.y, blockSpawned.transform.position.z);
            }
        }
        else
        {
            Mathf.Clamp(blockSpawned.transform.position.z, movePosRight[1].transform.position.z, movePosRight[0].transform.position.z);

            if (changeDirection)
            {
                blockSpawned.transform.position = new Vector3(blockSpawned.transform.position.x, blockSpawned.transform.position.y, blockSpawned.transform.position.z - speed * Time.deltaTime);
            }
            else
            {
                blockSpawned.transform.position = new Vector3(blockSpawned.transform.position.x, blockSpawned.transform.position.y, blockSpawned.transform.position.z + speed * Time.deltaTime);
            }
        }

       
    }

    public void DropBlock()
    {
        Debug.Log("Dropped");
        blockRB.useGravity = true;
        blockSpawned = null;

        isSpawned = false;

        if (changeMovePos)
        {
            changeMovePos = false;
        }
        else
        {
            changeMovePos = true;
        } 
    }

    public void AddScore(GameObject block)
    {
        if (!changeMovePos)
        {
            Debug.Log("X");
            if(block.transform.position.x <= 0.1 && block.transform.position.x >= -0.1)
            {
                score += 2;
            }
            else
            {
                score++;
            }
        }
        else
        {
            Debug.Log("Z");
            if(block.transform.position.z <= 0.1 && block.transform.position.z >= -0.1)
            {
                score += 2;
            }
            else
            {
                score++;
            }
        }
    }
}