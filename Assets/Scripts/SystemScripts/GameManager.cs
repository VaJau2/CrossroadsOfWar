using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    GameObject SettingsMenu;
    [SerializeField]
    GameObject AskQuitMenu;
    [SerializeField]
    GameObject HelpMenu;
    [SerializeField]
    GameObject StatisticMenu;

    [Header("Help")]
    [SerializeField]
    string[] helpText;
    [SerializeField]
    int tempTextNumber;
    [SerializeField]
    Text helpLabel;

    [Header("Music")]
    [SerializeField]
    AudioSource MusicSource;

    [SerializeField]
    Slider MusicVolume;
    [SerializeField]
    Slider SoundVolume;

    public float musicVolume;
    public float soundVolume;

    [Header("Statistic")]
    [SerializeField] //время в игре
    Text gameTimeText;
    int hours, minutes, seconds;
    public Text daysSurvivedText; //дней пережито
    [HideInInspector]
    public int daysSurvived;
    [SerializeField] //количество смертей
    Text DealthCountText;
    [SerializeField] //количество убийств
    Text KillsCountText;
    [HideInInspector]
    public int killsCount;
    [SerializeField] //убито тимбервульфов
    Text TimberKillsCountText;
    [HideInInspector]
    public int timberkillsCount;
    [SerializeField] //убито единорогов
    Text UniKillsCountText;
    [HideInInspector]
    public int unikillsCount;
    [SerializeField] //убито пегасов
    Text PegaKillsCountText;
    [HideInInspector]
    public int pegakillsCount;
    [SerializeField] //добыто древесины
    Text GotWoodCountText;
    [HideInInspector]
    public int gotWoodCount;
    [SerializeField] //потрачено древесины
    Text SpendWoodCountText;
    [HideInInspector]
    public int spendWoodCount;
    [SerializeField] //добыто еды
    Text GotFoodCountText;
    [HideInInspector]
    public int gotFoodCount;
    [SerializeField] //ОЗ получено едой
    Text HealsCountText;
    [HideInInspector]
    public int healsCount;
    [SerializeField] //Земнопони призвано в лагерь
    Text PoniesGotCountText;
    [HideInInspector]
    public int poniesGotCount;
    [SerializeField] //Убито юнитов в лагере
    Text UnitsKilledCountText;
    [HideInInspector]
    public int unitsKilledCount;

    [Header("Other")]
    public bool GamePaused;

    public int earthponiesCount;
    public int earthponiesMax;

    [SerializeField]
    TimeCount timeCount;
   
    [SerializeField]
    MovingController player;
    [SerializeField]
    public Text dealthText;

    float timerDealth = 1f;

    float tempTimeScale;

    IEnumerator gameTimeCount;

    IEnumerator GameTimeCount()
    {
        for (; ; )
        {
            if (seconds < 59) seconds++;
            else
            {
                seconds = 0;
                if (minutes < 59) minutes++;
                else
                {
                    hours++;
                    minutes = 0;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void ClickStatistic()
    {
        gameTimeText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        if (daysSurvived > timeCount.days) daysSurvivedText.text = daysSurvived.ToString();
        else daysSurvivedText.text = timeCount.days.ToString();
        KillsCountText.text = killsCount.ToString();
        TimberKillsCountText.text = timberkillsCount.ToString();
        UniKillsCountText.text = unikillsCount.ToString();
        PegaKillsCountText.text = pegakillsCount.ToString();
        GotWoodCountText.text = gotWoodCount.ToString();
        SpendWoodCountText.text = spendWoodCount.ToString();
        GotFoodCountText.text = gotFoodCount.ToString();
        HealsCountText.text = healsCount.ToString();
        PoniesGotCountText.text = poniesGotCount.ToString();
        UnitsKilledCountText.text = unitsKilledCount.ToString();

        StatisticMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void ClickHelp()
    {
        HelpMenu.SetActive(true);
        PauseMenu.SetActive(false);
        tempTextNumber = 0;
    }

    public void ClickHelpButton(int helpNum)
    {
        tempTextNumber = helpNum;
    }

    public void ClickSettings()
    {
        SettingsMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void ClickBackPauseMenu()
    {
        if(SettingsMenu.activeSelf)
        {
            PlayerPrefs.SetFloat("MusicVolume", MusicVolume.value);
            PlayerPrefs.SetFloat("SoundVolume", SoundVolume.value);
        }

        StatisticMenu.SetActive(false);
        HelpMenu.SetActive(false);
        AskQuitMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void ClickTryBackMainMenu()
    {
        PauseMenu.SetActive(false);
        AskQuitMenu.SetActive(true);
    }

    void SaveGameStats()
    {
        PlayerPrefs.SetInt("GameHours", hours);
        PlayerPrefs.SetInt("GameMinutes", minutes);
        PlayerPrefs.SetInt("GameSeconds", seconds);
        if (daysSurvived < timeCount.days) PlayerPrefs.SetInt("DaysSurvived", timeCount.days);
        PlayerPrefs.SetInt("KillsCount", killsCount);
        PlayerPrefs.SetInt("TimberKillsCount", timberkillsCount);
        PlayerPrefs.SetInt("UniKillsCount", unikillsCount);
        PlayerPrefs.SetInt("PegaKillsCount", pegakillsCount);
        PlayerPrefs.SetInt("GotWoodCount", gotWoodCount);
        PlayerPrefs.SetInt("SpendWoodCount", spendWoodCount);
        PlayerPrefs.SetInt("GotFoodCount", gotFoodCount);
        PlayerPrefs.SetInt("HealsCount", healsCount);
        PlayerPrefs.SetInt("PoniesGotCount", poniesGotCount);
        PlayerPrefs.SetInt("UnitsKilledCount", unitsKilledCount);
    }

    void LoadGameStats()
    {
        if (PlayerPrefs.HasKey("GameHours")) hours = PlayerPrefs.GetInt("GameHours");
        if (PlayerPrefs.HasKey("GameMinutes")) minutes = PlayerPrefs.GetInt("GameMinutes");
        if (PlayerPrefs.HasKey("GameSeconds")) seconds = PlayerPrefs.GetInt("GameSeconds");
        if (PlayerPrefs.HasKey("DaysSurvived")) daysSurvived = PlayerPrefs.GetInt("DaysSurvived");
        if (PlayerPrefs.HasKey("DealthCount")) DealthCountText.text = PlayerPrefs.GetInt("DealthCount").ToString();
        if (PlayerPrefs.HasKey("KillsCount")) killsCount = PlayerPrefs.GetInt("KillsCount");
        if (PlayerPrefs.HasKey("TimberKillsCount")) timberkillsCount = PlayerPrefs.GetInt("TimberKillsCount");
        if (PlayerPrefs.HasKey("UniKillsCount")) unikillsCount = PlayerPrefs.GetInt("UniKillsCount");
        if (PlayerPrefs.HasKey("PegaKillsCount")) pegakillsCount = PlayerPrefs.GetInt("PegaKillsCount");
        if (PlayerPrefs.HasKey("GotWoodCount")) gotWoodCount = PlayerPrefs.GetInt("GotWoodCount");
        if (PlayerPrefs.HasKey("SpendWoodCount")) spendWoodCount = PlayerPrefs.GetInt("SpendWoodCount");
        if (PlayerPrefs.HasKey("GotFoodCount")) gotFoodCount = PlayerPrefs.GetInt("GotFoodCount");
        if (PlayerPrefs.HasKey("HealsCount")) healsCount = PlayerPrefs.GetInt("HealsCount");
        if (PlayerPrefs.HasKey("PoniesGotCount")) poniesGotCount = PlayerPrefs.GetInt("PoniesGotCount");
        if (PlayerPrefs.HasKey("UnitsKilledCount")) unitsKilledCount = PlayerPrefs.GetInt("UnitsKilledCount");
    }

    public void ClickBackMainMenu()
    {
        SaveGameStats();
        SceneManager.LoadScene(0);
    }

    private void OnApplicationQuit()
    {
        SaveGameStats();
    }

    public void UnpauseGame()
    {
        GamePaused = false;
        SettingsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        AskQuitMenu.SetActive(false);

        if (tempTimeScale > 0)
            Time.timeScale = tempTimeScale;
        else
            Time.timeScale = 1;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
            musicVolume = MusicVolume.value;
        }
        else
        {
            MusicVolume.value = musicVolume = 1;
        }

        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            SoundVolume.value = PlayerPrefs.GetFloat("SoundVolume");
            soundVolume = SoundVolume.value;
        }
        else
        {
            SoundVolume.value = soundVolume = 1;
        }

        Time.timeScale = 1;
        GamePaused = false;

        LoadGameStats();

        gameTimeCount = GameTimeCount();
        StartCoroutine(gameTimeCount);
    }

    void Update()
    {
        if (SettingsMenu.activeSelf)
        {
            musicVolume = MusicVolume.value;
            MusicSource.volume = musicVolume * 0.7f;
            soundVolume = SoundVolume.value;
        }

        if(HelpMenu.activeSelf)
        {
            helpLabel.text = helpText[tempTextNumber];
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingsMenu.activeSelf || HelpMenu.activeSelf || AskQuitMenu.activeSelf || StatisticMenu.activeSelf) ClickBackPauseMenu();
            else
            {
                GamePaused = !GamePaused;
                if (GamePaused)
                {

                    PauseMenu.SetActive(true);
                    tempTimeScale = Time.timeScale;
                    Time.timeScale = 0;
                }
                else
                {
                    UnpauseGame();
                }
            }
        }

        if (player.health <= 0)
        {
            if (timerDealth > 0)
            {
                timerDealth -= Time.deltaTime;
            }
            else
            {
                if (dealthText.color.a < 1)
                {
                    dealthText.text = Lang.GetInstance().GetText("f_to_pay_respect");
                    dealthText.color = new Color(1, 1, 1, dealthText.color.a + Time.deltaTime);
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        PoolManager.ClearPools();
                        SaveGameStats();
                        SceneManager.LoadScene(0);
                    }
                }
            }
        }
    }
}
