using System;
using UnityEngine;

public class VoiceHelperZone : Zone
{
    System.Random random = new();
    //ѕри вызове проигрывает звук голосового помошника и через случайные
    //промежутки времени включает звук на всех остальных голосовых помошниках на сцене
    public override void Worked()
    {
        base.Worked();
        VoiceHelperZone[] zones = FindObjectsOfType<VoiceHelperZone>();
        int index = Array.IndexOf(zones,this);
        zones[index] = null;
        foreach(VoiceHelperZone zone in zones)
        {
            if (zone != null)
            {
                float delay = moveTime - (float)random.NextDouble() * moveTime * 0.6f;
                Debug.Log(delay);
                zone.Invoke(nameof(PlayAudio), delay);
            }
        }
    }
}
