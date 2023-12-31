using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameManager : UIManager
{
    public CinemachineVirtualCamera CineCam;
    public RewardedAdsButton rewardedAdsButton;

    //Reference for blocks
    public GameObject blockPrefab;
    private GameObject blockSpawned;
    [HideInInspector] public GameObject blockPrev;
    [HideInInspector] public GameObject blockPrev2;
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

    [HideInInspector] public float speed = 2;
    [HideInInspector] public float scale = 3;
    private int score;
    public static int highScore;
    private int blockCount;
    public static int streakCount;

    public static bool isRewarded;

    //UI stuffs
    public TextMeshProUGUI scoreTextInGame;
    public TextMeshProUGUI scoreTextResult;
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        CineCam = GameObject.Find("CineCam").GetComponent<CinemachineVirtualCamera>();

        blockPrev2 = GameObject.Find("StartingBlock");

        SoundButton = GameObject.Find("SoundButton");
        PauseButton = GameObject.Find("PauseButton");
        PauseScreen = GameObject.Find("PauseScreen");
        ResultScreen = GameObject.Find("ResultScreen");
        RewardScreen = GameObject.Find("RewardScreen");

        PauseScreen.SetActive(false);
        ResultScreen.SetActive(false);
        RewardScreen.SetActive(false);

        Time.timeScale = 1;

        isDropped = false;
        changeMovePos = true;
        isRewarded = false;
        
        blockCount = 0;
        streakCount = 0;
        speed = 2;
        scale = 3;
        score = 0;

        scoreTextInGame.text = score.ToString();

        SpawnBlock();

        AudioManager.instance.PlayBGM();

        if (Advertisement.isInitialized)
        {
            rewardedAdsButton.LoadAd();
        }
    }

    private void Update()
    {
        if (AudioManager.instance._BGMSource.mute || AudioManager.instance._SFXSource.mute)
        {
            SoundButton.GetComponent<Image>().sprite = SoundMute;
        }
        else
        {
            SoundButton.GetComponent<Image>().sprite = SoundOn;
        }

        if (isSpawned)
        {
            MoveBlock();
        }

        var touchArea = new Rect(0, 0, Screen.width, Screen.height * 0.8f);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touchArea.Contains(touch.position) && !isPaused)
            {
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
    }

    public void SpawnBlock()
    {
        if (streakCount >= 3)
        {
            Mathf.Clamp(scale, 0.5f, 3f);
            scale += 0.1f;
        }

        blockCount++;

        if (blockPrev != null)
        {
            movePositionParent.transform.position = new Vector3(blockPrev.transform.position.x, movePositionParent.transform.position.y + scale, blockPrev.transform.position.z);
        }
        else
        {
            movePositionParent.transform.position = new Vector3(movePositionParent.transform.position.x, movePositionParent.transform.position.y + scale, movePositionParent.transform.position.z);
        }
        
        
        if (changeMovePos)
        {
            Vector3 pos = new(movePosLeft[0].transform.position.x, movePositionParent.transform.position.y, movePosLeft[0].transform.position.z);
            blockSpawned = Instantiate(blockPrefab, pos, Quaternion.identity); 
        }
        else
        {
            Vector3 pos = new(movePosRight[0].transform.position.x, movePositionParent.transform.position.y, movePosRight[0].transform.position.z);
            blockSpawned = Instantiate(blockPrefab, pos, Quaternion.identity);
        }

        blockSpawned.name = "Block" + blockCount;
        
        blockSpawned.GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);

        blockRB = blockSpawned.GetComponent<Rigidbody>();
        blockRB.useGravity = false;

        isSpawned = true;
        isDropped = false;
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
        blockRB.useGravity = true;
        if (blockPrev != null)
        {
            blockPrev2 = blockPrev;
        }
       
        blockPrev = blockSpawned;
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
        AudioManager.instance.PlaySFX("Score");
        
        float distanceToMiddle;        

        if (!changeMovePos)
        {
            if (blockPrev2 != null)
            {
                distanceToMiddle = block.transform.position.x - blockPrev2.transform.position.x;
            }
            else
            {
                distanceToMiddle = block.transform.position.x - 0;
            }
            

            if (distanceToMiddle <= 0.15 && distanceToMiddle >= -0.15)
            {
                AudioManager.instance.PlaySFX("BonusScore");
                score += 2;
                streakCount++;
            }
            else
            {
                score++;
                streakCount = 0;
            }
        }
        else
        {
            if (blockPrev2 != null)
            {
                distanceToMiddle = block.transform.position.z - blockPrev2.transform.position.z;
            }
            else
            {
                distanceToMiddle = block.transform.position.z - 0;
            }

            if (distanceToMiddle <= 0.15 && distanceToMiddle >= -0.15)
            {
                AudioManager.instance.PlaySFX("BonusScore");
                score += 2;
                streakCount++;
            }
            else
            {
                score++;
                streakCount = 0;
            }
        }

        if(score >= highScore)
        {
            highScore = score;
        }

        scoreTextInGame.text = score.ToString();
    }

    public void ShowResult()
    {
        RewardScreen.SetActive(false);
        ResultScreen.SetActive(true);

        scoreTextResult.text = "Score:\n" + score;
        highScoreText.text = "High\nScore:\n" + highScore;
    }
    
    public void ShowReward()
    {
        RewardScreen.SetActive(true);
    }

    public void WatchAdsRewards()
    {
        RewardScreen.SetActive(false);

        Destroy(blockPrev.gameObject);

        blockPrev = blockPrev2;
        
        movePositionParent.transform.position = new Vector3(blockPrev.transform.position.x, movePositionParent.transform.position.y - scale, blockPrev.transform.position.z);
        SpawnBlock();
    }
}