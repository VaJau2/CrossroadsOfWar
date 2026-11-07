using UnityEngine;
using UnityEngine.UI;

public class HelpingHints : MonoBehaviour {

    [SerializeField]
    TimeCount timeCount;

    [SerializeField]
    GameObject gameObjects;

    [SerializeField]
    Text HelpText;

    [SerializeField]
    MovingController Player;
    bool movingHint;
    float movingHintTimer = 1;

    [SerializeField]
    MainFire Fire;
    [SerializeField]
    float FireDistance = 5f;
    float fireHintTimer = 4f;

    [SerializeField]
    Transform CampOne;
    [SerializeField]
    Transform CampTwo;
    [SerializeField]
    float CampDistance = 5f;
    float campHintTimer = 5f;

    [SerializeField]
    Transform Forest;
    [SerializeField]
    float ForestDistance = 10f;
    float forestHintTimer = 6f;

    float timerFoodHint = 70f;
    float foodHintTimer = 6f;

    bool ShowPlaceHint(Transform Place, float dis, string key, float timer, string hintText)
    {
        if (!PlayerPrefs.HasKey(key)) //если подсказка с зажиганием костра еще не появлялась
        {
            if (Vector2.Distance(Player.transform.position, Place.position) < dis) //если игрок близко к огню
            {
                if (timer > 0)
                {
                    HelpText.text = hintText;
                    if (HelpText.color.a < 1)
                    {
                        HelpText.color = new Color(1, 1, 1, HelpText.color.a + Time.deltaTime);
                    }
                    return true; //если появляется эта подсказка, дальше скрипт не идет 
                }
                else
                {
                    PlayerPrefs.SetInt(key,1);
                }
            }
        }
        return false;
    }

    private void Update()
    {
        if(!PlayerPrefs.HasKey("MovingHint")) //если подсказка о передвижении еще не показывалась
        {
            if (gameObjects.activeSelf)
            {
                if (Player.speedX == 0) //пока игрок стоит
                {
                    if (movingHintTimer > 0) //где-то секунду
                    {
                        movingHintTimer -= Time.deltaTime;
                    }
                    else //подсказка плавно появляется
                    {
                        HelpText.text = Lang.GetInstance().GetText("hint_moving");
                        if (HelpText.color.a < 1)
                        {
                            HelpText.color = new Color(1, 1, 1, HelpText.color.a + Time.deltaTime);
                        }
                    }
                }
                else //когда игрок уходит, подсказка перестает быть актуальной
                {
                    PlayerPrefs.SetInt("MovingHint", 1);
                }
            }
        }
        else //все остальные подсказки должны появляться после этой
        {
            if (ShowPlaceHint(Fire.transform, FireDistance, "FireHint", fireHintTimer,
                Lang.GetInstance().GetText("hint_fire")))
            {
                fireHintTimer -= Time.deltaTime;
                return;
            }

            if (Fire.buildingLevel == 1)
            {
                if (ShowPlaceHint(CampOne, CampDistance, "CampHint", campHintTimer,
                    Lang.GetInstance().GetText("hint_camp")))
                {
                    campHintTimer -= Time.deltaTime;
                    return;
                }
                if (ShowPlaceHint(CampTwo, CampDistance, "CampHint", campHintTimer,
                    Lang.GetInstance().GetText("hint_camp")))
                {
                    campHintTimer -= Time.deltaTime;
                    return;
                }
                if (ShowPlaceHint(Forest, ForestDistance, "ForestHint", forestHintTimer,
                    Lang.GetInstance().GetText("hint_forest")))
                {
                    forestHintTimer -= Time.deltaTime;
                    return;
                }
            }

            if (!PlayerPrefs.HasKey("FoodHint"))
            {
                if (timerFoodHint <= 0)
                {
                    if (foodHintTimer > 0)
                    {
                        foodHintTimer -= Time.deltaTime;

                        HelpText.text = Lang.GetInstance().GetText("hint_food");

                        if (HelpText.color.a < 1)
                        {
                            HelpText.color = new Color(1, 1, 1, HelpText.color.a + Time.deltaTime);
                        }
                        return;
                    }
                }
                {
                    PlayerPrefs.SetInt("FoodHint", 1);
                }
            }

            if (timeCount.days == 8)
            {
                if (System.Environment.UserName.Length > 0)
                    HelpText.text = string.Format(Lang.GetInstance().GetText("thanks_for_playing_name"), System.Environment.UserName);
                else
                    HelpText.text = Lang.GetInstance().GetText("thanks_for_playing");

                if (HelpText.color.a < 1)
                {
                    HelpText.color = new Color(1, 1, 1, HelpText.color.a + Time.deltaTime);
                }
                return;
            }

            if (HelpText.color.a > 0)
            {
                HelpText.color = new Color(1, 1, 1, HelpText.color.a - Time.deltaTime);
            }
        }


        if (!PlayerPrefs.HasKey("FoodHint") && timerFoodHint > 0)
        {
            timerFoodHint -= Time.deltaTime;
        }
    }
}
