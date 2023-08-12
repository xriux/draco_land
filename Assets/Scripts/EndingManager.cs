using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour {
    [SerializeField] private GameObject background;
    [SerializeField] private float scrollValue = 16.6f;

    void FixedUpdate() {
        Vector3 position = background.transform.position;
        position.x -= 0.05f;
        if (position.x < -scrollValue / 2) position.x += scrollValue;
        background.transform.position = position;

    }

    public void ToTitle() {
        DataManager.instance.MoveToScene("title");
    }
}
