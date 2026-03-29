using UnityEngine;

public class ResetActivatable : Resettable
{
    public bool savedActiveState;
    void Start()
    {
        savedActiveState = GetComponent<Activatable>().activated;
    }
    protected override void ResetObject()
    {
        GetComponent<Activatable>().activated = savedActiveState;
    }
    protected override void SaveDefault()
    {
        savedActiveState = GetComponent<Activatable>().activated;
    }
}
