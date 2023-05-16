using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour {

    public GameObject wheel;
    public Slider hpbar;
    public ParticleSystem blud;
    public ParticleSystem gas1;
    public ParticleSystem gas2;
    public Text timer;
    public Text fpsCounter;
    public GameObject debugObject;

    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float jumpHeight = 6f;
    [SerializeField] float health = 100f;
    [SerializeField] float wheelSpeed = 10f;
    [SerializeField] public bool isGrounded = false;
    [SerializeField] public bool debugEnabled = false;
    [HideInInspector] Rigidbody2D rb;
    [HideInInspector] static float dmg;
    [HideInInspector] static bool canDamage = false;
    [HideInInspector] float damageTimer = 0f; // seconds
    [HideInInspector] int groundLayer = 3;
    [HideInInspector] long startTime = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        health = 100f;
        startTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }

    void Update() {
        /* Movement */
        float speedmod = 100f / health;
        float _maxSpeed = maxSpeed * speedmod;
        float speed = maxSpeed * speedmod * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(new Vector3(-speed, 0));
            Vector3 angles = wheel.transform.eulerAngles;
            angles.z += speed * wheelSpeed;
            wheel.transform.eulerAngles = angles;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.Translate(new Vector3(speed, 0));
            Vector3 angles = wheel.transform.eulerAngles;
            angles.z -= speed * wheelSpeed;
            wheel.transform.eulerAngles = angles;
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))) {
            Vector2 velo = rb.velocity;
            velo.y = jumpHeight;
            rb.velocity = velo;
            gas1.Play();
            gas2.Play();
        }

        /* Health & Health Bar */
        damageTimer = Mathf.Clamp(damageTimer - Time.deltaTime, 0, damageTimer);
        canDamage = damageTimer <= 0;
        if (dmg > 0) {
            blud.Play();

            health -= dmg;
            dmg = 0;
            damageTimer += 0.5f;
        }

        hpbar.value = health / 100f;
        if (health <= 0) {
            Scenes.death();
        }
        
        /* Timer */
        long diffLong = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - startTime;
        float diff = Convert.ToSingle(diffLong) / 1000;

        timer.text = $"{diff:F2}s";

        /* Y level check */
        if (gameObject.transform.position.y <= -10) {
            Scenes.death();
        }

        /* Debug */
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) {
            debugEnabled = !debugEnabled;
        }

        if (debugEnabled) {
            if (Input.GetKeyDown(KeyCode.H)) {
                health -= 5f;
            }

            fpsCounter.text = $"{Math.Round(1f / Time.deltaTime)} FPS";
        }

        debugObject.SetActive(debugEnabled);
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
