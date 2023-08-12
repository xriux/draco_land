using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {
    IEnumerator Start() {
        float direction = Random.Range(0f, 2.0f * Mathf.PI);
        float speed = Random.Range(10.0f, 20.0f);
        Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();
        Vector2 v;
        v.x = Mathf.Cos(direction) * speed;
        v.y = Mathf.Sin(direction) * speed;
        rigidBody.velocity = v;

        while (transform.localScale.x > 0.01f) {
            yield return new WaitForSeconds(0.01f);
            transform.localScale *= 0.9f;
            rigidBody.velocity *= 0.9f;
        }

        Destroy(gameObject);
    }
}

