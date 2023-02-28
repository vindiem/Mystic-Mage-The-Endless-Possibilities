using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Menager : NetworkBehaviour
{
    private void Update()
    {
        if (!isLocalPlayer) return;
    }
}
