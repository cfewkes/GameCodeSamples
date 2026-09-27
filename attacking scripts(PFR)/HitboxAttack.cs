using System.Linq;
using UnityEngine;
using static Player;

public abstract class HitboxAttack : Attack
{
    public AttackCategory category;
    public override void Execute(Player player)
    {
        isRunning = true;
        Vector2 flippedOffset = new Vector2(hitboxOffset.x * player.facingRight, hitboxOffset.y);
        Vector2 position = (Vector2)player.transform.position + flippedOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(position, hitboxSize, 0f, player.hurtLayer);
        foreach (var hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                if (health.isShielding && category != AttackCategory.Grab)
                    continue; // shield blocks everything except grabs
                health.TakeDamage(damage, knockback * player.facingRight, health.GetDIInput(), player.gameObject);
            }
        }


    }
    public void DrawGizmo(Player player)
    {
        if (isRunning)
        {
            Gizmos.color = Color.red;
            Vector2 flippedOffset = new Vector2(hitboxOffset.x * player.facingRight, hitboxOffset.y);
            Gizmos.DrawWireCube((Vector2)player.transform.position + flippedOffset, hitboxSize);
        }
        
    }

}
