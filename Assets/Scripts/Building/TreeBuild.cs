using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeBuild : Building
{
    public bool leftTree;
    public bool axing;
    public Earthpony Worker;
    public MainFire mainFire;
    public Sprite MarkedTree;
    public GameObject SliderPrefab;
    public float progress { get; private set; }
    public ForestDissapear myForest;
    public Transform[] myLandingPositions;
    public SquirrelSpawn squirrels;
    public Crown crown;
    public int wood;
    public GameObject WoodPrefab;

    public AudioClip[] AxingSounds;

    bool cutDown, onetimeList;

    Slider tempSlider;

    SpriteRenderer SR;

    float axingTimer;
    int axingI;

    bool onetimeAddTree;

    void TextOff()
    {
        if (onetime)
        {
            HintText.color = new Color(1, 1, 1, 0);
            onetime = false;
        }
    }

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        _audi = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audi.volume = gameManager.soundVolume;

        MovingController controller = Player.GetComponent<MovingController>();
        if (mainFire.buildingLevel > 0 &&
        Vector2.Distance(transform.position, Player.transform.position) < 1.8f && !cutDown && Mathf.Abs(controller.speedX) < 8 &&
        controller.health > 0)
        {
            HintText.text = Lang.GetInstance().GetTextOr(BuildTextCode, BuildText);
            HintText.color = new Color(1, 1, 1, 1);
            onetime = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                mainFire.workManager.TreesToAxe.Add(this);
                SR.sprite = MarkedTree;
                cutDown = true;
            }
        }
        else
        {
            TextOff();
        }
        if (cutDown)
        {
            if (progress < 100)
            {
                if (axing)
                {
                    if(axingTimer > 0)
                    {
                        axingTimer -= Time.deltaTime;
                    }
                    else
                    {
                        _audi.PlayOneShot(AxingSounds[axingI]);
                        if (axingI < AxingSounds.Length - 1) axingI++;
                        else axingI = 0;
                        axingTimer = 0.88f;
                    }


                    progress += Time.deltaTime * 15f;
                }
            }
            else
            {
                if (!onetimeList)
                {
                    mainFire.workManager.treeI = 0;
                    mainFire.workManager.ponyI = 0;
                    mainFire.workManager.TreesToAxe.Remove(this);
                    onetimeList = true;
                }

                crown.timerLanding = 0f;
                crown.LandingPositions.Remove(this);
                if (leftTree)
                {
                    squirrels.LeftTrees.Remove(transform);
                }
                else
                {
                    squirrels.RightTrees.Remove(transform);
                }

                Squirrel tempSq = squirrels.FindSquirrelByTree(transform);
                if (tempSq != null) tempSq.newtree = null;

                TextOff();
                if (Down())
                {
                    if (!onetimeAddTree)
                    {
                        if (myForest != null)
                            myForest.myTrees--;
                        Item newWood = PoolManager.getGameObjectFromPool(WoodPrefab).GetComponent<Item>();
                        newWood.GetComponent<SpriteRenderer>().sprite = newWood.woodSprite;
                        newWood.player = Player;
                        newWood.transform.position = new Vector2(transform.position.x, transform.position.y - 0.2f);
                        newWood.woodCount = wood;
                        resourses.SaveItems.Add(newWood);
                        onetimeAddTree = true;
                    }
                    if(!_audi.isPlaying)
                    gameObject.SetActive(false);
                }
            }

            if (Worker != null && Worker.work != this) Worker = null;
        }
    }
}
