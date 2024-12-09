using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaySuperUnity;
using System.Threading.Tasks;
using UnityEngine.UI;

public class playsupercontroller : MonoBehaviour
{


    internal string playSuperAppKey = "1efbcce187492d466dcd742616636b78988327879e17a803799c7cea74590994";
    internal string playSuperXPID = "0cbfd3f8-8a1d-4f60-b9ae-cbbd619e6400";

    public Button openStoreButton;

    private void Awake()
    {
        PlaySuperUnitySDK.Initialize(playSuperAppKey);
    }

    public void Start()
    {
        openStoreButton.onClick.AddListener(() => PlaySuperUnitySDK.Instance.OpenStore());
    }



}
