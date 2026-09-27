using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using Unity.Mathematics;

public class PlayerManager : MonoBehaviour
{

    public WalkietalkieManager WalkietalkieManager;
    public CameraSwitch CameraSwitch;
    NightManager NightManagerScript;
    public GameObject cameraSwitch;
    
    public Image Fainting;

    public float Oxygen = 100; 
    public LeftVentLever LeftVentLever;
    private float OxygenLoss = .25f;
    private float OxygenRegain = .75f;

    public VideoPlayer WinScreenVP;

    public AudioSource AudioSource;
    public AudioClip JumpscareClip;
    public AudioClip GameOverMusic;
    public AudioClip SuffocationClip;

    public GameObject SFX;

    public GameObject AlwaysDeath;
    public GameObject SometimesDeath;
    public GameObject KDeath;
    public GameObject AsDeath;
    public GameObject ArDeath;
    public GameObject SDeath;
    public GameObject MDeath;
    public GameObject ODeath;
    public GameObject Stasis;
    public GameObject BlackSquare;
    public GameObject win;
    public GameObject sixwin;
    public GameObject winvp;
    public GameObject sixwinvp;

    public GameObject ambsource;

    public GameObject Kitty;
    public GameObject Mantis;
    public GameObject Slime;
    public GameObject Astro;
    public GameObject Arma;

    public int targetFPS = 120;

    public float Hour;

    bool OxygenDeathCalled;

    // 1 Suffocation, 2 Mantis, 3 Slime, 4 Kitty, 5 Armadillo, 6 Astronaut
    public int DeathCause;

    void Start()
    {
        OxygenDeathCalled = false;
        ambsource.SetActive(true);
        SFX.SetActive(true);
        GameObject NightManager = GameObject.FindWithTag("NightManager");
        sixwin.SetActive(false);
        win.SetActive(false);
        winvp.SetActive(false);
        sixwinvp.SetActive(false);
        BlackSquare.SetActive(false);
        Oxygen = 100;
        AlwaysDeath.SetActive(false);
        SometimesDeath.SetActive(true);
        KDeath.SetActive(false);
        AsDeath.SetActive(false);
        ArDeath.SetActive(false);
        SDeath.SetActive(false);
        MDeath.SetActive(false);
        ODeath.SetActive(false);
        Stasis.SetActive(false);
        StartCoroutine(OxygenCheck());
        StartCoroutine(FirstHour());
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
 
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


    IEnumerator HourCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(30);

            Hour++;

            if (Hour == 12)
            {
                WinUI();
            }

            yield return null;
        }
    }

    IEnumerator FirstHour()
    {
        Hour = 12;
        yield return new WaitForSeconds(30);

        Hour = 1;

        StartCoroutine(HourCor());
        
        yield return null;
       
    }

    IEnumerator OxygenCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(OxygenLoss);
            if (LeftVentLever.LeftVentShut)
            {
                Oxygen = Oxygen - 1;
            }
            else if (Oxygen < 100)
            {
                Oxygen = Oxygen + OxygenRegain;
            }
        }
    }

    void OxygenDeath()
    {
        if (!OxygenDeathCalled)
        {
            DeathCause = 1;
            AudioSource.clip = SuffocationClip;
            AudioSource.Play();
            StartCoroutine(DeathCor());
        }
        
    }

    void Update()
    {       
        Color color = Fainting.color;
        color.a = Mathf.Pow(1 - Oxygen / 100, 8);
        Fainting.color = color;
        if (Oxygen <= 0)
        {
            OxygenDeath();
            OxygenDeathCalled = true;
        }
    }

    public void died()
    {
        WalkietalkieManager.Mute();
        StartCoroutine(DeathCor());
        CameraSwitch.JumpscareCam();
        AudioSource.clip = JumpscareClip;
        AudioSource.Play();
        cameraSwitch.SetActive(false);
    }

    IEnumerator DeathCor()
    {
        yield return new WaitForSeconds(1.5f);
        BlackSquare.SetActive(true);
        yield return new WaitForSeconds(.3f);
        DeathUI();
        BlackSquare.SetActive(false);
    }

    public void DeathUI()
    {
        ambsource.SetActive(false);
        Mantis.SetActive(false);
        Kitty.SetActive(false);
        Slime.SetActive(false);
        Astro.SetActive(false);
        Arma.SetActive(false);
        AlwaysDeath.SetActive(true);
        AudioSource.clip = GameOverMusic;
        AudioSource.Play();
        if (DeathCause == 1)
        {
            ODeath.SetActive(true);
        }
        else if (DeathCause == 2)
        {
            MDeath.SetActive(true);
            Stasis.SetActive(true);
        }
        else if (DeathCause == 3)
        {
            SDeath.SetActive(true);
            Stasis.SetActive(true);
        }
        else if (DeathCause == 4)
        {
            KDeath.SetActive(true);
            Stasis.SetActive(true);
        }
        else if (DeathCause == 5)
        {
            ArDeath.SetActive(true);
            Stasis.SetActive(true);
        }
        else if (DeathCause == 6)
        {
            AsDeath.SetActive(true);
        }
    }

    public void WinUI()
    {
        NightManagerScript = GameObject.FindGameObjectWithTag("NightManager").GetComponent<NightManager>();
        SFX.SetActive(false);
        Mantis.SetActive(false);
        Kitty.SetActive(false);
        Slime.SetActive(false);
        Astro.SetActive(false);
        Arma.SetActive(false);
        CameraSwitch.JumpscareCam();
        cameraSwitch.SetActive(false);
        if (NightManager.Night < 6)
        {
            winvp.SetActive(true);
            NightManagerScript.Win();
            WinScreenVP.frame = 0;
            win.SetActive(true);
        }
        else if (NightManager.Night == 6)
        {
            NightManagerScript.Win();
            sixwinvp.SetActive(true);
            sixwin.SetActive(true);
        }
 
    }
}
