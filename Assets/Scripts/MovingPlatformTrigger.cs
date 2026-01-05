using UnityEngine;

using System.Collections;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MovingPlatformTrigger : MonoBehaviour
{
    public MovingPlatform movingPlatform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(movingPlatform.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    //if player interract/ press k in an area, it moves platform. can be apply like opening door or trap
    public void Interact()
    {
        if (movingPlatform != null)
        {
            movingPlatform.enabled = !movingPlatform.enabled;
        }
    }
}