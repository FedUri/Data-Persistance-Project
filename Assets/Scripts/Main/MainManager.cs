using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    //texto en pantalla
    public TextMeshProUGUI bestScore;

    //4 variables
    private static int bestPoint  = 0;
    private static string bestPlayer = "--";
    public string currentPlayer;

    public static MainManager instance;
    


    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        currentPlayer = MenuManager.playerString;
    }

    public void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        WhoIsBestPlayer();
        ShowBestScore();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void StartGame()
    {
        LoadBestScore();
        SceneManager.LoadScene(1);
    }

    public void ShowBestScore()
    {
        bestScore.text = "Best Score: " + bestPlayer + "  Score: " + bestPoint;
    }

    public void WhoIsBestPlayer()
    {
        if(m_Points > bestPoint)
        {
            bestPoint = m_Points;
            bestPlayer = currentPlayer;
            
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {


#if UNITY_EDITOR
        SaveBestScore();
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void ResetScore()
    {
        bestPoint = 0;
        bestPlayer = "--";
        SaveBestScore();
     
    }

    //datos que se guardan entre sesiones
    [System.Serializable]
    class SaveData
    {

        public string bestPlayer;
        public int bestPoint;
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestPlayer = bestPlayer;
        data.bestPoint = bestPoint;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.bestPlayer;
            bestPoint = data.bestPoint;
        }
    }


}
