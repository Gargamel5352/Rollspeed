using UnityEngine;

public class Damager : MonoBehaviour {
    public int damage;
    void OnCollisionStay2D(Collision2D collision2D) {
        Player.damage(damage);
    }
}
