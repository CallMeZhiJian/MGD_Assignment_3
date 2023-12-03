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
    private bool changeMovePos;
    private bool changeDirection;
    public static bool isDropped;
    public static bool isSpawned;
    static float t = 0;

    private void Start()
    {
        isDropped = false;

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
            Vector3 pos = new Vector3(movePosLeft[0].transform.position.x, movePositionParent.transform.position.y, movePosLeft[0].transform.position.z);
            blockSpawned = Instantiate(blockPrefab, pos, Quaternion.identity);
            
        }
        else
        {
            Vector3 pos = new Vector3(movePosRight[0].transform.position.x, movePositionParent.transform.position.y, movePosRight[0].transform.position.z);
            blockSpawned = Instantiate(blockPrefab, pos, Quaternion.identity);
            Debug.Log("Z: " + blockSpawned.transform.position.z);
        }

        blockRB = blockSpawned.GetComponent<Rigidbody>();
        blockRB.useGravity = true;
        blockRB.constraints = RigidbodyConstraints.None;
        blockRB.constraints = RigidbodyConstraints.FreezeRotation;

        isSpawned = true;
    }

    public void MoveBlock()
    {
        if (changeMovePos)
        {
            Mathf.Clamp(blockSpawned.transform.position.x, movePosLeft[0].transform.position.x, movePosLeft[1].transform.position.x);

            if (changeDirection)
            {
                blockSpawned.transform.position = new Vector3(Mathf.Lerp(movePosLeft[0].transform.position.x, movePosLeft[1].transform.position.x, t), 0, 0);
                t += 1.0f * Time.deltaTime;
            }
            else
            {
                blockSpawned.transform.position = new Vector3(Mathf.Lerp(movePosLeft[1].transform.position.x, movePosLeft[0].transform.position.x, t), 0, 0);
                t -= 1.0f * Time.deltaTime;
            }
        }
        else
        {
            Mathf.Clamp(blockSpawned.transform.position.z, movePosRight[1].transform.position.x, movePosRight[0].transform.position.x);

            if (changeDirection)
            {
                blockSpawned.transform.position = new Vector3(0, 0, Mathf.Lerp(movePosRight[0].transform.position.x, movePosRight[1].transform.position.x, t));
                t -= 1.0f * Time.deltaTime;
            }
            else
            {
                blockSpawned.transform.position = new Vector3(0, 0, Mathf.Lerp(movePosRight[1].transform.position.x, movePosRight[0].transform.position.x, t));
                t += 1.0f * Time.deltaTime;
            }
        }

        if(blockSpawned.transform.position.x == movePosLeft[0].transform.position.x || blockSpawned.transform.position.z == movePosRight[0].transform.position.x)
        {
            changeDirection = true;

            if (changeMovePos)
            {
                t = movePosLeft[0].transform.position.x;
            }
            else
            {
                t = movePosRight[0].transform.position.z;
            }
        }
        else if(blockSpawned.transform.position.x == movePosLeft[1].transform.position.x || blockSpawned.transform.position.z == movePosRight[1].transform.position.z)
        {
            changeDirection = false;

            if (changeMovePos)
            {
                t = movePosLeft[1].transform.position.x;
            }
            else
            {
                t = movePosRight[1].transform.position.z;
            }
        }
    }

    public void DropBlock()
    {
        blockRB.useGravity = false;

        blockRB.constraints = RigidbodyConstraints.FreezePositionX;
        blockRB.constraints = RigidbodyConstraints.FreezePositionZ;
    }
}
