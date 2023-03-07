using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterTemplate : ScriptableObject
{
    public Sprite sprite;
    public Vector2 hitBox;
    public Vector3 rayCastOffset;
    public float stunTime;
    public float dashParticleSize;
    public float punchAmount;
    public float dashAmount;
    public float jumpHeight;
}
