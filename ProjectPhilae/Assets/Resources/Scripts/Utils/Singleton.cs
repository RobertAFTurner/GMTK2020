using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public static Singleton<T> instance;

    protected void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
}

public class SingletonDestructible<T> : MonoBehaviour
{
    public static SingletonDestructible<T> instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
}