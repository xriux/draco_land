using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    [SerializeField] private float velocityX;
    [SerializeField] private float velocityY = 0;
    [SerializeField] private int life = 1;
    [SerializeField] private bool noGravity = false;
    [SerializeField] private bool isBoss = false;
    private Rigidbody2D rb;
    private float damageCooldown = 1f;
    private float prevDamageTime = 0f;
    private SpriteRenderer sr;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (isBoss) DataManager.instance.SetBossLife(life);
    }

    public void HitAttack() {
        if (Time.time >= prevDamageTime + damageCooldown) {
            Damage();
            prevDamageTime = Time.time;
        }
    }
    public void Damage(int damage = 1) {
        life -= damage;
        if (life <= 0) life = 0;
        if (isBoss) DataManager.instance.SetBossLife(life);
        if (life <= 0) {
            DataManager.instance.AddParticles(24, transform.position);
            Destroy(gameObject);
            if (isBoss) {
                GameObject.Find("Goal").transform.position = new Vector3(0, 0, 0);
            }
        }
    }

    void FixedUpdate() {
        bool isFaceOpposite = velocityX < 0;
        gameObject.transform.localScale = new Vector3(isFaceOpposite ? -1 : 1, 1, 1);
        if (!noGravity) velocityY = rb.velocity.y - 0.5f;
        rb.velocity = new Vector2(velocityX, velocityY);

        bool isBlink = Time.time < prevDamageTime + damageCooldown;
        bool isBlinkClear = ((int)(Time.time / 0.1f)) % 2 == 0;
        sr.color = isBlink && isBlinkClear ? Color.clear : Color.white;

        if (gameObject.transform.position.y < -5) {
            Damage(99);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Stage" || collider.tag == "Enemy") {
            velocityX = -velocityX;
        }
    }
}
