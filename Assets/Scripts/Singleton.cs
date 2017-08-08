using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent class for the varous manager functions
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour //last part allows FindObjectOfType
{
    public static T instance;

    public static T Instance
    {
        get
        {
            //create instance if none yet
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }
}
