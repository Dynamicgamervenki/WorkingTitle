using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    Mechanics mechanics;

    void Start()
    {
        playableDirector.stopped += OnCutsceneEnded;
        mechanics = FindObjectOfType<Mechanics>();
    }

    void OnCutsceneEnded(PlayableDirector director)
    {
        Debug.Log("Cutscene has ended!");
        mechanics.inCutscene = false;
        mechanics.spiderEncounterCutScene.gameObject.SetActive(false);
        mechanics.virtualCamera01.gameObject.SetActive(false);
        mechanics.sideVirtualCamera.gameObject.SetActive(false);
        mechanics.spiderVirtualCamera.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        playableDirector.stopped -= OnCutsceneEnded;
    }
}
