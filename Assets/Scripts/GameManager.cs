using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    private int _enemy1Counter = 0;
    
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

    private int _enemy2Counter = 0;
    
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

    private int _enemy3Counter = 0;
    
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
    
    //MisteryShip
    [SerializeField]
    GameObject misteryShip;
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
    
    //NewRestartFuction
    private int activeEnemies;
    private Vector3 startPosition;
    private float _startY;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        _startY=currentY;
        scoreBestText.text = $"hi-score:\n{PlayerPrefs.GetInt("score")}";
        scoreText.text = $"Score:\n{score}";

        SpawnEnemies();
        
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

    private void SpawnEnemies()
    {
        activeEnemies = 0;
        _enemy1Counter = 0;
        _enemy2Counter = 0;
        _enemy3Counter = 0;
        //TODO to change in a single call or a Action Callback
        gameObject.GetComponent <EnemiesMover>().Unmute();
        
        SpawnEnemyType(new Color(0, 1, 0), enemyMaterial, ref _enemy1Counter, enemy1Rows, enemy1Cols, enemy1, enemy1b, enemy1Points,
            this,1);
        
        Debug.Log($"DY: {deltaY}");
        
        SpawnEnemyType(new Color(1, 1, 0.5F), enemyMaterial, ref _enemy2Counter, enemy2Rows, enemy2Cols, enemy2, enemy2b, enemy2Points,
            this,2);
        
        Debug.Log($"DY: {currentY}");

        SpawnEnemyType(new Color(1, 0.5F, 0), enemyMaterial, ref _enemy3Counter, enemy3Rows, enemy3Cols, enemy3, enemy3b, enemy3Points,
            this,3);
    }

    private void SpawnEnemyType(Color color, Material enemyMaterial,ref int counter, int enemyRows, int enemyCols, Sprite enemySprite1,
        Sprite enemySprite2, int enemyPoints, GameManager manager,int type)
    {
        Material eMaterial = new Material(enemyMaterial);
        eMaterial.color = color;

        for (int i = 0; i < enemyRows; i++) {
            for (int j = 0; j < enemyCols; j++) {
                GameObject go = Instantiate(enemyGO);
                go.SetActive(true);
                activeEnemies++;
                counter++;
                
                EnemyManager em = go.GetComponent<EnemyManager>();
                
                //TODO to change into a Action Callback
                em.Configure(enemySprite1, enemySprite2, eMaterial, enemyPoints, manager,type);
                em.OnHit = DidHitEnemy;
                
                go.transform.position = new Vector3(j * deltaX, currentY, 0);
                go.transform.parent = transform;
            }
            currentY -= deltaY;
        }
    }
    
    public void DidHitEnemy(int newPoints,int type) {
        score += newPoints;
        Debug.Log("SCORE: " +score);
        scoreText.text = $"Score:\n{score}";
        if (type <= 0)
        {
            return;
        }
        
        activeEnemies--;
        if (activeEnemies <= 0)
        {
            transform.position=startPosition;
            currentY=_startY;
            //TODO to change in a single call or a Action Callback
            gameObject.GetComponent <EnemiesMover>().StepReset();
            //TODO to change in a single call or a Action Callback
            gameObject.GetComponent <EnemiesMover>().Mute();
            Invoke("SpawnEnemies", 3);
            //SpawnEnemies();
        }
    }

    public void GameOver() {
        player.GetComponent<PlayerManager>().enabled = false;
        misteryShip.GetComponent<MisteryShipManager>().enabled = false;
        misteryShip.GetComponent<AudioSource>().Stop();
        gameOverCanvas.SetActive(true);

        int lastScore = PlayerPrefs.GetInt("score", 0);

        if (score > lastScore) {
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}