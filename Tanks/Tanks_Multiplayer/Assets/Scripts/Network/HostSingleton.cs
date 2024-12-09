using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance;

    public HostGameManager hostGameManger;
    public static HostSingleton Instance
    {
        get
        {
            if (instance != null) { return instance; }

            if (instance == null)
            {
                instance = FindAnyObjectByType<HostSingleton>();
            }

            if (instance == null)
            {
                Debug.LogError("NO CLIENT SINGLETON IN SCENE");
                return null;
            }

            return instance;
        }

    }

    private void Start()
    {
        DontDestroyOnLoad(this);

    }


    public void CreateHostManager()
    {
        hostGameManger = new HostGameManager();

    }

    private void OnDestroy()
    {
        hostGameManger?.Dispose();
    }

}
