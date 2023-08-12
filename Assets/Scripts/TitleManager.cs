using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {
    [SerializeField] private GameObject background;
    [SerializeField] private float scrollValue = 10.8f;

    void FixedUpdate() {
        Vector3 position = background.transform.position;
        position.y -= 0.03f;
        if (position.y < -scrollValue / 2) position.y += scrollValue;
        background.transform.position = position;
    }

    public void StartGame() {
        DataManager.instance.MoveToScene("stage1");
    }
}
