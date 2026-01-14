using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isOpened = false;

    public void OpenDoor()
    {
        if (isOpened) return;
        isOpened = true;

        // Simple Way: Disable the collider and fade the sprite
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1, 1, 1, 0.3f); // Fades it slightly

        Debug.Log("Quest complete: Door is now open!");
    }
}