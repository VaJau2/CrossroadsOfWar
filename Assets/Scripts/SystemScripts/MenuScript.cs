using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MenuScript : MonoBehaviour {

    public AudioSource Ambient;
    [SerializeField]
    RandomSex ExamplePony;

    [SerializeField]
    Button LoadGameButton;

    [SerializeField]
    GameObject FirstMenu;
    [SerializeField]
    GameObject SecondMenu;

    [SerializeField]
    Slider SoundVolume;
    [SerializeField]
    Slider MusicVolume;
    [SerializeField]
    AudioClip ButtonClick;

    AudioSource _audi;

    bool exit, start;

    private void Start()
    {
        _audi = GetComponent<AudioSource>();
        
        LoadVolume();
        
        if(Ini.FileExists("Save.sv"))
        {
            Ini.LoadFile("Save.sv");
            ExamplePony.playerMare = Ini.Get("Player_mare") == "1";
            ExamplePony.Beard.SetActive(Ini.Get("Player_beard") == "1");
            ExamplePony.randomHair = Convert.ToInt32(Ini.Get("Player_hair"));
            ExamplePony.SetNewValues();
            ExamplePony.HairAnim.GetComponent<SpriteRenderer>().color = 
                ExamplePony.Beard.GetComponent<SpriteRenderer>().color = SaveLoadGame.LoadColor("Player_hairColor");
            ExamplePony.GetComponent<SpriteRenderer>().color = SaveLoadGame.LoadColor("Player_bodyColor");
            ExamplePony.anims[1].GetComponent<SpriteRenderer>().color = SaveLoadGame.LoadColor("Player_flagColor");
            ExamplePony.anims[2].GetComponent<SpriteRenderer>().color = SaveLoadGame.LoadColor("Player_eyesColor");

            ExamplePony.gameObject.SetActive(true);

            for (int i = 0; i < ExamplePony.anims.Length; i++)
            {
                ExamplePony.anims[i].SetBool("sit", true);
            }
            ExamplePony.HairAnim.SetBool("sit", true);

            if(ExamplePony.Beard.activeSelf)
                ExamplePony.Beard.GetComponent<Animator>().SetBool("sit", true);

            
            LoadGameButton.interactable = true;
        }
    }

    private void Update()
    {
        _audi.volume = SoundVolume.value * 0.5f;
        Ambient.volume = SoundVolume.value * 0.7f;

        if (start)
        {
            if(!_audi.isPlaying)
            {
                SceneManager.LoadScene(1);
            }
        }

        if(exit)
        {
            if(!_audi.isPlaying)
            {
                Application.Quit();
            }
        }
    }

    public void ClickSettings()
    {
        _audi.PlayOneShot(ButtonClick);
        FirstMenu.SetActive(false);
        SecondMenu.SetActive(true);

        LoadVolume();
    }

    private void LoadVolume()
    {
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            MusicVolume.value = 1;
        }

        if(PlayerPrefs.HasKey("SoundVolume"))
        {
            SoundVolume.value = PlayerPrefs.GetFloat("SoundVolume");
        }
        else
        {
            SoundVolume.value = 1;
        }
    }

    public void PlayClickSound()
    {
        _audi.PlayOneShot(ButtonClick);
    }

    public void ClickBackToMenu()
    {
        PlayClickSound();
        FirstMenu.SetActive(true);
        SecondMenu.SetActive(false);

        PlayerPrefs.SetFloat("MusicVolume", MusicVolume.value);
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume.value);
    }

    public void ClickResetPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


    public void Exit()
    {
        PlayClickSound();
        exit = true;
    }

    public void ContinueGame()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("Load", 1);
        start = true;
    }

    public void StartGame()
    {
        PlayClickSound();
        PlayerPrefs.SetInt("Load", 0);
        start = true;
    }

}
