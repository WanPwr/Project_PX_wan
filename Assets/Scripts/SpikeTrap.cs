using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ActivateTrap()
    {
        Debug.Log("Trap Sprung!");
        // If you have an animation, play it here
        if (anim != null) anim.SetTrigger("Activate");

        // Or simply move the object/enable damage logic
    }
}   