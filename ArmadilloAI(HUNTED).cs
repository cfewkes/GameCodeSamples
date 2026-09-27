using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloAI : MonoBehaviour
{
    public LeftVentLever LeftVentLever;
    public CameraSwitch CameraSwitch;
    public PlayerManager PlayerManager;
    public NightManager NightManager;

    public Animator ArmaAnimator;

    public AudioClip BBJ;

    public AudioSource AudioSource;

    public float minmovetime = 15;
    public float maxmovetime = 20;

    public GameObject desk;

    public int minmoveroll = 1;
    public int maxmoveroll = 20;
    public int AILevel = 5;
    public int Stage = 1;
    public bool CoroutineRunning = false;
    public bool CAKRunning = false;


    void Update()
    {
        if (CameraSwitch.CurrentCam == 3 & CameraSwitch.CamOn)
        {
            CoroutineRunning = false;
            StopCoroutine(TimeToMove());
        }
        else if (!CoroutineRunning)
        {
            StartCoroutine(TimeToMove());
        }
    }

    IEnumerator TimeToMove()
    {
        if (!CoroutineRunning)
        {
            CoroutineRunning = true;

            float RandomWait = Random.Range(minmovetime, maxmovetime);

            yield return new WaitForSeconds(RandomWait);

        
            if (CoroutineRunning)
            {
                CheckMove();
            }
            CoroutineRunning = false;
            yield break;
        
        }

    }

    IEnumerator CheckArmadilloKill()
    {
        if (!CAKRunning)
        {
            CAKRunning = true;
            Stage = 5;
            ArmadilloMove();
            float ExtraWait = Random.Range(4, 5);

            yield return new WaitForSeconds(ExtraWait);

            if (LeftVentLever.LeftVentShut)
            {
                AudioSource.clip = BBJ;
                AudioSource.Play();
                Stage = 1;
                ArmadilloMove();
                CAKRunning = false;
                yield break;
            }

            yield return new WaitForSeconds(ExtraWait);

            if (!LeftVentLever.LeftVentShut)
            {
                ArmadilloKill();
            }
            else
            {
                AudioSource.clip = BBJ;
                AudioSource.Play();
                Stage = 1;
                ArmadilloMove();
                StopCoroutine(TimeToMove());
                StartCoroutine(TimeToMove());
            }
            CAKRunning = false;
            yield break;
        }


    }

    void CheckMove()
    {
        int RandomMoveRoll = Random.Range(minmoveroll, maxmoveroll);

        if (AILevel >= RandomMoveRoll)
        {
            ArmadilloMovement();
        }
    }

    void ArmadilloMovement()
    {
        if (Stage <= 2)
        {
            Stage = Stage + 1;
            ArmadilloMove();
        }
        else if (Stage == 3)
        {
            StartCoroutine(CheckArmadilloKill());
        }
        else if (Stage > 5)
        {
            Stage = 1;
            ArmadilloMove();
        }
    }

    void ArmadilloKill()
    {
        Stage = 4;
        ArmadilloMove();
        PlayerManager.DeathCause = 5;
        PlayerManager.died();
        desk.SetActive(false);
    }

    void ArmadilloMove()
    {
        CameraSwitch.SmthMoved();
        if (Stage == 1)
        {
            transform.position = new Vector3(129.3f, 2.99f, 10.68f);
            transform.rotation = Quaternion.Euler(0f, 180f, 90f);
            ArmaAnimator.SetInteger("stage", 1);
        }
        else if (Stage == 2)
        {
            transform.position = new Vector3(123.6f, 1.08f, 28.88f);
            transform.rotation = Quaternion.Euler(0f, 34.62f, 0f);
            ArmaAnimator.SetInteger("stage", 2);
        }
        else if (Stage == 3)
        {
            transform.position = new Vector3(140.99f, 1.08f, 49.38f);
            transform.rotation = Quaternion.Euler(0f, 34.62f, 0f);
            ArmaAnimator.SetInteger("stage", 3);
        }
        else if (Stage == 4)
        {
            transform.position = new Vector3(15.36f, 16.7f, 173.22f);
            transform.rotation = Quaternion.Euler(-15f, 0f, 0f);
            ArmaAnimator.SetInteger("stage", 4);
        }
        else if (Stage == 5)
        {
            transform.position = new Vector3(15.36f, 160.7f, 173.22f);
            transform.rotation = Quaternion.Euler(-15f, 0f, 0f);
        }

    }

    void NightAICheck()
    {
        if (NightManager.Night == 1)
        {
            gameObject.SetActive(false);
        }
        else if (NightManager.Night == 2)
        {
            AILevel = 4;
        }
        else if (NightManager.Night == 3)
        {
            AILevel = 5;
        }
        else if (NightManager.Night == 4)
        {
            AILevel = 6;
        }
        else if (NightManager.Night == 5)
        {
            AILevel = 7;
        }
        else if (NightManager.Night == 6)
        {
            AILevel = 20;
        }
    }

    void Start()
    {
        GameObject NightManager = GameObject.FindWithTag("NightManager");
        NightAICheck();
        Stage = 1;
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(TimeToMove());
        }
        ArmadilloMove();
        minmovetime = 10;
        maxmovetime = 20;
}
}
