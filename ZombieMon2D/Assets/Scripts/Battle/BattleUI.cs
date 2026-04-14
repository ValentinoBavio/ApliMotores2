using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public void OnFight()
    {
        BattleController.Instance.Fight();
    }

    public void OnRun()
    {
        BattleController.Instance.Run();
    }
}
