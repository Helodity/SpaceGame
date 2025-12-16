using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooting : Shooting
{
    protected override bool wantToShoot()
    {
        return true;
    }
}
