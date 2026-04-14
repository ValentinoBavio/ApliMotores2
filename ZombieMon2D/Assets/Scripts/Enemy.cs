using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHP = 100;
    private float currentHP;

    public void ResetHP()
    {
        currentHP = maxHP;
        Debug.Log("Enemy HP: " + currentHP);
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;

        Debug.Log("Enemy HP: " + currentHP);
    }

    public void Heal(float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;

        Debug.Log("Enemy HP: " + currentHP);
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }
}
