using UnityEngine;
using System.Collections;
using UniRx;

public class DeletedFruitsParticle : MonoBehaviour
{
    public GameObject deleteParticle;

    void Start()
    {
        FruitsPunchManager.Instance
                     .DeleteFruitsObservable
                     .Subscribe(x => GenerateParticles(x))
                     .AddTo(gameObject);
    }

    void GenerateParticles(Fruits fruits)
    {
        foreach(var fruit in fruits)
        {
            Instantiate(deleteParticle, fruit.transform.position, Quaternion.identity);
        }
    }
}
