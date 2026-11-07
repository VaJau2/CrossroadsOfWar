using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HireUnit : MonoBehaviour
{
    [HideInInspector]
    public GameManager gameManager;
    public CopyColor Hood;
    public CopyColor Cloth;
    public MainFire fire;
    public Resourses res;
    public EarthponiesCamp mainCamp;
    public SpriteRenderer AnswerSprite;
    public Sprite ThinkingSprite;
    public Sprite YesSprite;
    public Sprite NoSprite;
    public Text HintText;
    [HideInInspector]
    public MovingController Player;
    Earthpony earthpony;
    public bool hintOff;
    [HideInInspector]
    public bool onetimeHint;

    bool onetimeSprite;
    [HideInInspector]
    public float timerThinking;
    [HideInInspector]
    public bool onetimeAgree;
    [HideInInspector]
    public float timerNo, timerYes;

    private void Start()
    {
        earthpony = GetComponent<Earthpony>();
        if (!fire) fire = GameObject.FindGameObjectWithTag("MainFire").GetComponent<MainFire>();
        if (!Player) Player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovingController>();
    }

    private void Update()
    {
        if (timerThinking > 0)
        {
            timerThinking -= Time.deltaTime;
            if (res.Ponies < res.PoniesMax)
            {
                timerYes = 1f;
                onetimeAgree = true;
            }
            else
            {
                onetimeAgree = false;
                timerNo = 1f;
            }
        }
        else
        {
            if (timerYes > 0)
            {
                if (res.Ponies >= res.PoniesMax)
                {
                    onetimeAgree = false;
                    timerNo = 1f;
                    timerYes = 0f;
                }

                AnswerSprite.sprite = YesSprite;
                timerYes -= Time.deltaTime;
            }
            else if (timerNo > 0)
            {
                AnswerSprite.sprite = NoSprite;
                timerNo -= Time.deltaTime;
                hintOff = false;
            }
            else
            {
                if (onetimeAgree)
                {
                    gameManager.poniesGotCount++;
                    res.Ponies++;
                    res.UpdateResourses();

                    if (earthpony.sitOnPlaceOne)
                    {
                        earthpony.sitOnPlaceOne = earthpony.myHome.placeOneBusy = false;
                    }
                    if (earthpony.sitOnPlaceTwo)
                    {
                        earthpony.sitOnPlaceTwo = earthpony.myHome.placeTwoBusy = false;
                    }
                    earthpony.cantSit = false;
                    Cloth.originSR = Player.clothAnimator.GetComponent<SpriteRenderer>();
                    Hood.enabled = Cloth.enabled = true;
                    GetComponent<EarthponyMovingController>().ForgetColors();
                    earthpony.unit = true;
                    earthpony.fire = fire.transform;
                    earthpony.minX = -99.34f;
                    earthpony.maxX = -28.61f;
                    int temp = earthpony.myHome.Ponies.IndexOf(earthpony);
                    earthpony.myHome.PoniesWalk.RemoveAt(temp);
                    earthpony.myHome.Ponies.Remove(earthpony);
                    earthpony.myHome = mainCamp;
                    earthpony.myHome.Ponies.Add(earthpony);
                    earthpony.myHome.PoniesWalk.Add(true);

                    enabled = false;
                }
                if (onetimeSprite) //вызывается в обоих случаях, даже после enabled false
                {
                    AnswerSprite.sprite = null;
                    onetimeSprite = false;
                }
            }
        }

        if (!hintOff && fire.buildingLevel > 0 && Vector2.Distance(Player.transform.position, transform.position) < 3f &&
        Mathf.Abs(Player.speedX) < 8 &&
        Player.health > 0 && earthpony.health > 0)
        {
            if (fire.workManager.tempUnitID == -1 && fire.workManager.cooldown <= 0)
            {
                fire.workManager.cooldown = 0.5f;
                fire.workManager.tempUnitID = earthpony.myHome.Ponies.IndexOf(earthpony);
                HintText.text = Lang.GetInstance().GetText("hire_unit");
                HintText.color = new Color(1, 1, 1, 1);
                onetimeHint = true;
            }
            if (Input.GetKeyDown(KeyCode.E) && fire.workManager.tempUnitID == earthpony.myHome.Ponies.IndexOf(earthpony) &&
                !gameManager.GamePaused)
            { //вторая проверка нужна, чтоб принимался на базу только один, а не сразу несколько
                fire.workManager.tempUnitID = -1;
                fire.workManager.cooldown = 0.6f;
                hintOff = true;
                onetimeSprite = true;
                AnswerSprite.sprite = ThinkingSprite;
                timerThinking = 1f;
            }
        }
        else
        {
            if (onetimeHint)
            {
                fire.workManager.tempUnitID = -1;
                HintText.color = new Color(1, 1, 1, 0);
                onetimeHint = false;
            }
        }
    }
}
