using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [SerializeField] private float maxHP = 100;
    public float CurrentHP { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentHP = maxHP;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        CurrentHP -= dmg;
        if (CurrentHP < 0) CurrentHP = 0;

        Debug.Log("Player HP: " + CurrentHP);
    }

    public void Heal(float amount)
    {
        CurrentHP += amount;
        if (CurrentHP > maxHP) CurrentHP = maxHP;
    }

    public bool IsDead()
    {
        return CurrentHP <= 0;
    }
    public float GetMaxHP()
    {
        return maxHP;
    }
}
