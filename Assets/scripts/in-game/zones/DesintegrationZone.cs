using UnityEngine;


public class DesintegrationZone : Zone
{
    public GameObject spotLight;
    public override void Worked()
    {
        base.Worked();
        spotLight.SetActive(false);
    }
}
