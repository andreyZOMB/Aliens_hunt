using UnityEngine;
using UnityEngine.Advertisements;

public class GiveAHint : Rewarded
{
    [SerializeField]
    private GameObject noHintCanvas;
    [SerializeField]
    private HintSystem hintSystem;

    //ѕеред показом подсказки провер€ет пройденный игроком путь на совпадение с правильным
    //¬ случае совпадени€ передаЄт управление в HintSystem
    public override void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed(GiveAHint)");
            _showAdButton.interactable = true;

            bool success = true;
            if (hintSystem.playerPath.Count > hintSystem.correctPath.Count)
            {
                success = false;
            }
            else
            {
                for (int i = 0; i < hintSystem.playerPath.Count; i++)
                {
                    if (!hintSystem.correctPath[i].Equals(hintSystem.playerPath[i]))
                    {
                        success = false;
                        break;
                    }
                }
            }
            if (success)
            {
                hintSystem.AddHints();
                hintSystem.ShowHint();
                LoadAd();
            }
            else
            {
                noHintCanvas.SetActive(true);
                _showAdButton.interactable = false;
            }

        }
    }
}
