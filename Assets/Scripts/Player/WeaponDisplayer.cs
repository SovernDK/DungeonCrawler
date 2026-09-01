using UnityEngine;

public class WeaponDisplayer : MonoBehaviour
{
    private Player player;

    private void Start() 
    {
        player = GetComponentInParent<Player>();
    }

    public void IndicateEndOfWeaponAniamtion()
    {
        player.WeaponAnimationEnded = true;
    }
}
