using UnityEngine;
using System.Collections;
using UniRx;
using System.Linq;
using System.Collections.Generic;
using System;

namespace FruitsPunchInGameScripts
{
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
                                      IWaitTimeProgressObservable,
                                      IFruitsPunchInGameProperties,
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

        // fever point relative properties
        public IObservable<float> FeverPointProgressObservable { get { return _feverPointManager.FeverPointProgressObservable; } }
        public bool IsOnFever { get { return _feverPointManager.IsOnFever; } }
        public float CalculateRadius(float originalRadius) { return _feverPointManager.CalculateRadius(originalRadius); }

        private FeverPoint _feverPointManagerSource;
        private FeverPoint _feverPointManager
        {
            get
            {
                InitializeComponents();
                return _feverPointManagerSource;
            }
        }

        // wait time relative properties
        public IObservable<float> WaitTimeProgressObservable { get { return _waitTimeManager.WaitTimeProgressObservable; } }

        private WaitTime _waitTimeManagerSource;
        private WaitTime _waitTimeManager
        {
            get
            {
                InitializeComponents();
                return _waitTimeManagerSource;
            }
        }

        // add components to its gameobject and initialize
        void InitializeComponents()
        {
            if (ComponentsAreReady()) return;

            _waitTimeManagerSource = gameObject.AddComponent<WaitTime>();
            _waitTimeManagerSource.Initialize(nextDeleteAvailableDuration);

            _feverPointManagerSource = gameObject.AddComponent<FeverPoint>();
            _feverPointManagerSource.Initialize(timeToFinishFever, pointEarnForEachDelete);
        }

        // check if components are ready
        bool ComponentsAreReady()
        {
            if (!_waitTimeManagerSource || !_feverPointManagerSource) return false;

            return true;
        }

        void Start()
        {
            GameState.Instance.GameStateReactiveProperty
                              .Where(x => x == GameStateEnum.BeforeGameStart)
                              .Subscribe(x => StartGame())
                              .AddTo(gameObject);

            GameState.Instance.GameStateReactiveProperty
                              .Where(x => x == GameStateEnum.InGame)
                              .Subscribe(x => StartReceivingMouseInput())
                              .AddTo(gameObject);
        }

        // OnClick MouseEvent Stream
        private IObservable<Vector3> _onMouseClickObservable { get; set; }


        void StartGame()
        {
            var inputtable = (IInputtable)InputManager.Instance;

            // create observable stream to notify every mouse event on click
            _onMouseClickObservable = inputtable.MouseEventObservable
                                                .Where(x => x.state == MouseClickState.OnClick)
                                                .Select(x => x.position);

            DropBalls();

        }

        private IDisposable inputSubscription;
        void StartReceivingMouseInput()
        {
            inputSubscription = this._onMouseClickObservable
                                    .Select(x => GetFruitAtThePosition(x))
                                    .Where(x => x)
                                    .Select(x => GetAllFruitsAroundTheFruit(x, CalculateRadius(fruitDeleteRadius)))
                                    .Subscribe(x => DeleteFruits(x))
                                    .AddTo(gameObject);
        }

        void DeleteFruits(GameObject[] fruits)
        {
            if (!_waitTimeManager.TryDelete()) return;

            foreach (var hit in fruits)
            {
                Destroy(hit);
            }

            _deleteFruitsStream.OnNext(new Fruits(fruits));
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
            return GetAllFruitsAroundTheFruit(targetFruit, radius, true);
        }

        GameObject[] GetAllFruitsAroundTheFruit(GameObject targetFruit, float radius, bool filterWithTargetColor)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(targetFruit.transform.position, radius, fruitLayer);

            if (filterWithTargetColor)
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
                if (count > this.NumberOfFruits)
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
            pos.x = UnityEngine.Random.Range(fruitsSpawnPoint.x + -2.0f, fruitsSpawnPoint.x + 2.0f);
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
            if (fruitsHolder != null)
                Destroy(fruitsHolder.gameObject);
            fruitsHolder = null;
        }
    }
}