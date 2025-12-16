using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    protected override bool wantToShoot()
    {
        return Input.GetMouseButton(0);
    }
}
