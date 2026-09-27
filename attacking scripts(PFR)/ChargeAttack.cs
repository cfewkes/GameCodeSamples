using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Fighter/Attack/Charge")]
public class ChargeAttack : HitboxAttack
{
    public float maxChargeTime;
    public float maxDamageMultiplier;
    public float maxKnockbackMultiplier;
    private float dealtDamage;
    private float dealtKnockback;

    private float chargeTime;

    public override IEnumerator ChargePhase(Player player)
    {
        chargeTime = 0f;
        // hold attack button to charge
        while (player.attackHeld && chargeTime < maxChargeTime)
        {
            Debug.Log($"attackHeld: {player.attackHeld}, chargeTime: {chargeTime}, maxChargeTime: {maxChargeTime}");
            
            chargeTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log($"exited loop — attackHeld: {player.attackHeld}, chargeTime: {chargeTime}");
    }

    public override void Execute(Player player)
    {
        Debug.Log($"Executed: {Time.time}");
        dealtDamage = damage * Mathf.Lerp(1f, maxDamageMultiplier, chargeTime / maxChargeTime);
        dealtKnockback = damage * Mathf.Lerp(1f, maxKnockbackMultiplier, chargeTime / maxChargeTime);
        base.Execute(player);
    }

    AttackCategory Category => AttackCategory.Charge;
}