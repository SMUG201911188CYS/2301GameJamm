using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RainManager : MonoBehaviour
{

    public PlayerController player;
    public AudioSource audioSource;
    int i = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (i == GameManager.GetLevel())
        {
            return;
        }
        else
        {
            OnRain();
        }
        i = GameManager.GetLevel();

    }

    public void OnRain()
    {
        print("비가 내린다");
        player.Rained();
        StartCoroutine(OnRainDuration(3.0f));

        

    }

    IEnumerator OnRainDuration(float duration)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(duration);
        transform.GetChild(0).gameObject.SetActive(false);
        audioSource.Stop();
    }
}
