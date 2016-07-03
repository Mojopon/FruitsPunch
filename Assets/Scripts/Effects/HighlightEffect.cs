using UnityEngine;
using System.Collections;
using FruitsPunchInGameScripts;

public class HighlightEffect : MonoBehaviour
{
    public float timeToFadeOut = 1f;
    public float startAlpha = 0.75f;

    private SpriteRenderer sprite;

    void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();

        var fruitsPunchIngameProperties = FruitsPunchManager.Instance as IFruitsPunchInGameProperties;
        if (fruitsPunchIngameProperties == null)
        {
            Debug.LogError("Couldnt find FruitsPunchManager");
        }
        else
        {
            radius = fruitsPunchIngameProperties.FruitsDeleteRadius;
        }

        transform.localScale = new Vector3(radius, radius);
        StartCoroutine(SequenceFadeOut());
	}

    private float radius = 1f;
    private float progress = 0;
    IEnumerator SequenceFadeOut()
    {
        progress = 0;

        while(1 > progress)
        {
            progress += Time.deltaTime / timeToFadeOut;

            var color = sprite.color;
            color.a = startAlpha * (1 - progress);

            sprite.color = color;

            yield return null;
        }

        Destroy(gameObject, 1f);
        yield break;
    }
}
