using UnityEngine;

public class EnemyAnimEventHandler : MonoBehaviour
{
    public void SetDeathAnimationEnd()
    {   
        GetComponentInParent<Enemy>().DeathAnimationEnded = true;
    }

    public void SetAttackAnimationEnd()
    {   
        GetComponentInParent<Enemy>().AttackAnimationEnded = true;
    }

    public void PlayEffect(string name)
    {
        Gamemaster.Instance.effectsManager.AddEffect(name, transform.position, Quaternion.identity, 0);
    }

    public void SetBusy()
    {
        GetComponentInParent<Enemy>().Busy = true;
    }

    public void SetNotBusy()
    {
        GetComponentInParent<Enemy>().Busy = false;
    }
}
