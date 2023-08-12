using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    [SerializeField] private string nextScene;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            DataManager.instance.MoveToScene(nextScene);
        }
    }
}
