using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    Text scoreText;
    Text GameOverText;
    public GameObject head;
    public static int score = 0;
    public static bool isDead = true;

	// Use this for initialization
	void Start () {
        scoreText = GameObject.Find("scoreText").GetComponent<Text>();
        GameOverText = GameObject.Find("GameOver").GetComponent<Text>();
        GameOverText.gameObject.SetActive(true);
        score = 0;
        
	}
	
	// Update is called once per frame
	void Update () {

        if (isDead)
        {
            GameOverText.gameObject.SetActive(true);

            if (Input.anyKeyDown)
            {
                Restart();
                Instantiate(head, transform.position, Quaternion.identity);
                isDead = false;
            }
        }
        else
        {
            GameOverText.gameObject.SetActive(false);
        }
    }

    public void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    void Restart()
    {
        GameObject[] tails = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in tails)
        {
            Destroy(item);
        }

        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        foreach (var item in foods)
        {
            Destroy(item);
        }
    }
}
