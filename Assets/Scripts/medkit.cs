using UnityEngine;

public class medkit : MonoBehaviour {
    public int RecoverHealth;
    void OnCollisionEnter2D(Collision2D collision2D) {
        GameObject.FindWithTag("Player1").GetComponent<Player>().health += RecoverHealth;
        Destroy(this.gameObject);
    }
}
