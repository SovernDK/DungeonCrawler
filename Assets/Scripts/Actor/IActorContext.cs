using System.Collections;
using System.Collections.Generic;
using Crawler;
using UnityEngine;

public interface IActorContext
{
    public bool Done { get; set; }
    public bool Busy { get; set; }
    public string ActorName { get; set; }
    public bool Alive { get; }
    public MainContext MainContext { get; }

    public void Initialize(MainContext context) {  }
    public IEnumerator PlayAttack() { yield return null; }
    public IEnumerator PlayKnockback(TileController tileController) { yield return null; }
    public IEnumerator PlayMoveBlocked() { yield return null; }
    public IEnumerator PlayMove(TileController tileController) { yield return null; }
    public IEnumerator PlayRotate(float newRotation) { yield return null; }
    public IEnumerator PlaySpellCasting() { yield return null; }
    public IEnumerator PlayGetHit() { yield return null; }
    public IEnumerator PlayDeath() { yield return null; }
    public IEnumerator OnTurnStart() { yield return null; }
    public IEnumerator OnTurnEnd() { yield return null; }
    public IEnumerator OnActionStart() { yield return null; }
    public IEnumerator OnActionEnd() { yield return null; }
    public IEnumerator Act() { yield return null; }
    public void Knockback(int strength, Vector3Int direction) {}
}
