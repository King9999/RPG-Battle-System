using UnityEngine;

[CreateAssetMenu(menuName = "Action Gauge", fileName = "actGauge_")]
public class ActionGaugeData : ScriptableObject
{
    public enum ActionValue {Normal, Reduced, Miss, Critical, Special} //Reduced halves damage

    [Header("Action Values")]
    public ActionValue[] actValues = new ActionValue[10];
}
