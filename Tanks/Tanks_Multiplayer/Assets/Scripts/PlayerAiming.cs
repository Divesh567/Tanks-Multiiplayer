using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private Transform turrentTransform;
    private Vector2 aimPos;
    private bool doRotate;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        inputReader.OnWeaponRotated += HandleAiming;
        inputReader.OnFirePressed += StopAiming;

    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        inputReader.OnWeaponRotated -= HandleAiming;
        inputReader.OnFirePressed -= StopAiming;
    }


    private void LateUpdate()
    {
        if (!IsOwner) return;

        if (!doRotate) return;

        if (aimPos.x == 0 && aimPos.y == 0) return;

        float angle = Mathf.Atan2(aimPos.y, aimPos.x) * Mathf.Rad2Deg;

        turrentTransform.rotation = Quaternion.Euler(0, 0, angle - 90);

        doRotate = false;
       /* Vector2 mouseScreenPos = inputReader.aimPosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        turrentTransform.up = new Vector2
                                (
                                    mouseWorldPos.x - turrentTransform.position.x,
                                    mouseWorldPos.y - turrentTransform.position.y
                                );*/
    }

    public void HandleAiming(Vector2 aimPos)
    { 
        doRotate = true;
        this.aimPos = aimPos;
    }

    public void StopAiming(bool doFire, Vector2 aimPos)
    {
        doRotate = false;
    }
}
