using UnityEngine;

[CreateAssetMenu(menuName = "Fighter/Attack/Jab")]
public class JabAttack : HitboxAttack { 

    AttackCategory Category => AttackCategory.Jab;
}