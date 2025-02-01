using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public float patrolSpeed = 1f;
    public float detectionRange = 10f;
    public float patrolRange = 5f;

    private Vector2 startPosition;
    private Animator animator;
    private HealthManager healthManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        healthManager = GetComponent<HealthManager>();
    }

    private void Update()
    {
        if (player == null) return;

        if (healthManager.currentHealth <= 0)
        {
            return;
        }

        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

        if (distanceToPlayer <= detectionRange)
            ChasePlayer();
        else
            Patrol();
    }

    private void ChasePlayer()
    {
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);

        // Flip sprite correctly
        transform.localScale = new Vector3(player.position.x < transform.position.x ? -1 : 1, 1, 1);
    }

    private void Patrol()
    {
        Vector2 targetPosition = startPosition + new Vector2(patrolRange, 0);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);
        // Flip sprite correctly
        transform.localScale = new Vector3(targetPosition.x < transform.position.x ? -1 : 1, 1, 1);
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            startPosition = targetPosition;
            patrolSpeed *= -1;
        }
    }
}
