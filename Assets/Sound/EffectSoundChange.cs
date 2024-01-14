using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSoundChange : MonoBehaviour
{
    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip audio;
    }

    public BgmType[] BGMList;

    private AudioSource BGM;

    void Start()
    {
        BGM = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBGM(string name)
    {
        for (int i = 0; i < BGMList.Length; ++i)
            if (BGMList[i].name.Equals(name))
            {
                BGM.clip = BGMList[i].audio;
                BGM.Play();
            }
    }

}
