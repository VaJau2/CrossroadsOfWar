using UnityEngine;
using UnityEngine.UI;

public class ChoosePonyMenu : MonoBehaviour {

    [Header("StartPlayingThings")]
    public SaveLoadGame Saving;
    public GameObject GameThings;
    public GameObject MainCamera;
    public GameObject Sky;


    [Header("Changing Things")]
    public RandomSex ExamplePony;
    public RandomSex MainHero;

    public Text genderText;
    bool playerMare;

    public Toggle BeardToggle;

    public Slider HairSlider;

    public Dropdown ChangeColor;
    public Slider ColorRed;
    public Slider ColorGreen;
    public Slider ColorBlue;

    float timerCheck, timerColor;
    bool startCheck;

    Color tempHairColor, tempBodyColor, tempEyesColor, tempFlagColor;

    private void Start()
    {
        timerCheck = 0.11f;

        if (PlayerPrefs.HasKey("Load") && PlayerPrefs.GetInt("Load") == 1)
        {
            if (Ini.FileExists("Save.sv"))
            {
                Saving.LoadGame();
                StartGame();
            }
        }
    }

   public void ChangeGender()
    {
        playerMare = !playerMare;
        genderText.text = Lang.GetInstance().GetText(playerMare ? "gender_mare" : "gender_colt");
        ExamplePony.playerMare = playerMare;
        ExamplePony.SetNewValues();
    }

    public void SetHairForm()
    {
        ExamplePony.randomHair = (int)HairSlider.value;
        ExamplePony.SetNewValues();
    }

    void StartCheck()
    {
        playerMare = ExamplePony.playerMare;
        genderText.text = Lang.GetInstance().GetText(playerMare ? "gender_mare" : "gender_colt");

        if (playerMare)
        {
            HairSlider.gameObject.SetActive(true);
            HairSlider.value = ExamplePony.randomHair;
        }
        else
        {
            HairSlider.gameObject.SetActive(false);
            BeardToggle.gameObject.SetActive(true);
        }

        BeardToggle.isOn = ExamplePony.Beard.activeSelf && !playerMare;

        tempHairColor = ExamplePony.HairAnim.GetComponent<SpriteRenderer>().color;
        tempBodyColor = ExamplePony.GetComponent<SpriteRenderer>().color;
        tempFlagColor = ExamplePony.anims[1].GetComponent<SpriteRenderer>().color;
        tempEyesColor = ExamplePony.anims[2].GetComponent<SpriteRenderer>().color;

        ChangeColor.value = 0;

        ColorRed.value = tempHairColor.r;
        ColorGreen.value = tempHairColor.g;
        ColorBlue.value = tempHairColor.b;
    }

    public void OpenHairColor()
    {
        if(ChangeColor.value == 0)
        {
            ColorRed.value = tempHairColor.r;
            ColorGreen.value = tempHairColor.g;
            ColorBlue.value = tempHairColor.b;
        }
        else if(ChangeColor.value == 1)
        {
            ColorRed.value = tempBodyColor.r;
            ColorGreen.value = tempBodyColor.g;
            ColorBlue.value = tempBodyColor.b;
        }
        else if(ChangeColor.value == 2)
        {
            ColorRed.value = tempEyesColor.r;
            ColorGreen.value = tempEyesColor.g;
            ColorBlue.value = tempEyesColor.b;
        }
        else if(ChangeColor.value == 3)
        {
            ColorRed.value = tempFlagColor.r;
            ColorGreen.value = tempFlagColor.g;
            ColorBlue.value = tempFlagColor.b;
        }
        timerColor = 0.1f;
    }

    void StartGame()
    {
        MainCamera.GetComponent<TimeCount>().enabled = true;
        MainCamera.GetComponent<SunMoving>().enabled = true;
        MainCamera.GetComponent<CameraFollow>().enabled = true;
        MainCamera.GetComponent<WeatherControl>().enabled = true;
        Sky.SetActive(true);
        GameThings.SetActive(true);
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void ClickPlay()
    {
        MainHero.playerMare = playerMare;
        MainHero.Beard.SetActive(BeardToggle.isOn && !playerMare);
        MainHero.randomHair = (int)HairSlider.value;
        MainHero.SetNewValues();
        MainHero.HairAnim.GetComponent<SpriteRenderer>().color = tempHairColor;
        MainHero.GetComponent<SpriteRenderer>().color = tempBodyColor;
        MainHero.anims[1].GetComponent<SpriteRenderer>().color = tempFlagColor;
        MainHero.anims[2].GetComponent<SpriteRenderer>().color = tempEyesColor;

        StartGame();
    }

    void UpdateCheck()
    {
        if (playerMare)
        {
            HairSlider.gameObject.SetActive(true);
            BeardToggle.gameObject.SetActive(false);
        }
        else
        {
            HairSlider.gameObject.SetActive(false);
            BeardToggle.gameObject.SetActive(true);
        }

        ExamplePony.Beard.SetActive(BeardToggle.isOn && !playerMare);

        if (timerColor > 0)
        {
            timerColor -= Time.deltaTime;
        }
        else
        {
            if (ChangeColor.value == 0)
            {
                tempHairColor.r = ColorRed.value;
                tempHairColor.g = ColorGreen.value;
                tempHairColor.b = ColorBlue.value;
            }
            else if (ChangeColor.value == 1)
            {
                tempBodyColor.r = ColorRed.value;
                tempBodyColor.g =  ColorGreen.value;
                tempBodyColor.b = ColorBlue.value;
            }
            else if (ChangeColor.value == 2)
            {
                tempEyesColor.r = ColorRed.value;
                tempEyesColor.g = ColorGreen.value;
                tempEyesColor.b = ColorBlue.value;
            }
            else if (ChangeColor.value == 3)
            {
                tempFlagColor.r = ColorRed.value;
                tempFlagColor.g = ColorGreen.value;
                tempFlagColor.b = ColorBlue.value;
            }
        }


        ExamplePony.HairAnim.GetComponent<SpriteRenderer>().color = tempHairColor;
        ExamplePony.GetComponent<SpriteRenderer>().color = tempBodyColor;
        ExamplePony.anims[1].GetComponent<SpriteRenderer>().color = tempFlagColor;
        ExamplePony.anims[2].GetComponent<SpriteRenderer>().color = tempEyesColor;
    }

    private void Update()
    {
        if(!startCheck)
        {
            //вводим значения из рандомно созданного пня
            if (timerCheck > 0)
            {
                timerCheck -= Time.deltaTime;
            }
            else
            {
                StartCheck();
                startCheck = true;
            }
        }
        else
        {
            //делаем наоборот
            UpdateCheck();
        }
    }
}
