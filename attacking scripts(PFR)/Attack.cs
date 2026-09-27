using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public enum AttackCategory { Hitbox, Projectile, Grab, MultiHit,
    Jab, Charge
}

[CreateAssetMenu(menuName = "Fighter/Attack")]
public abstract class Attack : ScriptableObject
{

    public string attackName;
    public bool isRunning = false;

    public int startupFrames;
    public int activeFrames;
    public int endlagFrames;

    public float damage;
    public Vector2 knockback;
    public Vector2 hitboxSize;
    public Vector2 hitboxOffset;

    public abstract void Execute(Player player);

    public virtual void OnEnd(Player player)
    {
       isRunning = false;
    }

    public virtual IEnumerator ChargePhase(Player player)
    {
        yield break; // default does nothing, coroutine just moves on
    }
}