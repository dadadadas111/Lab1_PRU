using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // a slider to represent the health bar
    public Slider healthBar;

    // the max health of the player
    public float maxHealth = 100f;

    // the current health of the player
    public float currentHealth;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10f);
        }

    }


    public void TakeDamage(float damage)
    {
        // subtract the damage from the current health
        currentHealth -= damage;
        // update the health bar
        healthBar.value = currentHealth / maxHealth;
        // check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // destroy the player
        //Destroy(gameObject);
    }

    public void Heal(float healAmount)
    {
        // add the heal amount to the current health
        currentHealth += healAmount;
        // make sure the current health does not exceed the max health
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        // update the health bar
        healthBar.value = currentHealth / maxHealth;
    }
}
