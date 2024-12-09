using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    public ClientSingleton clientSingletonPrefab;

    public HostSingleton hostSingletonPrefab;

    // Start is called before the first frame update
    async void Start()
    {
        DontDestroyOnLoad(this);

       await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {

        }
        else
        {
            HostSingleton hostSingleton = Instantiate(hostSingletonPrefab);

            hostSingleton.CreateHostManager();


            ClientSingleton clientSingleton = Instantiate(clientSingletonPrefab);
            bool authenticated = await clientSingleton.CreateClientManager();

          

            if (authenticated)
            {
                clientSingleton.clientGameManager.GoToMenu();
            }
        }
    }
}
