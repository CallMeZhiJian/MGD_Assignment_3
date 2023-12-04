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
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            if (!GameManager.isSpawned)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 1.8f, Camera.main.transform.position.z);
                gameManager.speed += 0.5f;
                gameManager.scale -= 0.05f;
                gameManager.SpawnBlock();
                gameManager.AddScore(gameObject);
            }

            gameManager.CineCam.m_LookAt = gameManager.blockPrev.transform;
            gameManager.CineCam.m_Follow = gameManager.blockPrev.transform;

            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            gameManager.ShowResult();
        }
    }
}
