using UnityEngine;
using TMPro;

public class BattleRewardManager : MonoBehaviour
{
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private GameObject rewardPanel;

    public void GiveReward(BattleReward enemyReward)
    {
        if (enemyReward == null)
        {
            Debug.LogWarning("No se encontró EnemyReward en el enemigo.");
            return;
        }

        int reward = enemyReward.GetReward();
        CurrencyManager.Instance.AddCoins(reward);

        if (rewardPanel != null)
            rewardPanel.SetActive(true);

        if (rewardText != null)
            rewardText.text = "Ganaste " + reward + " monedas";
    }

    public void CloseRewardPanel()
    {
        if (rewardPanel != null)
            rewardPanel.SetActive(false);
    }
}