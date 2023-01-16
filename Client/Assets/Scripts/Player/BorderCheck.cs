using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BorderCheck : MonoBehaviour
{

    private bool trigger = false;
    private float timer = 5f;

    [SerializeField] public Text timerText;
    [SerializeField] public GameObject warnUI;

    private void Update() {
        if (trigger) {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F1");
        }
    }

    private void enterMap() {
        warnUI.SetActive(false);
        trigger = false;
        timer = 5f;
    }

    private void exitMap() {
        warnUI.SetActive(true);
        trigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        enterMap();
    }

    private void OnTriggerExit2D(Collider2D other) {
        exitMap();
    }

}
