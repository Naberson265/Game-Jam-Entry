using UnityEngine;

public class ResetDestroy : Resettable
{
    private bool exists = false;

    protected override void ResetObject()
    {
        if (!exists)
        {
            Destroy(gameObject);
        }
    }

    protected override void SaveDefault()
    {
        exists = true;
    }

}
