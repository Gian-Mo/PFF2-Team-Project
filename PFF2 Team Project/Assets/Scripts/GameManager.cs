using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuGameInstructions; 
    [SerializeField] GameObject WandUpgradeScreen;

    [SerializeField] TMP_Text gameGoalCountText; // keeps track of the player's kill count
    [SerializeField] TMP_Text gameHeightCountText;
    [SerializeField] WandStats medium;
    [SerializeField] WandStats epic;

    public Image playerHPBar;
    public GameObject heavySpell;
    public GameObject slowSpell;
    public GameObject playerFlashScreen;
    public GameObject player;
    public playerController playerScript;
    public bool isPaused;
    bool wandPopUp;

    float timeScaleOrig;
    float heightCounter;

    public int gameGoalCount;
    public float gameHeightCount;
    bool wandMax;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        instance = this;
        timeScaleOrig = Time.timeScale;

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();

        statePause();
        menuActive = menuGameInstructions;
        menuActive.SetActive(true);
        wandMax = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuPause.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }

        if (!wandMax)
        {
           
            PlayerUpgrade();

         
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        gameGoalCountText.text = gameGoalCount.ToString();
        wandPopUp = false;
    }

    public void updateHeightCounter(float amount)
    {
        gameHeightCount += amount;
        gameHeightCountText.text = gameHeightCount.ToString();
    }

    public void YouWin()
    {
        // you won
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void YouLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
    public void Settings()
    {
        statePause();
        menuActive.SetActive(false);
        statePause();
        menuActive = menuSettings;
        menuActive.SetActive(true);
    }

    public void FlashScreen(Color color)
    {
        color.a = 0.3f;
        playerFlashScreen.GetComponent<Image>().color = color;


        StartCoroutine(flashDamageScreen());
    }

    IEnumerator flashDamageScreen()
    {
        playerFlashScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerFlashScreen.SetActive(false);
    }


    void PlayerUpgrade()
    {

        if (gameGoalCount == 1 && !wandPopUp)
        {
            playerScript.wandInfo = medium;
            playerScript.SetWand();
             wandPopUp = true;
            StartCoroutine(wandUpgradePopUp());
        }
        else if (gameGoalCount == 3 && !wandPopUp)
        {
            playerScript.wandInfo = epic;
            playerScript.SetWand();
            StartCoroutine(wandUpgradePopUp());
            wandPopUp = true;
            wandMax = true;
        }
    }

    IEnumerator wandUpgradePopUp()
    {

      WandUpgradeScreen.SetActive(true);
        yield return new WaitForSeconds(3);
        WandUpgradeScreen.SetActive(false);
    }
    
}