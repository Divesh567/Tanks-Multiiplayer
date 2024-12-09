using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    private static ClientSingleton instance;


    public ClientGameManager clientGameManager;
    public static ClientSingleton Instance 
    
    {
        get
        {
            if (instance != null) { return instance; }

            if(instance == null)
            {
                instance = FindAnyObjectByType<ClientSingleton>();
            }

            if(instance == null)
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

    public async Task<bool> CreateClientManager()
    {
        clientGameManager = new ClientGameManager();

        return await clientGameManager.InitAsync();

    }

    private void OnDestroy()
    {
        clientGameManager?.Dispose();
    }

}
