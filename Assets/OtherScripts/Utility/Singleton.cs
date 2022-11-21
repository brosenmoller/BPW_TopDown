using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
            
        }
    }

    //protected virtual void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = GetComponent<T>();
    //        DontDestroyOnLoad(instance.gameObject);
    //    }
    //    else if (instance.GetInstanceID() != GetInstanceID())
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
