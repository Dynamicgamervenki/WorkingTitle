using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Video;
using UnityEngine.Playables;
using UnityEngine.Video;

public class IntroCutsceneManager : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public GameObject Tv;
    private VideoPlayer[] videoPlayers;

    public ParticleSystem sparksEffect;
    public ParticleSystem smokeEffect;

    public Light roomLight;
    public Light redLight;

    public float dangerClipTime = 22.0f;
    public float vfxPlayTime = 19.0f;

    VideoPlayer gameplay;
    VideoPlayer danger;

    public GameObject friend;

    void Start()
    {
        playableDirector.stopped += OnCutsceneEnded;

        StartCoroutine(CheckTimelineTime());

        videoPlayers = Tv.GetComponents<VideoPlayer>();

        // Access the video players individually
        if (videoPlayers.Length > 1)
        {
            gameplay = videoPlayers[0];
            danger = videoPlayers[1];
        }
        Tv.gameObject.SetActive(true);
        gameplay.Play();
    }


    void OnCutsceneEnded(PlayableDirector director)
    {
        Debug.Log("Cutscene has ended!");
    }

    void OnDestroy()
    {
        playableDirector.stopped -= OnCutsceneEnded;
    }

    IEnumerator CheckTimelineTime()
    {
        while (playableDirector.state == PlayState.Playing)
        {
            if (playableDirector.time >= vfxPlayTime)
            {
                smokeEffect.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                sparksEffect.gameObject.SetActive(true);
            }

            if (playableDirector.time >= dangerClipTime)
            {
                gameplay.Pause();
                danger.Play();
            }
            if(playableDirector.time >= 29.0 && playableDirector.time  < 29.90f)
                roomLight.gameObject.SetActive(false);

            if(playableDirector.time >= 33f)
            {
                roomLight.gameObject.SetActive(false);
                redLight.gameObject.SetActive(true); // Red light starts on when room light is off

                int toggleCount = 10;
                float delay = 0.1f;

                for (int i = 0; i < toggleCount; i++)
                {
                    yield return new WaitForSeconds(delay);

                    // Toggle room light and red light alternatively
                    bool isRoomLightActive = roomLight.gameObject.activeSelf;
                    roomLight.gameObject.SetActive(!isRoomLightActive);
                    redLight.gameObject.SetActive(isRoomLightActive);
                }
                smokeEffect.gameObject.transform.localScale = new Vector3 (5f, 5f, 5f);
                // Ensure the room light ends in the correct state (off) and red light on
                roomLight.gameObject.SetActive(false);
                redLight.gameObject.SetActive(false);
                friend.gameObject.SetActive(false);
            }
            //if(playableDirector.time >= 35.0f)
            //{
            //    roomLight.gameObject.SetActive(false);
            //    redLight.gameObject.SetActive(false);


            //}
            yield return null; // Wait until the next frame
        }
    }
}
