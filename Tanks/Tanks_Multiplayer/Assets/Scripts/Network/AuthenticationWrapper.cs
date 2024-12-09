using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper 
{

    public static AuthState AuthState { get; private set; } = AuthState.NotAutenticated;

    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        if(AuthState == AuthState.Authenticated)
        {
            return AuthState.Authenticated;
        }

        if(AuthState == AuthState.Autenticating)
        {
            Debug.LogWarning("Is already authenticating");
            await Authenticating();
            return AuthState;
        }


        await SignInAnonymouslyAsync(maxTries);

        return AuthState;
    }

    public static async Task<AuthState> Authenticating()
    {
        while(AuthState == AuthState.Autenticating || AuthState == AuthState.NotAutenticated)
        {
            await Task.Delay(200);
        }

        return AuthState;
    }

    public static async Task SignInAnonymouslyAsync(int maxTries)
    {

        AuthState = AuthState.Autenticating;

        int tries = 0;
        while (AuthState == AuthState.Autenticating && tries <= maxTries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
              
            }
            catch(AuthenticationException ex)
            {
                Debug.LogError(ex);
                AuthState = AuthState.Error;
            }
            catch (RequestFailedException ex)
            {
                Debug.LogError(ex);
                AuthState = AuthState.Error;
            }


            tries++;
            await Task.Delay(1000);
        }

        if(AuthState != AuthState.Authenticated)
        {
            Debug.LogWarning("Not able to sign in sucessfully please try again in sometime");
            AuthState = AuthState.TimeOut;
        }

    }

}

public enum AuthState
{
    Authenticated,
    Autenticating,
    NotAutenticated,
    Error,
    TimeOut,
}
