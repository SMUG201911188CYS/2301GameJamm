using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimeLineEnd : MonoBehaviour
{
    private PlayableDirector playableDirector;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playableDirector.stopped += OnTimelineFinished;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        SceneManager.LoadScene(2);
    }
}

