using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthpony : Animal
{
    [Header("Shooting")]
    [SerializeField]
    GameObject BowPrefab;
    public Transform ArrowStartPosition;
    bool shoot;
    [SerializeField]
    float waiting;
    public GameObject ArrowPrefab;
    float speed = 50;
    Vector3 tempDestination;

    [Header("Work")]
    public bool hunter;
    [HideInInspector]
    public bool hasTower;
    [HideInInspector]
    public bool onTower;

    public float distanceSeeing = 13f;
    public bool axing;
    public Animator Axe;
    public GameObject Bow;
    public Building work;
    [Header("Sound")]
    public AudioClip[] WalkSlow;
    public AudioClip[] WalkFast;
    public AudioClip[] Run;
    public AudioClip[] snowWalkSlow;
    public AudioClip[] snowWalkFast;
    public AudioClip[] snowRun;
    public AudioClip[] GuitarMusic;
    [SerializeField]
    AudioSource guitarAudi;
    int guitarMusicI;
    [Header("Other")]
    public GameObject FollowingPicture;
    public CampAlertSystem alerting;
    public RandomColor[] randomColors;
    public bool unit;
    public EarthponiesCamp myHome;
    public Transform fire;
    public UrsaMinor ursa;
    public Manticore Timberwolf1;
    public Manticore Timberwolf2;
    public UnicornsSpawn UniSpawn;
    public PegasusSpawn PegaSpawn;

    public Creature tempVictim;

    public int myOrder = 6;

    [HideInInspector]
    public bool guitaring;

    HireUnit hireUnit;

    public bool sit;
    float shootCooldown;
    [HideInInspector]
    public bool sitOnPlaceOne, sitOnPlaceTwo, cantSit;
    float timerTowerSit = 2f;
    float randomPlace;
    [HideInInspector]
    public bool hasBow;
    [HideInInspector]
    public float timerRunningForLife;
    bool followPlayer;
    float timerCheckSit;
    float tempDisFollow;

    [SerializeField]
    AudioClip BowShoot;

    int uniI, pegI;
    [HideInInspector]
    public bool onetimeDead;
    bool onetimeAlert;

    float stepTimer;
    int stepI;

    float sitGuitarTimer;
    bool onetimeSitGuitarTimer;

    void GoHome(float _speed)
    {
        if (!hunter)
        {
            if (sit)
                GetUp();
            if (transform.position.x > myHome.transform.position.x + 0.2f)
            {
                Running(-_speed);
            }
            else if (transform.position.x < myHome.transform.position.x - 0.2f)
            {
                Running(_speed);
            }
            else
            {
                int index = myHome.Ponies.IndexOf(this);
                if (hunter) return;
                if (index > myHome.PoniesWalk.Count)
                {
                    Debug.Log("У меня нет PWalk :c");
                    return;
                }
                myHome.PoniesWalk[index] = false;
                if (hireUnit.onetimeHint)
                {
                    hireUnit.HintText.color = new Color(1, 1, 1, 0);
                    hireUnit.onetimeHint = false;
                }
                gameObject.SetActive(false);
            }
        }
    }

    public bool SomeoneIsTryingToKillMe
    {
        get
        {
            if (hunter)
            { //проверки охотников
                if (Timberwolf1.health > 0 && Vector2.Distance(transform.position, Timberwolf1.transform.position) < distanceSeeing)
                { //проверяем первого волка
                    tempVictim = Timberwolf1;
                    return true;
                }
                if (Timberwolf2.health > 0 && Vector2.Distance(transform.position, Timberwolf2.transform.position) < distanceSeeing)
                {   //проверяем второго волка
                    tempVictim = Timberwolf2;
                    return true;
                }
                if (ursa.gameObject.activeSelf && ursa.health > 0 && Vector2.Distance(transform.position, ursa.transform.position) < distanceSeeing)
                {  //проверяем урсу
                    tempVictim = ursa;
                    return true;
                }
                if (UniSpawn.unicorns.Count > 0)
                {  //проверяем единорогов
                    if (uniI < UniSpawn.unicorns.Count)
                    {
                        if (UniSpawn.unicorns[uniI].health > 0 && Vector2.Distance(transform.position, UniSpawn.unicorns[uniI].transform.position) < distanceSeeing)
                        {
                            tempVictim = UniSpawn.unicorns[uniI];
                            return true;
                        }
                    }
                    else uniI = 0;
                }
                if (PegaSpawn.pegasus.Count > 0)
                {  //проверяем пегасов
                    if (pegI < PegaSpawn.pegasus.Count)
                    {
                        if (PegaSpawn.pegasus[pegI].health > 0 && Vector2.Distance(transform.position, PegaSpawn.pegasus[pegI].transform.position) < distanceSeeing)
                        {
                            tempVictim = PegaSpawn.pegasus[pegI];
                            return true;
                        }
                    }
                    else pegI = 0;
                }
            }
            else if (work != null) return false; //если понь работает, он должен оставаться на своей работе

            if (Timberwolf1.health > 0 && Vector2.Distance(transform.position, Timberwolf1.transform.position) < 11f)
            {
                return true;
            }
            if (Timberwolf2.health > 0 && Vector2.Distance(transform.position, Timberwolf2.transform.position) < 11f)
            {
                return true;
            }
            if(ursa.gameObject.activeSelf && ursa.health > 0 && Vector2.Distance(transform.position, ursa.transform.position) < 11f)
            {
                return true;
            }
            if (UniSpawn.unicorns.Count > 0)
            {
                if (uniI < UniSpawn.unicorns.Count - 1) uniI++;
                else uniI = 0;

                if (UniSpawn.unicorns[uniI].health == 0) uniI = 0;
                else if (UniSpawn.unicorns[uniI].health > 0 && Vector2.Distance(transform.position, UniSpawn.unicorns[uniI].transform.position) < 13.5f)
                {
                    return true;
                }
            }
            if (PegaSpawn.pegasus.Count > 0)
            {
                if (pegI < PegaSpawn.pegasus.Count - 1) pegI++;
                else pegI = 0;

                if (PegaSpawn.pegasus[pegI].health == 0) pegI = 0;
                else if (PegaSpawn.pegasus[pegI].health > 0 && Vector2.Distance(transform.position, PegaSpawn.pegasus[pegI].transform.position) < 14f)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public override bool isHitted
    {
        get
        {
            if (flyTime > 0.25f)
            {
                speedY = 6f;
            }
            else
            {
                speedY = -6f;
            }

            if (redTime > 0)
            {
                redTime -= Time.deltaTime;
            }

            if (flyTime > 0)
            {
                flyTime -= 0.05f;
                return true;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
                flyTime = 0;
                speedX = speedY = 0;
                return false;
            }
        }
    }

    public void SitFire(float fireX)
    {
        maxSpeed = 3f;
        if (RunTo(new Vector2(fireX, transform.position.y)))
        {
            anim.SetBool("sit", true);
            sit = true;
            if (myHome.myCamp && !myHome.guitarIsPlaying)
            {
                if (!onetimeSitGuitarTimer)
                {
                    sitGuitarTimer = Random.Range(1f, 20f);
                    onetimeSitGuitarTimer = true;
                }
                else
                {
                    if (sitGuitarTimer > 0)
                    {
                        sitGuitarTimer -= Time.deltaTime;
                    }
                    else CheckGuitar();
                }
            }
            if (guitaring) CheckGuitar();
        }
    }

    void GetUp()
    {
        onetimeSitGuitarTimer = false;
        if (guitaring)
        {
            myHome.guitarIsPlaying = false;
            guitaring = false;
            guitarAudi.clip = null;
        }

        timerTowerSit = 5f;
        anim.SetBool("sit", false);
        sit = false;
        if (sitOnPlaceOne)
        {
            myHome.placeOneBusy = false;
            sitOnPlaceOne = false;
        }
        if (sitOnPlaceTwo)
        {
            myHome.placeTwoBusy = false;
            sitOnPlaceTwo = false;
        }
    }

    public void FindSit()
    {
        if (!sitOnPlaceOne && !sitOnPlaceTwo)
        {
            if (randomPlace == 0) randomPlace = Random.value + 0.1f;
            else
            {
                if (randomPlace > 0.6f)
                {
                    if (!myHome.placeOneBusy)
                    {
                        myHome.placeOneBusy = sitOnPlaceOne = true;
                    }
                    else if (!myHome.placeTwoBusy)
                    {
                        myHome.placeTwoBusy = sitOnPlaceTwo = true;
                    }
                    else
                    {
                        timerCheckSit = 10f;
                        cantSit = true;
                    }
                }
                else
                {
                    if (!myHome.placeTwoBusy)
                    {
                        myHome.placeTwoBusy = sitOnPlaceTwo = true;
                    }
                    else if (!myHome.placeOneBusy)
                    {
                        myHome.placeOneBusy = sitOnPlaceOne = true;
                    }
                    else
                    {
                        timerCheckSit = 10f;
                        cantSit = true;
                    }
                }
            }

        }
        else
        {
            if (sitOnPlaceOne)
            {
                SR.flipX = false;
                SitFire(fire.position.x - 2.1f);
            }
            if (sitOnPlaceTwo)
            {
                SR.flipX = true;
                SitFire(fire.position.x + 2.5f);
            }
        }
    }

    public void HangAround()
    {
        if (timerCheckSit > 0) //понь проверяет, освободился ли костер, чтоб можно было туда сесть
        {
            timerCheckSit -= Time.deltaTime;
        }
        else
        {
            cantSit = false;
        }

        if (sit)
            GetUp();
        if (timerStand > 0)
        {
            if (Vector2.Distance(transform.position, Camera.transform.position) < 9f)
            {
                if (transform.position.x > Camera.transform.position.x) SR.flipX = true;
                else SR.flipX = false;
            }
        }
        maxSpeed = 3f;
        GoSomewhere();
    }

    void Flip(bool flip)
    {
        if (SR.flipX != flip)
        {
            SR.flipX = flip;
        }
    }

    void CheckFlip(float destinX)
    {
        if (destinX > transform.position.x)
        {
            Flip(false);
        }
        else
        {
            Flip(true);
        }
    }

    void MakeShoot()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        else
        {
            anim.SetTrigger("shoot");
            GetComponent<EarthponyMovingController>().shoot = true;
            shootCooldown = 1f;
            shoot = true;
            waiting = 0.5f;
            tempDestination = tempVictim.transform.position;
            CheckFlip(tempDestination.x);
        }
    }

    void Shoot()
    {
        axing = false;
        if (waiting <= 0)
        {
            if (tempVictim != null)
            {
                if (tempVictim.health > 0 && Vector2.Distance(transform.position, tempVictim.transform.position) <= distanceSeeing)
                {
                    maxSpeed = 6f;
                    if (sit)
                        GetUp();

                    if (!onTower)
                    {
                        if (RunTo(tempVictim.transform.position, distanceSeeing))
                        {
                            MakeShoot();
                        }
                    }
                    else
                    {
                        MakeShoot();
                    }
                }
                else
                {
                    tempVictim = null;
                }
            }
        }
    }

    void CheckShooting()
    {
        if (shoot)
        {
            if (waiting > 0)
            {
                waiting -= Time.deltaTime;
            }
            else
            {
                _audi.PlayOneShot(BowShoot);

                Vector3 mouseWorldPos = tempDestination;
                bool inrange;
                Vector3 tempStartPos;
                if (SR.flipX)
                {
                    tempStartPos = new Vector3(ArrowStartPosition.position.x - 2.8f, ArrowStartPosition.position.y, ArrowStartPosition.position.z);
                }
                else
                {
                    tempStartPos = ArrowStartPosition.position;
                }
                Vector3 direction = ArrowTest.DirectionOfLaunchForArc(mouseWorldPos, tempStartPos, speed, false, out inrange);

                Rigidbody2D newarrow = PoolManager.getGameObjectFromPool(ArrowPrefab).GetComponent<Rigidbody2D>();
                newarrow.GetComponent<SpriteRenderer>().flipX = SR.flipX;
                newarrow.transform.position = tempStartPos;
                newarrow.velocity = transform.TransformDirection(direction * speed * 2.2f);
                ArrowScript newArrow = newarrow.GetComponent<ArrowScript>();
                newArrow.Hunter = this;
                newArrow.SetNewArrow();
                newArrow.timer = 5f;
                newArrow.unicornSpawn = UniSpawn;
                newArrow.pegasusSpawn = PegaSpawn;
                newArrow.Timberwolf1 = Timberwolf1;
                newArrow.Timberwolf2 = Timberwolf2;
                newArrow.Ursa = ursa;
                shoot = false;
            }
        }
    }

    void SitOnTower()
    {
        TowerBuild tower = work.GetComponent<TowerBuild>(); //создаем локальную башню, чтоб не пришлось по три раза геткомпонить
        onTower = true;
        transform.position = new Vector2(tower.HunterPosition.position.x,transform.position.y);
        positionY = tower.HunterPosition.position.y;
        distanceSeeing = tower.radius;

        if (timerTowerSit > 0)
        {
            timerTowerSit -= Time.deltaTime;
            sit = false;
            anim.SetBool("sit", false);
        }
        else
        {
            sit = true;
            anim.SetBool("sit", true);
        }
    }

    bool FollowPlayer
    {
        get
        {
            if (followPlayer)
            {
                FollowingPicture.SetActive(true);
                float dist = Vector2.Distance(transform.position, hireUnit.Player.transform.position);
                if (dist > 6f + tempDisFollow) maxSpeed = 9f;
                else if (dist > 5f + tempDisFollow) maxSpeed = 4f;
                else maxSpeed = 3f;

                RunTo(hireUnit.Player.transform.position, 3f + tempDisFollow);

                if (Vector2.Distance(hireUnit.Player.transform.position, transform.position) < 2f &&
        Mathf.Abs(hireUnit.Player.speedX) < 8 &&
        hireUnit.Player.health > 0)
                {
                    hireUnit.HintText.text = Lang.GetInstance().GetText("squad_dismiss");
                    hireUnit.HintText.color = new Color(1, 1, 1, 1);
                    hireUnit.onetimeHint = true;
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        hireUnit.mainCamp.fireBuild.huntersFollow--;
                        followPlayer = false;
                    }
                }
                else
                {
                    if (hireUnit.onetimeHint)
                    {
                        hireUnit.HintText.color = new Color(1, 1, 1, 0);
                        hireUnit.onetimeHint = false;
                    }
                }

                return true;
            }
            else
            {
                FollowingPicture.SetActive(false);
                if (hireUnit.mainCamp != null && hireUnit.mainCamp.fireBuild != null)
                {
                    if (Vector2.Distance(hireUnit.Player.transform.position, transform.position) < 2f &&
           Mathf.Abs(hireUnit.Player.speedX) < 8 &&
           hireUnit.Player.health > 0 && hireUnit.mainCamp.fireBuild.huntersFollow < hireUnit.mainCamp.fireBuild.buildingLevel)
                    {
                        hireUnit.HintText.text = Lang.GetInstance().GetText("squad_gather");
                        hireUnit.HintText.color = new Color(1, 1, 1, 1);
                        hireUnit.onetimeHint = true;
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            tempDisFollow = hireUnit.mainCamp.fireBuild.huntersFollow;
                            hireUnit.mainCamp.fireBuild.huntersFollow++;
                            followPlayer = true;
                            GetUp();
                        }
                    }
                    else
                    {
                        if (hireUnit.onetimeHint)
                        {
                            hireUnit.HintText.color = new Color(1, 1, 1, 0);
                            hireUnit.onetimeHint = false;
                        }
                    }
                }
                return false;
            }

        }
    }

    void CheckWork()
    {
        if (RunTo(work.transform.position, 1.6f)) //если понь близко к работе
        {
            TreeBuild tempWork = work.GetComponent<TreeBuild>();
            if (tempWork)
            {
                if (tempWork.progress >= 100)
                {
                    axing = false;
                    tempWork.axing = false;
                    work = null;
                }
                else
                {
                    axing = true;
                    tempWork.axing = true;
                }
                return;
            }
            WeaponBuilding tempWork1 = work.GetComponent<WeaponBuilding>();
            if (tempWork1)
            {
                anim.SetBool("sit", true);
                sit = true;
                transform.position = tempWork1.workerPosition.position;
                positionY = tempWork1.workerPosition.position.y;
                return;
            }
            FarmBuild tempWork2 = work.GetComponent<FarmBuild>();
            if (tempWork2)
            {
                anim.SetBool("sit", true);
                sit = true;
                transform.position = tempWork2.workerPosition.position;
                positionY = tempWork2.workerPosition.position.y;
                return;
            }
        }
        else //если понь далеко от работы
        {
            axing = false;
            GetUp();
            maxSpeed = 4f;
        }
    }

    void CheckWood()
    {
        if (RunTo(work.transform.position, 1.6f)) //если охотник близко к дереву
        {
            TreeBuild tempWork = work.GetComponent<TreeBuild>();
            if (tempWork)
            {
                if (tempWork.progress >= 100)
                {
                    axing = false;
                    tempWork.axing = false;
                    work = null;
                }
                else
                {
                    axing = true;
                    tempWork.axing = true;
                }
                return;
            }
        }
        else //если охотник далеко от дерева
        {
            axing = false;
            GetUp();
            maxSpeed = 4f;
        }
    }

    void CheckGuitar()
    {
        if(myHome.myCamp)
        {
            if(!myHome.musicScript.MusicPlay && (myHome.timing.hours >= 21 || myHome.timing.hours < 5))
            {
                guitarAudi.volume = gameManager.musicVolume;

                if (guitarAudi.volume > 0)
                {
                    if (!myHome.guitarIsPlaying)
                    {
                        myHome.guitarIsPlaying = true;
                        guitaring = true;
                    }
                    if (guitaring)
                    {
                        if (!guitarAudi.isPlaying)
                        {
                            if (guitarMusicI < GuitarMusic.Length - 1) guitarMusicI++;
                            else guitarMusicI = 0;
                            guitarAudi.clip = GuitarMusic[guitarMusicI];
                            guitarAudi.Play();
                        }
                    }
                }
                else
                {
                    myHome.guitarIsPlaying = false;
                    guitaring = false;
                    guitarAudi.clip = null;
                }
            }
            else
            {
                if (guitarAudi.volume > 0)
                {
                    guitarAudi.volume -= Time.deltaTime / 2f;
                }
                else
                {
                    myHome.guitarIsPlaying = false;
                    guitaring = false;
                    guitarAudi.clip = null;
                }
            }
        }
    }

    void ShakeGuitarMusic()
    {
        for(int i = 0; i < GuitarMusic.Length; i++)
        {
            int randomI = Random.Range(0, GuitarMusic.Length);
            AudioClip temp = GuitarMusic[i];
            GuitarMusic[i] = GuitarMusic[randomI];
            GuitarMusic[randomI] = temp;
        }
    }

    private void Start()
    {
        InitiateAnimal();
        hireUnit = GetComponent<HireUnit>();
        alerting = GameObject.FindGameObjectWithTag("Resourses").GetComponent<CampAlertSystem>();
        guitarMusicI = -1;
        ShakeGuitarMusic();
    }

    void SoundSteps()
    {
        _audi.volume = 0.5f * gameManager.soundVolume;
        

        float spd = Mathf.Abs(speedX);
        bool snow = transform.position.x > 20;
        if (spd > 0 && spd < 4)
        {
            //играем звуки шагов
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else
            {
                if (snow)
                {
                    _audi.PlayOneShot(snowWalkSlow[stepI]);
                }
                else
                {
                    _audi.PlayOneShot(WalkSlow[stepI]);

                }
                stepTimer = 0.42f;
                if (stepI < WalkSlow.Length - 1) stepI++;
                else stepI = 0;

            }
        }
        if (spd > 3 && spd < 8)
        {
            //играем звуки шагов
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else
            {
                if (snow)
                {
                    _audi.PlayOneShot(snowWalkFast[stepI]);
                }
                else
                {
                    _audi.PlayOneShot(WalkFast[stepI]);

                }
                stepTimer = 0.35f;
                if (stepI < WalkSlow.Length - 1) stepI++;
                else stepI = 0;

            }
        }
        else if (spd >= 8)
        {
            //играем звуки бега
            if (stepTimer > 0)
            {
                stepTimer -= Time.deltaTime;
            }
            else
            {
                if (snow)
                {
                    _audi.PlayOneShot(snowRun[stepI]);
                }
                else
                {
                    _audi.PlayOneShot(Run[stepI]);
                }
                stepTimer = 0.58f;
                if (stepI < Run.Length - 1) stepI++;
                else stepI = 0;

            }
        }
    }

    void GetBow()
    {
        if (RunTo(work.transform.position))
        {
            speedX = 0;
            Bow.SetActive(true);
            Bow.GetComponent<SpriteRenderer>().flipX = SR.flipX;
            work = null;
            hasBow = true;
        }
        else //по пути он должен встать и забыть про костер
        {
            GetUp();
        }
    }

    private void Update()
    {
        rigbody.velocity = new Vector2(speedX, speedY);
        anim.SetFloat("speed", Mathf.Abs(speedX));
        SoundSteps();

        if (work == null && positionY != -0.83)
        {
            positionY = -0.83f;
        }

        anim.SetBool("axe", axing);
        Axe.SetBool("axe", axing);
        anim.SetBool("guitar", guitaring);

        if (stanned > 0)
        {
            stanned -= Time.deltaTime;
        }

        CheckShooting();

        if (!isHitted)
        {
            if (health > 0)
            {
                if (stanned < 0.1f)
                {
                    if (timerRunningForLife > 0) //если понь убегает
                    {
                        axing = false;
                        GoHome(8);
                        timerRunningForLife -= Time.deltaTime;
                    }
                    else //если понь не убегает
                    {
                        if (!SomeoneIsTryingToKillMe)
                        {
                            onetimeAlert = false;
                            if (hunter) //если понь охотник
                            {
                                axing = false;
                                if (hasBow)
                                {
                                    if (hasTower)
                                    {
                                        if (hireUnit.onetimeHint)
                                        {
                                            hireUnit.HintText.color = new Color(1, 1, 1, 0);
                                            hireUnit.onetimeHint = false;
                                        }

                                        if (RunTo(work.transform.position, 1.5f) || onTower)
                                        {
                                            SitOnTower();
                                        }
                                        else //по пути он должен встать и забыть про костер
                                        {
                                            GetUp();
                                        }
                                    }
                                    else
                                    {
                                        if (!FollowPlayer)
                                        {
                                            if (work != null)
                                            {
                                                CheckWood();
                                            }
                                            else
                                            {
                                                if (!cantSit)
                                                {
                                                    FindSit();
                                                }
                                                else
                                                {
                                                    HangAround();
                                                }
                                            }
                                        }
                                    }

                                }
                                else //если у охотника еще нет лука
                                {
                                    GetBow();
                                }
                            }
                            else //если понь не охотник
                            {
                                if (!unit || work == null)
                                {
                                    axing = false;
                                    if ((myHome.weather.weatherNumber > 7 || myHome.timing.hours < 4 || myHome.timing.hours > 21) && myHome.enabled)
                                    {
                                        GoHome(4);
                                    }
                                    else //если у пня нет никакой работы
                                    {
                                        if (!cantSit)
                                        {
                                            FindSit();
                                        }
                                        else
                                        {
                                            HangAround();
                                        }
                                    }

                                }
                                else //если у пня есть какая-то работа
                                {
                                    CheckWork();
                                }
                            }
                        }
                        else //если понь находится в сражении
                        {
                            if(unit)
                            {
                                if (!onetimeAlert)
                                {
                                    onetimeAlert = true;
                                    alerting.SeeEnemy(transform.position);
                                }
                            }

                            if (hunter)
                            {
                                if (hasBow)
                                    Shoot();
                                else
                                    GetBow();
                            }
                            else //если понь - не охотник
                            {
                                timerRunningForLife = 3.5f;
                            }
                        }
                    }
                }
                else //если понь в стане
                {
                    speedX = 0;
                }
            }
            else //если понь умер
            {
                rigbody.velocity = Vector2.zero;
                if (!hunter)
                    myHome.Ponies.Remove(this);
                else
                {
                    myHome.Hunters.Remove(this);
                    Bow.SetActive(false);
                    hunter = hasBow = false;

                    Item newBow = PoolManager.getGameObjectFromPool(BowPrefab).GetComponent<Item>();
                    newBow.GetComponent<SpriteRenderer>().sprite = newBow.bowSprite;
                    newBow.player = hireUnit.Player.transform;
                    newBow.transform.position = new Vector2(transform.position.x, transform.position.y - 0.2f);
                    newBow.bowCount = 1;
                    myHome.res.SaveItems.Add(newBow);
                }
                if (followPlayer)
                {
                    hireUnit.mainCamp.fireBuild.huntersFollow--;
                    followPlayer = false;
                }
                if (work != null)
                {
                    if (work.GetComponent<WeaponBuilding>())
                        work.GetComponent<WeaponBuilding>().worker = null;
                    if (work.GetComponent<FarmBuild>())
                        work.GetComponent<FarmBuild>().worker = null;
                    if (work.GetComponent<TowerBuild>())
                    {
                        hasTower = onTower = false;
                        work.GetComponent<TowerBuild>().myHunter = null;
                    }
                    work = null;
                }

                GetComponent<EarthponyMovingController>().MakeColorsNotHired();

                hireUnit.hintOff = false;
                hireUnit.onetimeAgree = false;
                hireUnit.timerThinking = hireUnit.timerNo = hireUnit.timerYes = 0;
                hireUnit.enabled = true;

                unit = false;

                if (!onetimeDead)
                {
                    hireUnit.res.Ponies--;
                    hireUnit.res.UpdateResourses();
                    gameManager.earthponiesCount--;
                    gameManager.unitsKilledCount++;
                    onetimeDead = true;
                }
                Dead();
            }
        }
    }
}