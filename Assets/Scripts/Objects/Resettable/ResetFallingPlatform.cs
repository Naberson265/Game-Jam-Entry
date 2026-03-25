using UnityEngine;

public class ResetFallingPlatform : Resettable
{
    FallingPlatform platform;

    private void Start()
    {
        platform = gameObject.GetComponent<FallingPlatform>();
    }
    protected override void ResetObject()
    {
        platform.ResetObject();
    }

    protected override void SaveDefault()
    {
        return;
    }
}
