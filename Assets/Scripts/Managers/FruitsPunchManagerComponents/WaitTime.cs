using UnityEngine;
using System.Collections;
using UniRx;

namespace FruitsPunchInGameScripts
{

    public class WaitTime : MonoBehaviour, IWaitTimeProgressObservable
    {
        public float gapBetweenDelete;

        public IObservable<float> WaitTimeProgressObservable { get { return _waitTimeProgressStream.AsObservable(); } }
        private BehaviorSubject<float> _waitTimeProgressStream = new BehaviorSubject<float>(0);

        private float _waitTimeForNextDelete = 0;
        private float waitTimeForNextDelete
        {
            get { return _waitTimeForNextDelete; }
            set
            {
                _waitTimeForNextDelete = value;
                _waitTimeProgressStream.OnNext(_waitTimeForNextDelete / gapBetweenDelete);
            }
        }

        private bool CanDelete() { { return _waitTimeForNextDelete <= 0; } }

        public void Initialize(float gapBetweenDelete)
        {
            this.gapBetweenDelete = gapBetweenDelete;
        }

        void Start()
        {
            Observable.EveryUpdate()
                      .Where(x => waitTimeForNextDelete > 0)
                      .Select(x => Time.deltaTime)
                      .Subscribe(x => waitTimeForNextDelete -= x)
                      .AddTo(gameObject);
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
            waitTimeForNextDelete = gapBetweenDelete;
        }
    }
}
