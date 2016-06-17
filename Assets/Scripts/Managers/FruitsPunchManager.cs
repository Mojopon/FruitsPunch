using UnityEngine;
using System.Collections;
using UniRx;
using System.Linq;
using System.Collections.Generic;
using System;

public class Fruits : IEnumerable<GameObject>
{
    private List<GameObject> fruits = new List<GameObject>();

    public Fruits(params GameObject[] fruitsObjects)
    {
        fruits = fruitsObjects.ToList();
    }

    public IEnumerator<GameObject> GetEnumerator()
    {
        return fruits.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return fruits.GetEnumerator();
    }

    public int Count
    {
        get { return fruits.Count; }
    }
}

public class FruitsPunchManager : ReactiveSingletonMonoBehaviour<FruitsPunchManager>,
                                  IFruitsPunchInGameProperties,
                                  IWaitTimeProgressObservable,
                                  IFeverPointProgressObservable
{
    public float fruitDeleteRadius = 1f;
    public Vector3 fruitsSpawnPoint = new Vector3(0, 12, 0);
    public LayerMask fruitLayer;

    public GameObject fruitPrefab;
    public Sprite[] fruitSprites;


    public float nextDeleteAvailableDuration = 0.8f;

    public float timeToFinishFever = 8f;
    public float pointEarnForEachDelete = 0.05f;

    public IObservable<Fruits> DeleteFruitsObservable { get { return _deleteFruitsStream.AsObservable(); } }
    private Subject<Fruits> _deleteFruitsStream = new Subject<Fruits>();

    public IObservable<float> WaitTimeProgressObservable { get { return _waitTimeProgressStream.AsObservable(); } }
    private BehaviorSubject<float> _waitTimeProgressStream = new BehaviorSubject<float>(0);

    public IObservable<float> FeverPointProgressObservable { get { return _feverPointProgressStream.AsObservable(); } }
    private BehaviorSubject<float> _feverPointProgressStream = new BehaviorSubject<float>(0);

    void Start ()
    {
        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.BeforeGameStart)
                          .Subscribe(x => OnGameStart())
                          .AddTo(gameObject);

        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.InGame)
                          .Subscribe(x => StartReceivingMouseInput())
                          .AddTo(gameObject);
    }

    // OnClick MouseEvent Stream
    private IObservable<Vector3> _onMouseClickObservable { get; set; }

    // Wait Time Relative Properties
    private float _waitTimeForNextDelete = 0;
    private float waitTimeForNextDelete
    {
        get { return _waitTimeForNextDelete; }
        set
        {
            _waitTimeForNextDelete = value;
            _waitTimeProgressStream.OnNext(_waitTimeForNextDelete / nextDeleteAvailableDuration);
        }
    }
    private bool CanDelete() { { return _waitTimeForNextDelete <= 0; } }

    // Fever Point Relative Properties
    private float _feverProgress = 0;
    private float feverProgress
    {
        get { return _feverProgress; }
        set
        {
            _feverProgress = value;
            _feverPointProgressStream.OnNext(_feverProgress);
        }
    }
    private bool _isOnFever = false;

    void OnGameStart()
    {
        var inputtable = (IInputtable)InputManager.Instance;

        // create observable stream to notify every mouse event on click
        _onMouseClickObservable = inputtable.MouseEventObservable
                                           .Where(x => x.state == MouseClickState.OnClick)
                                           .Select(x => x.position);

        // update waittime for next delete
        Observable.EveryUpdate()
                  .Where(x => waitTimeForNextDelete > 0)
                  .Select(x => Time.deltaTime)
                  .Subscribe(x => waitTimeForNextDelete -= x)
                  .AddTo(gameObject);

        Observable.EveryUpdate()
                  .Where(x => _isOnFever)
                  .Select(x => Time.deltaTime / timeToFinishFever)
                  .Subscribe(x => 
                  {
                      feverProgress -= x;
                      if(feverProgress <= 0)
                      {
                          feverProgress = 0f;
                          _isOnFever = false;
                      }
                  })
                  .AddTo(gameObject);

        DeleteFruitsObservable.Where(x => !_isOnFever && x.Count >= 2)
                              .Subscribe(x =>
                              {
                                  feverProgress += pointEarnForEachDelete * x.Count;
                                  if(feverProgress > 1f)
                                  {
                                      feverProgress = 1f;
                                      _isOnFever = true;
                                  }
                              })
                              .AddTo(gameObject);

        DropBalls();

    }

    private IDisposable inputSubscription;
    void StartReceivingMouseInput()
    {
        inputSubscription = this._onMouseClickObservable
                                .Select(x => GetFruitAtThePosition(x))
                                .Where(x => x)
                                .Select(x => GetAllFruitsAroundTheFruit(x, fruitDeleteRadius))
                                .Subscribe(x => DeleteFruits(x))
                                .AddTo(gameObject);
    }

    void DeleteFruits(GameObject[] fruits)
    {
        if (!CanDelete()) return;

        foreach (var hit in fruits)
        {
            Destroy(hit);
        }

        _deleteFruitsStream.OnNext(new Fruits(fruits));

        SetWaitTimeForNextDelete();
    }

    void SetWaitTimeForNextDelete()
    {
        waitTimeForNextDelete = nextDeleteAvailableDuration;
    }

    void DropBalls()
    {
        StartCoroutine(SequenceDropBalls(80));
    }

    GameObject GetFruitAtThePosition(Vector3 position)
    {

        var hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Epsilon, fruitLayer);
        if (!hit) return null;

        return hit.collider.gameObject;

    }

    GameObject[] GetAllFruitsAroundTheFruit(GameObject targetFruit, float radius)
    {
        if (_isOnFever) radius *= 2;

        return GetAllFruitsAroundTheFruit(targetFruit, radius, true);
    }

    GameObject[] GetAllFruitsAroundTheFruit(GameObject targetFruit, float radius, bool filterWithTargetColor)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetFruit.transform.position, radius, fruitLayer);

        if(filterWithTargetColor)
        {
            hits = hits.Where(x => FruitsAreSameColor(x.gameObject, targetFruit))
                       .Select(x => x)
                       .ToArray();
        }

        return hits.Select(x => x.gameObject).ToArray();
    }

    

    bool FruitsAreSameColor(GameObject fruitsOne, GameObject fruitsTwo)
    {
        return fruitsOne.name == fruitsTwo.name;
    }

    private Transform fruitsHolder;
    IEnumerator SequenceDropBalls(int count)
    {
        fruitsHolder = new GameObject("FruitsHolder").transform;

        while (true)
        {
            if(count > this.NumberOfFruits)
            {
                for (int i = this.NumberOfFruits; i < count; i++)
                {
                    yield return StartCoroutine(SequenceDropTheBall());
                }
            }
            else
            {
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

    IEnumerator SequenceDropTheBall()
    {
        var fruit = Instantiate(fruitPrefab) as GameObject;
        var pos = fruit.transform.position;
        pos.x = UnityEngine.Random.Range(fruitsSpawnPoint.x + -2.0f,fruitsSpawnPoint.x +  2.0f);
        pos.y = fruitsSpawnPoint.y;
        fruit.transform.position = pos;

        var fruitTexture = fruit.GetComponent<SpriteRenderer>();
        var spriteID = UnityEngine.Random.Range(0, 5);
        fruitTexture.sprite = fruitSprites[spriteID];
        fruit.name = "Fruit" + spriteID;

        fruit.transform.SetParent(fruitsHolder);

        yield return new WaitForSeconds(0.05f);

        yield break;
    }

    int NumberOfFruits { get { return fruitsHolder.childCount; } }

    void OnDestroy()
    {
        if(fruitsHolder != null) 
            Destroy(fruitsHolder.gameObject);
        fruitsHolder = null;
    }
}
