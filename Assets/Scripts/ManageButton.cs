using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartGame()
    {
        PlayerPrefs.SetInt("Jogadas", 0);
        PlayerPrefs.SetInt("recorde", 0);
        SceneManager.LoadScene("Lab3");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Lab3");
    }
}
