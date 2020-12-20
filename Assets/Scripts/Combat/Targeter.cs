using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    [SerializeField] Targetable target;

    public Targetable GetTarget()
    {
        return target;
    }


    #region Server

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget)) { return; }
        Debug.Log("CmdSetTarget was called");
        Debug.Log("Target is: " + targetGameObject);
        target = newTarget;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }

    #endregion

    #region Client

    #endregion
}
