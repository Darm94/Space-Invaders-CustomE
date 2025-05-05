using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Enemies setup
    [SerializeField]
    Material enemyMaterial;

    //Enemy 1st row top
    [SerializeField]
    Sprite enemy1;

    [SerializeField]
    Sprite enemy1b;

    [SerializeField]
    int enemy1Cols = 11;

    [SerializeField]
    int enemy1Rows = 1;

    [SerializeField]
    int enemy1Points = 30;

    //Enemy 2nd and 3rd row middle
    [SerializeField]
    Sprite enemy2;

    [SerializeField]
    Sprite enemy2b;

    [SerializeField]
    int enemy2Cols = 11;

    [SerializeField]
    int enemy2Rows = 2;

    [SerializeField]
    int enemy2Points = 20;

    //Enemy 4th and 5th row bottom
    [SerializeField]
    Sprite enemy3;

    [SerializeField]
    Sprite enemy3b;

    [SerializeField]
    int enemy3Cols = 11;
    
    [SerializeField]
    int enemy3Rows = 2;

    [SerializeField]
    int enemy3Points = 10;

    [SerializeField]
    GameObject enemyGO;

    [SerializeField]
    float deltaX = 1;

    [SerializeField]
    float deltaY = 1;

    float currentY = 0;

    //Barriers setup
    [SerializeField]
    Material barrierMaterial;

    [SerializeField]
    GameObject barrierGO;

    [SerializeField]
    Sprite barrier;

    [SerializeField]
    int barriers = 4;

    [SerializeField]
    Vector3 barriersDelta = new Vector3(1, 0, 0);

    [SerializeField]
    Vector3 barriersStart;

    //Player
    [SerializeField]
    GameObject player;

    //Score
    int score = 0;

    //GUI
    [SerializeField]
    TMP_Text scoreText;

    [SerializeField]
    TMP_Text scoreBestText;
    
    //GameOver
    [SerializeField]
    GameObject gameOverCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreBestText.text = $"hi-score:\n{PlayerPrefs.GetInt("score")}";
        scoreText.text = $"Score:\n{score}";

        Material e1Material = new Material(enemyMaterial);
        e1Material.color = new Color(0, 1, 0);

        for (int i = 0; i < enemy1Rows; i++) {
            for (int j = 0; j < enemy1Cols; j++) {
                Debug.Log($"Loop GM: " + i + " : " + j);

                GameObject go = Instantiate(enemyGO);
                go.SetActive(true);

                EnemyManager em = go.GetComponent<EnemyManager>();
                em.Configure(enemy1, enemy1b, e1Material, enemy1Points, this);

                go.transform.position = new Vector3(j * deltaX, currentY, 0);
                go.transform.parent = transform;
            }

            currentY -= deltaY;
        }

        Debug.Log($"DY: {deltaY}");

        Material e2Material = new Material(enemyMaterial);
        e2Material.color = new Color(1, 1, 0.5F);

        for (int i = 0; i < enemy2Rows; i++) {
            for (int j = 0; j < enemy2Cols; j++) {
                GameObject go = Instantiate(enemyGO);
                go.SetActive(true);

                EnemyManager em = go.GetComponent<EnemyManager>();
                em.Configure(enemy2, enemy2b, e2Material, enemy2Points, this);

                go.transform.position = new Vector3(j * deltaX, currentY, 0);
                go.transform.parent = transform;
            }

            currentY -= deltaY;
        }
        
        Debug.Log($"DY: {currentY}");
        
        Material e3Material = new Material(enemyMaterial);
        e2Material.color = new Color(1, 0.5F, 0);

        for (int i = 0; i < enemy3Rows; i++) {
            for (int j = 0; j < enemy3Cols; j++) {
                GameObject go = Instantiate(enemyGO);
                go.SetActive(true);

                EnemyManager em = go.GetComponent<EnemyManager>();
                em.Configure(enemy3, enemy3b, e3Material, enemy3Points, this);

                go.transform.position = new Vector3(j * deltaX, currentY, 0);
                go.transform.parent = transform;
            }

            currentY -= deltaY;
        }
        
        //Barriers
        for (int i = 0; i < barriers; i++) {
            GameObject go = Instantiate(barrierGO);
            go.name = $"Barrier #{i}";
            go.SetActive(true);

            BarrierManager bm = go.GetComponent<BarrierManager>();
            bm.Configure(barrier, new Color(1, 1, 1), barrierMaterial);

            go.transform.position = new Vector3(
                barriersStart.x + i * barriersDelta.x,
                barriersStart.y,
                barriersStart.z
            );
        }
        
    }
    
    public void DidHitEnemy(int newPoints) {
        score += newPoints;
        Debug.Log("SCORE: " +score);
        scoreText.text = $"Score:\n{score}";
    }

    public void GameOver() {
        player.GetComponent<PlayerManager>().enabled = false;
        gameOverCanvas.SetActive(true);

        int lastScore = PlayerPrefs.GetInt("score", 0);

        if (score > lastScore) {
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();
        }
    }
    
}