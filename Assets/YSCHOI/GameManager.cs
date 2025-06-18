using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float maxHealth;
    float currentHealth;
    float score;
    float count_get_flower;
    private int level;



    GameObject soundManager;
    GameObject effectManger;
    RainManager rainManager;

    private static GameManager instance;
    bool [] score_check = {true, true, true, true};

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>(); ;
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    public static int GetLevel()
    {
        return instance.level;
    }

    public static void SetLevel(int amount)
    {
        Instance.level = amount;
    }


    void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if(objs.Length != 1) 
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);

        maxHealth = 3.0f;
        currentHealth = maxHealth;
       
    }

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("MainBGM");
        effectManger = GameObject.Find("EffectBGM");
        rainManager = GameObject.Find("RainManager").GetComponent<RainManager>();
    }



    // Update is called once per frame
    void Update()
    {
        UIManager.Instance.SetValueToUIText("Score: " + score);
        if(score > 4000 && score_check[3])
        {
            SetLevel(5);
            score_check[3] = false;
            soundManager.GetComponent<SoundChange>().PlayBGM("level5");
            
        }
        else if(score > 3000 && score_check[2])
        {
            SetLevel(4);
            score_check[2] = false;
            soundManager.GetComponent<SoundChange>().PlayBGM("level4");
            
        }
        else if(score > 2000 && score_check[1])
        {
            SetLevel(3);
            score_check[1] = false;
            soundManager.GetComponent<SoundChange>().PlayBGM("level3");
            
        }
        else if(score > 1000 && score_check[0])
        {
            SetLevel(2);
            score_check[0] = false;
            soundManager.GetComponent<SoundChange>().PlayBGM("level2");
            
        }
        else SetLevel(1);
    }

    public void LostHealth(float amount)
    {
        currentHealth -= amount;
        if(currentHealth >= 0)
        {
            
            UIManager.Instance.UpdateHealthUI(amount);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("GameOver");
            UIManager.Instance.UIGameOver();
            Time.timeScale = 0.0f;
            //gameOver
        }
    }

    public void GetHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > 3)
        {
            currentHealth = 3;
            UIManager.Instance.UpdateHealthUI(amount);
        }

        if (currentHealth <= 0)
        {
            UIManager.Instance.UpdateHealthUI(amount);
        }
    }
    
    public void CountFlower()
    {
        count_get_flower++;

        if(count_get_flower == 5)
        {
            Debug.Log("Game end");
        }
    }

    public void GetScore(float amount, string object_type)
    {
        score += amount;
        
        if (object_type == "Trash")
        {
            effectManger.GetComponent<EffectSoundChange>().PlayBGM("Explosion");
        }
        else if (object_type == "FastFood")
        {
            effectManger.GetComponent<EffectSoundChange>().PlayBGM("FastFood");
        }
        else if (object_type == "Flower")
        {
            effectManger.GetComponent<EffectSoundChange>().PlayBGM("Flower");
        }
        else if (object_type == "Cake")
        {
            effectManger.GetComponent<EffectSoundChange>().PlayBGM("Eat");
        }
        //이후 ui 업데이트 추가좀요
        Debug.Log(score);
    }


}
