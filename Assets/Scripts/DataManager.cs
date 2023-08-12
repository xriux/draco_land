using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour {
    public static DataManager instance = null;
    public GameObject particlePrefab = null;
    private GameObject canvasGameOver = null;
    private Text playerLifeText;
    private Text stageText;
    private Text bossLifeText;

    AudioSource getSE;
    AudioSource hitSE;
    AudioSource fireSE;
    AudioSource punyoSE;
    AudioSource fallSE;
    AudioSource punchSE;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            canvasGameOver = GameObject.Find("GameOver");
            playerLifeText = GameObject.Find("LifeText").GetComponent<Text>();
            stageText = GameObject.Find("StageText").GetComponent<Text>();
            bossLifeText = GameObject.Find("BossText").GetComponent<Text>();
            Component[] audios = GetComponentsInChildren<AudioSource>();
            getSE = audios[0] as AudioSource;
            hitSE = audios[1] as AudioSource;
            fireSE = audios[2] as AudioSource;
            punyoSE = audios[3] as AudioSource;
            fallSE = audios[4] as AudioSource;
            punchSE = audios[5] as AudioSource;
        } else {
            Destroy(this.gameObject);
        }
        instance.InitStage();
    }

    public void InitStage() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (playerLifeText) {
            playerLifeText.text = "";
        }
        if (bossLifeText) {
            bossLifeText.text = "";
        }
        if (stageText) {
            stageText.text = "";
            if (sceneName == "stage1") stageText.text = "STAGE 1";
            if (sceneName == "stage2") stageText.text = "STAGE 2";
            if (sceneName == "stage3") stageText.text = "STAGE 3";
        }
        if (canvasGameOver) canvasGameOver.SetActive(false);
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MoveToScene(string scene) {
        PlaySE("get");
        SceneManager.LoadScene(scene);
    }

    public void SetPlayerLife(int life) {
        if (!playerLifeText) return;
        playerLifeText.text = "LIFE: " + life.ToString();
    }

    public void SetBossLife(int life) {
        if (!bossLifeText) return;
        bossLifeText.text = "BOSS: " + life.ToString();
    }

    public void GameOver() {
        if (!canvasGameOver) return;
        canvasGameOver.SetActive(true);
    }

    public void PlaySE(string id) {
        if (id == "get") getSE.Play();
        if (id == "hit") hitSE.Play();
        if (id == "fire") fireSE.Play();
        if (id == "punyo") punyoSE.Play();
        if (id == "fall") fallSE.Play();
        if (id == "punch") punchSE.Play();
    }

    public void AddParticles(int num, Vector3 position) {
        for (int i = 0; i < num; i++) {
            GameObject g = Instantiate(particlePrefab);
            g.transform.position = new Vector3(position.x, position.y, 0);
        }
    }
}
