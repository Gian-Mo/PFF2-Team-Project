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
    [SerializeField] GameObject menuGameInstructions;

    [SerializeField] TMP_Text gameGoalCountText;

    public Image playerHPBar;
    public GameObject heavySpell;
    public GameObject slowSpell;
    public GameObject playerFlashScreen;
    public GameObject player;
    public playerController playerScript;
    public bool isPaused;



    float timeScaleOrig;

    public int gameGoalCount;

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
}