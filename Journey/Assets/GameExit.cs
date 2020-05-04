using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameExit : MonoBehaviour
{
    public bool exit = false;
    public float swipeAmount = 0f;
    public Image fade;

    private void Update() {
        if(exit) { SceneManager.LoadScene("MainMenu"); }
        fade.color = new Color(1, 1, 1, swipeAmount);
    }
    private void OnTriggerEnter(Collider other) {
        GetComponent<Animator>().Play("Exit");
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
