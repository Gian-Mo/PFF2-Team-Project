using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpause();
    }
    public void Heal()
    {
        GameManager.instance.playerScript.HP += 3;
        GameManager.instance.playerScript.isHealing = true;
        GameManager.instance.playerScript.updatePlayerUI();
        GameManager.instance.stateUnpause();
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SpeedUpgrade()
    {
        GameManager.instance.playerScript.speed += 1;
        GameManager.instance.playerScript.isSpeedBoosting = true;
        GameManager.instance.stateUnpause();
    }

    public void ClickOKToContinue()
    {
        GameManager.instance.stateUnpause();
    }
    public void settings()
    {
        GameManager.instance.Settings();
    }
    public void back() // simply puts the menu back to the previous one
    {
        
    }

}
