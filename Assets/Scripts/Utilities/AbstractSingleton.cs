using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance != null) 
                return _instance;

            _instance = FindObjectOfType<T>();

            if (_instance != null)
                return _instance;

            GameObject container = new(typeof(T).Name);
            _instance = container.AddComponent<T>();

            return _instance;
        }
    }
}
