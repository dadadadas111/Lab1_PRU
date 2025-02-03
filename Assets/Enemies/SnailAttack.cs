using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailAttack : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public float DamagePerSecond = 1f;
    public int lastAttackTime = 0;

    // this script is attached to the snail prefab, snail can dealt damage to the player by touching it

    private void Update()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayer)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController.isRolling)
            {
                return;
            }
            if (Time.time - lastAttackTime >= 1)
            {
                player.GetComponent<HealthManager>().TakeDamage(DamagePerSecond);
                lastAttackTime = (int)Time.time;
            }
        }
    }

    // draw the attack range of the snail
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
