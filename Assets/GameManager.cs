using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int requiredScore;
    public static int completionScore;
    public GameObject winUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        requiredScore = GameObject.FindGameObjectsWithTag("Resident").Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LevelComplete() //Checks if Level is complete
    {
        if (completionScore >= requiredScore)
        {
            //StartCoroutine("WIN"); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);//For now since we only have one level, I'm just reloading it
        }
    }
    public IEnumerator WIN()
    {
        winUI.SetActive(true);
        yield return new WaitForSeconds(5);
    }
}
