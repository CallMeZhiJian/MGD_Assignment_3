using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Spawned");
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 1, Camera.main.transform.position.z);
            GameManager.isDropped = false;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (!GameManager.isSpawned)
            {
                gameManager.SpawnBlock();
            }      
        }
        else if (collision.collider.CompareTag("Finish"))
        {
            Debug.Log("Lose, On result");
        }
    }
}
