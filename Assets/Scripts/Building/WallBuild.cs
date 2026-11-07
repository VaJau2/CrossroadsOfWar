using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuild : Building
{
    public bool leftWall;
    public RabbitSpawn rabbits;
    public SquirrelSpawn squirels;
    public float health;
    public float healthMax;
    public Sprite[] levelSprites;
    public Sprite[] HealthSpritesLevelOne;
    public Sprite[] HealthSpritesLevelThree;
    public Flag flag;
    public MainFire mainFire;
    int fixWood;

    bool startRepairing;

    SpriteRenderer SR;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        if (buildingLevel >= 0) transform.localEulerAngles = Vector3.zero;
        if (mainFire.buildingLevel == 3 && buildingLevel > 0)
        {
            SR.sprite = levelSprites[2];
        }
        if (mainFire.buildingLevel == 2 && buildingLevel > 0)
        {
            SR.sprite = levelSprites[1];
        }
    }

    void CheckRepair()
    {
        if (!hintOff && Vector2.Distance(Player.transform.position, transform.position) < 2f &&
        Player.GetComponent<MovingController>().speedX == 0 &&
        Player.GetComponent<MovingController>().health > 0)
        {
            if (timerWood > 0)
            {
                HintText.text = Lang.GetInstance().GetText("not_enough_wood");
                timerWood -= Time.deltaTime;
            }
            else
                HintText.text = Lang.GetInstance().GetText("repair_wall");

            HintText.color = new Color(1, 1, 1, 1);
            onetimeHint = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (resourses.Wood >= fixWood)
                {
                    startRepairing = true;
                    hintOff = true;
                }
                else
                    timerWood = 0.7f;
            }
        }
        else
        {
            if (onetimeHint)
            {
                timerWood = 0;
                HintText.color = new Color(1, 1, 1, 0);
                onetimeHint = false;
            }
        }
    }

    public void CheckHealth()
    {
        if (health > healthMax - 10)
        {
            fixWood = 2;
            if (mainFire.buildingLevel == 3)
            {
                SR.sprite = HealthSpritesLevelThree[0];
            }
            else
            {
                SR.sprite = HealthSpritesLevelOne[0];
            }
        }
        else if (health > healthMax / 1.5f)
        {
            fixWood = 3;
            if (mainFire.buildingLevel == 3)
            {
                SR.sprite = HealthSpritesLevelThree[1];
            }
            else
            {
                SR.sprite = HealthSpritesLevelOne[1];
            }
        }
        else if (health > healthMax / 2)
        {
            fixWood = 4;
            if (mainFire.buildingLevel == 3)
            {
                SR.sprite = HealthSpritesLevelThree[2];
            }
            else
            {
                SR.sprite = HealthSpritesLevelOne[2];
            }
        }
        else if (health > healthMax / 4)
        {
            fixWood = 5;
            if (mainFire.buildingLevel == 3)
            {
                SR.sprite = HealthSpritesLevelThree[3];
            }
            else
            {
                SR.sprite = HealthSpritesLevelOne[3];
            }
        }
        else if (health <= 0)
        {
            fixWood = needWood;
            SR.enabled = false;
        }
    }

    private void Update()
    {
        if (buildingLevel == -1)
        {
            if (flag.enabled == false)
            {
                if (Up())
                {
                    buildingLevel = 0;
                }
            }
        }
        if (buildingLevel == 0)
        {
            if (mainFire.buildingLevel > 0)
            {
                CheckBuild();
            }

            if (startBuilding)
            {
                if (Down())
                {
                    if (mainFire.buildingLevel == 3)
                    {
                        SR.sprite = levelSprites[2];
                    }
                    else
                    {
                        SR.sprite = levelSprites[1];
                    }
                    onetime = true;
                    startBuilding = false;
                }
            }
            else
            {
                if (Up())
                {
                    if (onetime)
                    {
                        resourses.AddResourses(-needWood, 0);
                        if (leftWall)
                        {
                            rabbits.LeftWall = true;
                            squirels.LeftWall = true;
                        }
                        else
                        {
                            rabbits.RightWall = true;
                            squirels.RightWall = true;
                        }
                        health = healthMax;
                        buildingLevel++;
                        onetime = false;
                        hintOff = false;
                    }
                }
            }
        }
        if (buildingLevel == 1)
        {
            if (startRepairing)
            {
                if (Down())
                {
                    onetime = true;
                    startRepairing = false;
                }
            }
            else
            {
                if (Up())
                {
                    if (onetime)
                    {
                        resourses.AddResourses(-fixWood,0);
                        SR.enabled = true;
                        health = healthMax;
                        onetime = false;
                        hintOff = false; 
                    }
                }
            }

            CheckHealth();
            if (health < healthMax)
            {
                CheckRepair();
            }
        }
    }
}
