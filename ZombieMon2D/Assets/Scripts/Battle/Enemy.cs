using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHP = 100;
    private int currentHP;

    [Header("Animation (opcional)")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void ResetHP()
    {
        currentHP = maxHP;

        if (animator != null)
            animator.Play("Idle");
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log(name + " recibe daño: " + amount);

        if (animator != null)
            animator.SetTrigger("Hurt");
    }


    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (animator != null)
            animator.SetTrigger("OnGuard");
    }

    public bool IsDead()
    {
        if (currentHP <= 0)
        {
            if (animator != null)
                animator.SetTrigger("Death");

            return true;
        }

        return false;
    }

    public int GetCurrentHP() => currentHP;
    public int GetMaxHP() => maxHP;

    public void PlayAttack()
    {
        Animator animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("No Animator");
            return;
        }

        animator.Play("Attack");
    }
}