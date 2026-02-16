using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance {get; private set;}

    public Color teamColor;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
