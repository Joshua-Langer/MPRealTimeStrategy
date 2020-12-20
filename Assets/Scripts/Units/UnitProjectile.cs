using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] Rigidbody rb = null;
    [SerializeField] float lifeTime = 5f;
    [SerializeField] float launchForce = 10f;

    void Start()
    {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), lifeTime);
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

}
