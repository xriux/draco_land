using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    [SerializeField] GameObject prefabBullet;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int jumpForce;
    [SerializeField] private int life = 3;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isFaceOpposite = false;
    private float shootCooldown = 0.5f;
    private float prevShootTime = 0f;

    private float damageCooldown = 1f;
    private float prevDamageTime = 0f;
    private SpriteRenderer sr;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        DataManager.instance.SetPlayerLife(life);
    }

    void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool pushUpKey = vertical > 0;
        bool pushDownKey = vertical < 0;
        isMoving = horizontal != 0;
        isFalling = rb.velocity.y < -0.5f;
        if (isFalling) isJumping = false;

        if (isMoving) {
            isFaceOpposite = horizontal < 0;
        }
        gameObject.transform.localScale = new Vector3(isFaceOpposite ? -1 : 1, 1, 1);

        bool isBlink = Time.time < prevDamageTime + damageCooldown;
        bool isBlinkClear = ((int)(Time.time / 0.1f)) % 2 == 0;
        sr.color = isBlink && isBlinkClear ? Color.clear : Color.white;

        if (gameObject.transform.position.y < -5) {
            DataManager.instance.PlaySE("fall");
            Damage(99);
        }

        if (pushDownKey) {
            if (Time.time >= prevShootTime + shootCooldown) {
                DataManager.instance.PlaySE("fire");
                Shoot();
                prevShootTime = Time.time;
            }
        }

        if (pushUpKey && !isJumping && !isFalling) {
            Jump();
        }
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsFalling", isFalling);
    }

    void Shoot() {
        GameObject g = Instantiate(prefabBullet);
        g.GetComponent<Bullet>().velocityX = 7f * (isFaceOpposite ? -1 : 1);
        g.transform.position = transform.position;
    }
    void Jump() {
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Damage(int damage = 1) {
        life -= damage;
        if (life <= 0) life = 0;
        DataManager.instance.SetPlayerLife(life);
        if (life <= 0) {
            DataManager.instance.AddParticles(24, transform.position);
            Destroy(gameObject);
            DataManager.instance.GameOver();
        }
    }

    void CollidingAction(Collision2D collision) {
        if (collision.collider.tag == "Enemy") {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            float colliderHeight = GetComponent<CapsuleCollider2D>().size.y;
            float footHeightRate = 0.25f;
            float footCheckHeight = transform.position.y + colliderHeight * (footHeightRate - 0.5f);
            bool isJump = false;
            bool isDamage = false;
            foreach (ContactPoint2D p in collision.contacts) {
                if (p.point.y < footCheckHeight) {
                    isJump = true;
                } else {
                    isDamage = true;
                }
            }
            if (isDamage) {
                if (Time.time >= prevDamageTime + damageCooldown) {
                    DataManager.instance.PlaySE("hit");
                    Damage();
                    prevDamageTime = Time.time;
                }
            }
            if (isJump) {
                DataManager.instance.PlaySE("punyo");
                Jump();
                enemy.HitAttack();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        CollidingAction(collision);
    }

    void OnCollisionStay2D(Collision2D collision) {
        CollidingAction(collision);
    }
}
