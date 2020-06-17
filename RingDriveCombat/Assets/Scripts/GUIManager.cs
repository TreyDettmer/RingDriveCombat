using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour {

    // Use this for initialization
    public TextMeshProUGUI waveCounterGUI;
    public TextMeshProUGUI pointCounterGUI;
    public TextMeshProUGUI waveAnouncementGUI;
    public TextMeshProUGUI waveAnouncementPointsGUI;
    public TextMeshProUGUI ammoCounterGUI;
    public Image weaponHighlightSquare;
    public Image jumpCooldownBar;
    public Image jumpCooldownBack;
    public GameObject jumpCooldownWarning;
    public Image gameOverPanel;
    public Image winGamePanel;
    public TextMeshProUGUI finalWaveText;
    public TextMeshProUGUI gameFeed;
    public TextMeshProUGUI finalScoreText;
    public Image pauseMenuPanel;
    public Image settingsMenuPanel;
    public Image reticle;
    public Image powerupImage;
    public GameObject baseGameUI;
    public GameObject instructionsScreen;
    public TextMeshProUGUI winningScoreText;
    public bool bJumpCooldown = false;
    public float waveAnnouncmentDuration = 3f;
    Coroutine gameFeedCoroutine;

    void Start() {
        jumpCooldownBack.enabled = false;
        jumpCooldownBar.enabled = false;
        jumpCooldownWarning.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        pauseMenuPanel.gameObject.SetActive(false);
        StartCoroutine(DisplayWaveNumberCoroutine());
        
    }

    // Update is called once per frame
    void Update() {

    }


    public void ShowPauseMenu(bool pause)
    {
        if (pause)
        {
            baseGameUI.SetActive(false);
            pauseMenuPanel.gameObject.SetActive(true);
        }
        else
        {
            
            pauseMenuPanel.gameObject.SetActive(false);
            baseGameUI.SetActive(true);
        }
    }

    public void OnPlayAgainClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ShowGameOverGUI(int wave, int points)
    {
        finalWaveText.text = "Final Wave: " + wave.ToString();
        finalScoreText.text = "Score: " + points.ToString();
        gameOverPanel.gameObject.SetActive(true);
    }

    public void ShowWinGameGUI(int points)
    {
        winningScoreText.text = "Final Score: " + points.ToString();
        winGamePanel.gameObject.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        
        FindObjectOfType<AudioManager>().StopMusic();
        FindObjectOfType<AudioManager>().StopSoundEffects();
        FindObjectOfType<AudioManager>().PlayMusic(0);
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }

    public void GoToInstructions()
    {
        pauseMenuPanel.gameObject.SetActive(false);
        instructionsScreen.SetActive(true);
    }

    public void GoToSettings()
    {
        pauseMenuPanel.gameObject.SetActive(false);
        settingsMenuPanel.gameObject.SetActive(true);
        GameObject.Find("_app").GetComponent<SettingsScript>().hasApplied = false;
       
    }

    public void UpdateGameFeed(string message)
    {
        if (gameFeedCoroutine != null)
        {
            StopCoroutine(gameFeedCoroutine);
        }
        gameFeedCoroutine = StartCoroutine(gameFeedMessageTime());
        
        gameFeed.text = message;
        
    }

    public void GoBackToPauseMenu()
    {
        
        settingsMenuPanel.gameObject.SetActive(false);
        instructionsScreen.SetActive(false);
        pauseMenuPanel.gameObject.SetActive(true);
        
    }

    public IEnumerator gameFeedMessageTime()
    {
        yield return new WaitForSeconds(3f);
        gameFeed.text = "";
    }

    public IEnumerator JumpCoolDown(float time)
    {
        
        jumpCooldownBack.enabled = true;
        jumpCooldownBar.enabled = true;
        jumpCooldownWarning.SetActive(true);
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            while (LevelManager.bPaused)
            {
                yield return null;
            }
            i += Time.deltaTime * rate;
            jumpCooldownBar.GetComponent<RectTransform>().localScale = new Vector3(1f, i, 1f);

            yield return null;
        }
        jumpCooldownBack.enabled = false;
        jumpCooldownBar.enabled = false;
        jumpCooldownWarning.SetActive(false);

    }
    

    public void UpdateAmmoCounter(int ammo)
    {
        ammoCounterGUI.text = "Ammo: " + ammo.ToString();
        if (ammo <= 3 && ammo != -1)
        {
            ammoCounterGUI.color = Color.red;
        }
        else
        {
            ammoCounterGUI.color = Color.white;
        }
    }

    public void UpdateStatGUI(int waveCount, int pointCount)
    {
        waveCounterGUI.text = "Wave " + waveCount.ToString();
        waveAnouncementGUI.text = "Wave " + waveCount.ToString();
        pointCounterGUI.text = pointCount.ToString();
        waveAnouncementPointsGUI.text = "Points: " + pointCount.ToString();
    }

    public void SwitchWeapons(int choice)
    {
        if (choice == 1)
        {
            weaponHighlightSquare.GetComponent<RectTransform>().anchoredPosition = new Vector2(345f, 165f);
            reticle.color = new Color(1f, 0.615f, 0f);

        }
        else if (choice == 0)
        {
            weaponHighlightSquare.GetComponent<RectTransform>().anchoredPosition = new Vector2(275f, 165);
            reticle.color = Color.white;
        }
    }

    public void DisplayPowerupImg(bool _display)
    {
        if (_display)
        {
            powerupImage.GetComponent<Image>().enabled = true;
        }
        else
        {
            powerupImage.GetComponent<Image>().enabled = false;
        }
    }

    public void DisplayWaveNumber()
    {
        StartCoroutine(DisplayWaveNumberCoroutine());
    }

    IEnumerator DisplayWaveNumberCoroutine()
    {
        waveAnouncementGUI.enabled = true;
        waveAnouncementPointsGUI.enabled = true;
        yield return new WaitForSeconds(waveAnnouncmentDuration);
        waveAnouncementGUI.enabled = false;
        waveAnouncementPointsGUI.enabled = false;
    }

    public IEnumerator CoinPointsAnimation(float time)
    {
        float i = 0.0f;
        float rate = 1.0f / (time/2f);
        float initialFontSize = pointCounterGUI.fontSize;
        while (i < 1.0f)
        {
            while (LevelManager.bPaused)
            {
                yield return null;
            }
            pointCounterGUI.fontSize = Mathf.Lerp(initialFontSize, initialFontSize + 7, i);
            i += Time.deltaTime * rate;
            yield return null;
        }
        StartCoroutine(CoinPointsAnimation2(time, initialFontSize));
    }
    IEnumerator CoinPointsAnimation2(float time,float goalSize)
    {
        float i = 0.0f;
        float rate = 1.0f / (time/2f);
        float initialFontSize = pointCounterGUI.fontSize;
        while (i < 1.0f)
        {
            while (LevelManager.bPaused)
            {
                yield return null;
            }
            pointCounterGUI.fontSize = Mathf.Lerp(initialFontSize, goalSize, i);
            i += Time.deltaTime * rate;
            yield return null;
        }
    }


}
