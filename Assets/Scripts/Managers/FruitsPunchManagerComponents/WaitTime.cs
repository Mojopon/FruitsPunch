using UnityEngine;
using System.Collections;
using UniRx;

namespace FruitsPunchInGameScripts
{

    public class WaitTime : MonoBehaviour, IWaitTimeProgressObservable
    {
        public float _gapTimeBetweenDelete;

        public IObservable<float> WaitTimeProgressObservable { get { return _waitTimeProgressStream.AsObservable(); } }
        private BehaviorSubject<float> _waitTimeProgressStream = new BehaviorSubject<float>(0);

        private float _waitTimeForNextDelete = 0;
        private float waitTimeForNextDelete
        {
            get { return _waitTimeForNextDelete; }
            set
            {
                _waitTimeForNextDelete = value;
                _waitTimeProgressStream.OnNext(_waitTimeForNextDelete / _gapTimeBetweenDelete);
            }
        }

        private bool CanDelete() { { return _waitTimeForNextDelete <= 0; } }

        void Start()
        {
            IFruitsPunchInGameProperties ingameProperties = FruitsPunchManager.Instance;
            Initialize(ingameProperties);

            Observable.EveryUpdate()
                      .Where(x => waitTimeForNextDelete > 0)
                      .Select(x => Time.deltaTime)
                      .Subscribe(x => waitTimeForNextDelete -= x)
                      .AddTo(gameObject);
        }

        void Initialize(IFruitsPunchInGameProperties properties)
        {
            this._gapTimeBetweenDelete = properties.GapTimeBetweenDelete;
        }

        public bool TryDelete()
        {
            if (CanDelete())
            {
                SetWaitTimeForNextDelete();
                return true;
            }
            else return false;
        }

        void SetWaitTimeForNextDelete()
        {
            waitTimeForNextDelete = _gapTimeBetweenDelete;
        }
    }
}
