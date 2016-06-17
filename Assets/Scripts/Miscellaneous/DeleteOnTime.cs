using UnityEngine;
using System.Collections;

public class DeleteOnTime : MonoBehaviour
{
    public float timeToDelete = 5f;

    void Start()
    {
        Destroy(gameObject, timeToDelete);
    }
}
