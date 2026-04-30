using UnityEngine;

public class BattleTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject enemyGO;

    private bool alreadyDefeated = false;

    public Enemy Enemy => enemy;

    public void Interact()
    {
        if (alreadyDefeated) return;

        BattleController.Instance.StartBattle(this);
    }

    public void MarkAsDefeated()
    {
        alreadyDefeated = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        if (enemyGO != null)
            enemyGO.SetActive(false);

        gameObject.SetActive(false);
    }
}