using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    Vector3 startPotision;

    [SerializeField]
    float horizontalDelta = 50;

    [SerializeField]
    Sprite player;

    [SerializeField]
    Color color = Color.white;

    [SerializeField]
    float speed = 1000;

    [SerializeField]
    Material playerMaterial;

    [SerializeField]
    Material playerEmissiveMaterial;

    [SerializeField]
    GameManager manager;

    GameObject playerShape;

    [SerializeField]
    AudioSource fireAS;

    [SerializeField]
    AudioSource dieAS;

    float deltaX = 1;
    float deltaY = 1;

    private void Start()
    {
        playerShape = new GameObject();
        playerShape.name = "Player";
        playerShape.transform.parent = transform;

        PlayerDestroyOnHit pdh = playerShape.AddComponent<PlayerDestroyOnHit>();
        pdh.Manager = manager;

        int startX = (int)player.textureRect.xMin;
        int startY = (int)player.textureRect.yMin;

        int w = (int)player.textureRect.width;
        int h = (int)player.textureRect.height;

        //Prepare to read pixels colors for frame1
        Color[] pixels = player.texture.GetPixels();

        float currentX = 0;
        float currentY = 0;

        //iterate image 1
        for (int i = startX; i < startX + w; i++) {
            for (int j = startY; j < startY + h; j++) {
                Color c = player.texture.GetPixel(i, j);
                if (c.r != 0 && c.g != 0 && c.b != 0) {
                    //not black
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.GetComponent<MeshRenderer>().material = playerEmissiveMaterial;
                    go.name = $"{i}:{j}:{pixels[(i * j) + i]}%";
                    go.transform.position = new Vector3(currentX, currentY, 0);
                    go.transform.parent = playerShape.transform; //make object child of playerShape
                }
                currentY += deltaY;
            }
            currentY = 0;
            currentX += deltaX;
        }

        MeshMerger mm = playerShape.AddComponent<MeshMerger>();
        mm.Configure(playerMaterial, false);

        //The translation should be done after merging the geometry
        transform.position = startPotision;
        transform.localScale = new Vector3(2, 2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            //left movement
            transform.Translate(Time.deltaTime * speed *  -1 * transform.right );
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            //right movement
            transform.Translate(Time.deltaTime * speed * transform.right);
        }

        //Avoid moving outside right/left delta
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, startPotision.x - horizontalDelta, startPotision.x + horizontalDelta);
        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Space)) {
            fireAS.PlayOneShot(fireAS.clip);

            //fire
            GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bullet.tag = "PlayerBullet";
            bullet.transform.localScale = new Vector3(3, 5, 3);
            bullet.transform.position = transform.position + new Vector3(10, 20, 0);
            BulletManager bm = bullet.AddComponent<BulletManager>();
            Destroy(bullet, 4);
        }
    }

    private void OnDisable()
    {
        dieAS.Play();
    }
}