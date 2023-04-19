using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject wheel;
    public Slider hpbar;
    public new ParticleSystem particleSystem;
    
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float jumpHeight = 6f;
    [SerializeField] float health = 100f;
    [SerializeField] public bool isGrounded = false;
    [HideInInspector] Rigidbody2D rb;
    [HideInInspector] static float dmg;
    [HideInInspector] static bool canDamage = false;
    [HideInInspector] float damageTimer = 0f; // seconds
    [HideInInspector] int groundLayer = 3;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        health = 100f;
    }

    void Update() {
        float speedmod = 100f / health;
        float _maxSpeed = maxSpeed * speedmod;
        float speed = maxSpeed * speedmod;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0));
            Vector3 angles = wheel.transform.eulerAngles;
            angles.z += speed / 10f;
            wheel.transform.eulerAngles = angles;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0));
            Vector3 angles = wheel.transform.eulerAngles;
            angles.z -= speed / 10f;
            wheel.transform.eulerAngles = angles;
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))) {
            // rb.AddForce(Vector3.up * jumpHeight, ForceMode2D.Force);
            Vector2 velo = rb.velocity;
            velo.y = jumpHeight;
            rb.velocity = velo;
            // transform.Translate(new Vector2(0f, jumpHeight));
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            health -= 5f;
        }

        damageTimer = Mathf.Clamp(damageTimer - Time.deltaTime, 0, damageTimer);
        canDamage = damageTimer == 0;
        if (dmg > 0) {
            particleSystem.Play();

            health -= dmg;
            dmg = 0;
            damageTimer += 0.5f;
        }

        hpbar.value = health / 100f;
        if (health <= 0) {
            Scenes.death();
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D) {
        if (collision2D.gameObject.layer == groundLayer) {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision2D) {
        if (collision2D.gameObject.layer == groundLayer) {
            isGrounded = false;
        }
    }

    public static void damage(float damage) {
        if (!canDamage) return;
        if (damage > dmg) {
            dmg = damage;
        }
    }
}
