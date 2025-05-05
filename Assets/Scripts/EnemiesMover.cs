using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class EnemiesMover : MonoBehaviour
{
    float deltaX = 5;
    float step = 1;
    int direction = 1;
    
    //play sounds
    private bool _soundOne = true;
    private bool muted = false;
    AudioSource _audioSource;
    private AudioClip fastInvader1;
    private AudioClip fastInvader2;
    
    [SerializeField]
    float gameOverLowerY = -50;

    // Start is called before the first frame update
    void Start() {
        InvokeRepeating("Move", step, step);
        _audioSource = GetComponent<AudioSource>();
        fastInvader1 = Resources.Load<AudioClip>("Audios/fastinvader4");
        fastInvader2 = Resources.Load<AudioClip>("Audios/fastinvader2");
    }

    public void Mute()
    {
        muted = true;
    }
    public void Unmute()
    {
        muted = false;
    }
    
    public void StepReset()
    {
        step = 1;
    }
    private void FlipFlopSound()
    {
        if (muted) return;
        if (_soundOne)
        {
            _audioSource.PlayOneShot(fastInvader1);
        }
        else
        {
            _audioSource.PlayOneShot(fastInvader2);
        }
        
        
        _soundOne = !_soundOne;
    }
    void Move()
    {
        
        transform.position += deltaX * direction * Vector3.right;
        FlipFlopSound();
        
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