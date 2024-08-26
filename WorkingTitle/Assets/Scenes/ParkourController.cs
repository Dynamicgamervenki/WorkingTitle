using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    EnvironmentScanner environmentScanner;
    Mechanics playerController;
    Animator anim;

    [Header("Actions")]
    public bool inAction;
    public List<ParkourAction> actions = new List<ParkourAction>();

    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        playerController = GetComponent<Mechanics>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObstacleHitData obstacleHitData = environmentScanner.ObstacleCheck();
            if (obstacleHitData.forwordHitFound)
            {
                foreach (ParkourAction action in actions)
                {
                    if (action.CheckIfPossible(obstacleHitData, this.transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;
                    }
                }
            }
        }

    }

    IEnumerator DoParkourAction(ParkourAction action)
    {
        var name = action.animName;

        inAction = true;

        anim.SetBool("mirrorAction", action.Mirror);
        playerController.OnControl(false);
        anim.Play(name, 0);

        yield return null;

        var animState = anim.GetNextAnimatorStateInfo(0);
        if (!animState.IsName(name))
            Debug.LogError("The parkour animation name is wrong !");


        float timer = 0f;
        while (timer <= animState.length)
        {
            timer += Time.deltaTime;
            if (action.rotateToObstacle)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, playerController.rotationSpeed * Time.deltaTime);
            }

            if (action.enableTargetMatching)
            {
                TargetMatching(action);
            }

            if (anim.IsInTransition(0) && timer > 0.5f)
                break;


            yield return null;
        }
        yield return new WaitForSeconds(action.postActionDelay);

        playerController.OnControl(true);
        inAction = false;
    }

    private void TargetMatching(ParkourAction action)
    {
        if (anim.isMatchingTarget)
            return;

        anim.MatchTarget(action.MatchPos, transform.rotation, action.avatarTarget, new MatchTargetWeightMask(action.matchposWeight, 0), action.matchStartTym, action.matchTargetTym);
    }


}
