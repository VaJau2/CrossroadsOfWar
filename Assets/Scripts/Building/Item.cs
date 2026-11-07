using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public Transform player;

    public int woodCount;
    public int foodCount;
    public int bowCount;

    public Sprite woodSprite;
    public Sprite foodSprite;
    public Sprite bowSprite;

    public float positionY;
    SpriteRenderer SR;

    Resourses res;

    bool onetimeHint;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        if (woodCount > 0) SR.sprite = woodSprite;
        else if (foodCount > 0) SR.sprite = foodSprite;
        else if (bowCount > 0) SR.sprite = bowSprite;

        res = GameObject.FindGameObjectWithTag("Resourses").GetComponent<Resourses>();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);

        if (woodCount > 0)
        {
            if (Vector2.Distance(transform.position, player.position) < 2.3f)
            {
                if (res.Wood + woodCount < res.WoodMax)
                {
                    res.AddResourses(woodCount, 0);
                    if (res.SaveItems.Contains(this)) res.SaveItems.Remove(this);

                    woodCount = 0;
                    PoolManager.putGameObjectToPool(gameObject);
                }
                else
                {
                    res.weapon.HintText.text = Lang.GetInstance().GetText("warehouse_full");
                    res.weapon.HintText.color = new Color(1, 1, 1, 1);
                    onetimeHint = true;             
                }
            }
            else
            {
                if(onetimeHint)
                {
                    res.weapon.HintText.color = new Color(1, 1, 1, 0);
                    onetimeHint = false;
                }
            }
        }
        else if (foodCount > 0)
        {
            if (res.Food + foodCount < res.FoodMax)
            {
                if (Vector2.Distance(transform.position, player.position) < 2.3f)
                {
                    res.AddResourses(0, foodCount);
                    if (res.SaveItems.Contains(this)) res.SaveItems.Remove(this);

                    foodCount = 0;
                    PoolManager.putGameObjectToPool(gameObject);
                }
            }
        }
        else if (bowCount > 0)
        {
            if (Vector2.Distance(transform.position, player.position) < 2.3f)
            {
                if (res.weapon.bowsCount + 1 < res.weapon.bowsMax)
                {
                    res.weapon.bowsCount++;
                    if (res.SaveItems.Contains(this)) res.SaveItems.Remove(this);

                    bowCount = 0;
                    PoolManager.putGameObjectToPool(gameObject);
                }
            }
        }
    }
}
