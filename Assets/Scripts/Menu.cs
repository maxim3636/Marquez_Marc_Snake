using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI bestScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bestScore.text = PlayerPrefs.GetInt("BestScore").ToString();
    }

    public void OnePlayer()
    {
        SceneManager.LoadScene(1);
    }
    public void TwoPlayer()
    {
        SceneManager.LoadScene(2);
    }
}
