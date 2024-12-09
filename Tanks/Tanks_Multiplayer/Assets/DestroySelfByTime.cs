using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfByTime : MonoBehaviour
{
    [SerializeField] float _destroyTimer;

    private void Update()
    {
        _destroyTimer -= Time.deltaTime;

        if(_destroyTimer < 0)
        {
            Destroy(gameObject);
        }
    }
}
