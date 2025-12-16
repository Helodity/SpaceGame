using System;
using UnityEngine;

[Serializable]
public struct BulletData
{
    [SerializeField] public int damage;
    [SerializeField] public float speed;
    [SerializeField] public float lifetime;
}
