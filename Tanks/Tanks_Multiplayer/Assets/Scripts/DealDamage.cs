using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField]
    private int damage = 15;

    public ulong ownerId;

    public bool hasHit;
    public bool CheckOwner(ulong ownerId)
    {
        if(this.ownerId == ownerId)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) return;

     
      

      /*  if (collision.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject nObj))
        {
            if (CheckOwner(nObj.NetworkObjectId)) return;
        }*/
    

        if(collision.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.ReduceHealth(damage);

        }
    }
}
