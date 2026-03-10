using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resettable : MonoBehaviour
{
    private static List<Resettable> objectList = new List<Resettable>();

    private void Awake()
    {
        objectList.Add(this);
    }

    abstract protected void ResetObject();
    abstract protected void SaveDefault();

    public static void SaveDefaults()
    {
        foreach (Resettable obj in objectList)
        {
            if (obj != null)
            {
                obj.SaveDefault();
            }
        }
    }

    public static void ResetAll()
    {
        foreach (Resettable obj in objectList)
        {
            if (obj != null)
            {
                obj.ResetObject();
            }
        }
    }
}
