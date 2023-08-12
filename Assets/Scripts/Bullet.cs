using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float velocityX = 3f;
    private Rigidbody2D rb;
    private float timePassed = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        timePassed += Time.deltaTime;
        if (timePassed > 1f) Destroy(gameObject);

    }

    void FixedUpdate() {
        bool isFaceOpposite = velocityX < 0;
        gameObject.transform.localScale = new Vector3(isFaceOpposite ? -1 : 1, 1, 1);
        rb.velocity = new Vector2(velocityX, 0);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            enemy.HitAttack();
            DataManager.instance.PlaySE("punch");
            Destroy(gameObject);
        }
        if (collider.tag == "Stage") {
            Destroy(gameObject);
        }
    }
}
