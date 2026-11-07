using UnityEngine;
using UnityEngine.UI;

public class TwilightScript : MonoBehaviour
{
    public Manticore Timberwolf1;
    public Manticore Timberwolf2;
    public UrsaMinor ursa;
    public UnicornsSpawn UniSpawn;
    public PegasusSpawn PegaSpawn;
    int uniI, pegI;

    public float dissapearTimer = 1.5f;
    public MovingController Player;
    public string[] AskText;
    public string[] AnswerText;
    // Коды перевода AskText и AnswerText (по тем же индексам)
    public string[] AskTextCodes;
    public string[] AnswerTextCodes;
    bool dissapear;
    int textI;

    [SerializeField]
    Text HintText;
    [SerializeField]
    Text Answer;

    [SerializeField]
    Animator anim;

    bool onetimeHint;

    float answerTimer;
    bool answer;

    public bool SomeoneIsTryingToKillMe
    {
        get
        {
            if (Timberwolf1.health > 0 && Vector2.Distance(transform.position, Timberwolf1.transform.position) < 11f)
            {
                return true;
            }
            if (Timberwolf2.health > 0 && Vector2.Distance(transform.position, Timberwolf2.transform.position) < 11f)
            {
                return true;
            }
            if (ursa.gameObject.activeSelf && ursa.health > 0 && Vector2.Distance(transform.position, ursa.transform.position) < 11f)
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

    SpriteRenderer SR;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        Answer.transform.position = new Vector2(transform.position.x, transform.position.y + 3f);

        if(textI > 0) //если твай начала говорить с игроком, она поворачивается к нему лицом
        {
            if (transform.position.x > Player.transform.position.x) SR.flipX = true;
            else SR.flipX = false;
        }

        if(SomeoneIsTryingToKillMe)
        {
            dissapear = true;
            Answer.gameObject.SetActive(false);
            if (onetimeHint)
            {
                HintText.color = new Color(1, 1, 1, 0);
                onetimeHint = false;
            }
            answer = false;
        }

        if (answer) //если она сейчас говорит
        {
            if (onetimeHint)
            {
                HintText.color = new Color(1, 1, 1, 0);
                onetimeHint = false;
            }

            if (answerTimer > 0)
            {
                Answer.gameObject.SetActive(true);
                Answer.text = Lang.GetInstance().GetTextOr(AnswerTextCodes, textI, AnswerText[textI]);
                answerTimer -= Time.deltaTime;
            }
            else
            {
                if (textI < AnswerText.Length)
                    textI++;
                else dissapear = true;
                Answer.gameObject.SetActive(false);
                answer = false;
            }
        }
        else if (dissapear) //если она сейчас исчезает
        {
            if (dissapearTimer > 0)
            {
                dissapearTimer -= Time.deltaTime;
                anim.SetTrigger("dissapear");
            }
            else
            {
                PlayerPrefs.SetInt("Twy", 1);
                gameObject.SetActive(false);
            }
        }
        else //тут она просто стоит 
        {
            if(textI >= AskText.Length)
            {
                dissapear = true;
                return;
            }
            if (Vector2.Distance(Player.transform.position, transform.position) < 2f &&
       Mathf.Abs(Player.speedX) < 8 &&
       Player.health > 0)
            {
                HintText.text = Lang.GetInstance().GetTextOr(AskTextCodes, textI, AskText[textI]);
                HintText.color = new Color(1, 1, 1, 1);
                onetimeHint = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    answer = true;
                    answerTimer = 4.2f;
                }
            }
            else
            {
                if (onetimeHint)
                {
                    HintText.color = new Color(1, 1, 1, 0);
                    onetimeHint = false;
                }
            }

        }
    }
}
