using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    float deltaX = 1;
    float deltaY = 1;

    public void Configure(Sprite barrierShape, Color color, Material newMat) {
        int startX = (int)barrierShape.textureRect.xMin;
        int startY = (int)barrierShape.textureRect.yMin;

        int w = (int)barrierShape.textureRect.width;
        int h = (int)barrierShape.textureRect.height;

        //Prepare to read pixels colors for frame1
        Color[] pixels = barrierShape.texture.GetPixels();

        float currentX = 0;
        float currentY = 0;

        //iterate image 1
        for (int i = startX; i < startX + w; i++) {
            for (int j = startY; j < startY + h; j++) {
                Color c = barrierShape.texture.GetPixel(i, j);
                if (c.r != 0 && c.g != 0 && c.b != 0) {
                    //not black
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.tag = "Barrier";
                    go.AddComponent<FallOnHit>();
                    go.GetComponent<MeshRenderer>().material = newMat;
                    go.GetComponent<MeshRenderer>().material.color = color;
                    go.name = $"{i}:{j}:{pixels[(i * j) + i]}%";
                    go.transform.parent = transform; //make object child of this go
                    go.transform.position = new Vector3(currentX, currentY, 0);
                }
                currentY += deltaY;
            }
            currentY = 0;
            currentX += deltaX;
        }
    }
}