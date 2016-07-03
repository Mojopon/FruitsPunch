using UnityEngine;
using System.Collections;
using UniRx;
using FruitsPunchInGameScripts;

public class DeleteHighlight : MonoBehaviour
{
    public GameObject highlightObject;

	void Start ()
    {
        var deleteFruitsObservable = FruitsPunchManager.Instance as IDeleteFruitsObservable;

        deleteFruitsObservable.DeleteFruitsObservable
                              .Select(x => x[0].transform.position)
                              .Subscribe(x => SpawnHighlightObject(x))
                              .AddTo(gameObject);
	}

    void SpawnHighlightObject(Vector3 pos)
    {
        Instantiate(highlightObject, pos, Quaternion.identity);
    }
}
