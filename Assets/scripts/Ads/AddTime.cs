using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AddTime : Rewarded
{
    [SerializeField]
    private LevelController controller;
    public override void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed(add_time)");
            _showAdButton.interactable = true;
            controller.stepCount += 3;
            controller.UpdateTextFields();
            LoadAd();
        }
    }
}
