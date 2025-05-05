using UnityEngine;

public class MisteryShipVisibleAlerter : MonoBehaviour
{
    private void OnBecameVisible()
    {
        transform.parent.gameObject.GetComponent<MisteryShipManager>().Visible();
    }

    private void OnBecameInvisible()
    {
        transform.parent.gameObject.GetComponent<MisteryShipManager>().Invisible();
    }
}
