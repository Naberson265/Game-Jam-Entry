using UnityEngine;

public class ResetTransform : Resettable
{
    Vector3 savedPosition;
    Quaternion savedRotation;
    Vector3 savedScale;

    private void Start()
    {
        SaveDefault();
    }

    protected override void ResetObject()
    {
        transform.position = savedPosition;
        transform.rotation = savedRotation;
        transform.localScale = savedScale;
    }

    protected override void SaveDefault()
    {
        savedPosition = transform.position;
        savedRotation = transform.rotation;
        savedScale = transform.localScale;
    }

}
