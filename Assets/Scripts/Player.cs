using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using System.Text;

public class IpInfoResponse {
    public string ip;
    public string city;
    public string region;
    public string country;
    public string loc;
    public string org;
    public string postal;
    public string timezone;
}

public class Player : MonoBehaviour {

    public GameObject wheel;
    public Slider hpbar;
    public GameObject blud;
    public ParticleSystem gas1;
    public ParticleSystem gas2;
    public Text timer;
    public Text fpsCounter;
    public Text ipText;
    public GameObject debugObject;
    public AudioClip movementSound;
    public AudioClip damageSound;

    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float jumpHeight = 6f;
    [SerializeField] float health = 100f;
    [SerializeField] float wheelSpeed = 10f;
    [SerializeField] float ipUpdate = 3f; // seconds
    [SerializeField] public bool isGrounded = false;
    [SerializeField] public bool debugEnabled = false;
    [HideInInspector] Rigidbody2D rb;
    [HideInInspector] static float dmg;
    [HideInInspector] static bool canDamage = false;
    [HideInInspector] float damageTimer = 0f; // seconds
    [HideInInspector] int groundLayer = 3;
    [HideInInspector] long startTime = 0;
    [HideInInspector] AudioSource audioSource;
    [HideInInspector] string userIp = null;
    [HideInInspector] System.Random random = new System.Random();

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        health = 100f;
        startTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        StartCoroutine(GetIP());
    }

    IEnumerator GetIP() {
        UnityWebRequest www = UnityWebRequest.Get("https://ipinfo.io/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        } else {
            IpInfoResponse resp = JsonUtility.FromJson<IpInfoResponse>(www.downloadHandler.text);

            userIp = resp.ip;
        }
    }

    public string RandomString(int length) {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
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

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            audioSource.PlayOneShot(movementSound); // Play movement sound (no way)
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
            Instantiate(blud, transform.position + new Vector3(0, -2.9f, -10), Quaternion.identity);
            audioSource.PlayOneShot(damageSound);

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

        /* Nice IP bozo */
        if (userIp != null) {
            int shown = (int) Math.Floor(diff / ipUpdate);
            Debug.Log($"shown {shown}");
            Debug.Log($"div {Math.Floor(diff / ipUpdate)}");
            Debug.Log($"update {ipUpdate}");

            if (shown > userIp.Length) {
                ipText.color = ((int) diff) % 2 == 0 ? new Color(1, 0, 0) : new Color(1, 1, 1);
                if (shown - userIp.Length >= (int) Math.Ceiling(10f / ipUpdate)) {
                    Scenes.death();
                }
                shown = userIp.Length;
            }
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < shown; i++) {
                builder.Append(userIp.ElementAt(i));
            }

            builder.Append(RandomString(userIp.Length - shown));

            ipText.text = builder.ToString();
        }

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
