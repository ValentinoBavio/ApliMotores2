using UnityEngine;

public class BattleTrigger : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        BattleController.Instance.StartBattle();
    }

    //public void Interact()
    //{
    //    GameManager.Instance.SetState(GameState.Battle);
    //    BattleManager.Instance.StartBattle();
    //    Debug.Log("TURURURURURURURURU TU TU tutu tu ");
    //}
}
