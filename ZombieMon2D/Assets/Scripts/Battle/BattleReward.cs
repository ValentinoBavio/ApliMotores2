using UnityEngine;

public class BattleReward : MonoBehaviour
{
    [SerializeField] private EnemyDifficulty difficulty;

    public int GetReward()
    {
        switch (difficulty)
        {
            case EnemyDifficulty.Easy:
                return 10;
            case EnemyDifficulty.Medium:
                return 20;
            case EnemyDifficulty.Hard:
                return 35;
            case EnemyDifficulty.Boss:
                return 75;
            default:
                return 0;
        }
    }
}