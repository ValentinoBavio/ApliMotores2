using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider enemySlider;

    public void Init(Enemy enemy)
    {
        playerSlider.maxValue = PlayerStats.Instance.GetMaxHP();
        playerSlider.value = PlayerStats.Instance.CurrentHP;

        enemySlider.maxValue = enemy.GetMaxHP();
        enemySlider.value = enemy.GetCurrentHP();
    }

    public void UpdatePlayerHP()
    {
        playerSlider.value = PlayerStats.Instance.CurrentHP;
    }

    public void UpdateEnemyHP(Enemy enemy)
    {
        enemySlider.value = enemy.GetCurrentHP();
    }
}