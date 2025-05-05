using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class EnemiesMover : MonoBehaviour
{
    float deltaX = 5;
    float step = 1;
    int direction = 1;
    [SerializeField]
    float gameOverLowerY = -50;

    // Start is called before the first frame update
    void Start() {
        InvokeRepeating("Move", step, step);
    }

    void Move() {
        transform.position += deltaX * direction * Vector3.right;

        if (transform.position.x > 20 && direction == 1 || transform.position.x < -20 && direction == -1) {
            CancelInvoke();

            direction *= -1;
            transform.position -= Vector3.up;

            step -= 0.06f;

            if (step >= 0.08f) {
                InvokeRepeating("Move", step, step);
            }
            else
            {
                InvokeRepeating("Move", 0.09f, 0.09f);
            }
        }
        
        foreach (var childTransform in gameObject.GetComponentsInChildren<Transform>())
        {
            if (childTransform.position.y < gameOverLowerY)
            {
                //GameOver
                CancelInvoke();
                Debug.Log("Game Over" + childTransform.position, childTransform.gameObject);
                GetComponent<GameManager>().GameOver();
                enabled = false;
            }
        }
        /*if (transform.position.y < gameOverLowerY) {
            //GameOver
            CancelInvoke();
            GetComponent<GameManager>().GameOver();
            enabled = false;
            
        }*/
    }
}