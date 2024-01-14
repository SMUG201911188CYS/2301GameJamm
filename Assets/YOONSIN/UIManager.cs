using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public float[] health; //체력 (1/4칸 : 0.25)
    public Image[] hearts; //풀 하트 이미지
    private static UIManager instance;
    public TMP_Text uiText;

    public GameObject overtext;
    public GameObject overButton;

    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<UIManager>();
                if(obj != null )
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<UIManager>();
                    instance = newObj;
                }
               
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<UIManager>();
        if( objs.Length != 1 )
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = health.Length-1 ; i >= 0; i--)
        {
            if (health[i] > 0)
            {
                //하트 한 칸마다 
                hearts[i].fillAmount = health[i];
                break;
            }else if (health[i] == 0)
            {
                hearts[i].fillAmount = health[i];
            }
            
        } 
    }

    public void UpdateHealthUI(float amount)
    {
        float damage = amount;

        for (int i = health.Length - 1; i >= 0; i--)
        {
            if (health[i] > 0)
            {
                //하트 한 칸마다
                float temp = health[i] - damage;
                if(temp >= 0)
                {
                    health[i] = temp;
                    //hearts[i].fillAmount = temp;
                    break;
                }else
                {
                    health[i] = 0;
                    //hearts[i].fillAmount = 0;
                    damage = Mathf.Abs(temp);
                }

               
               
            }

        }
    }

    public void SetValueToUIText(string value)
    {
        uiText.text = value;
    }

    public void UIGameOver()
    {
        overtext.SetActive(true);
        overButton.SetActive(true);
    }

    public void GoToTutorialScene()
    {
        SceneManager.LoadScene(2);
    }
}
