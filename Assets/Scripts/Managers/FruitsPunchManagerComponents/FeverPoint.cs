using UnityEngine;
using System.Collections;
using UniRx;

namespace FruitsPunchInGameScripts
{
    public class FeverPoint : MonoBehaviour, IFeverPointProgressObservable
    {
        private float timeToFinishFever;
        private float pointEarnForEachDelete;

        public IObservable<float> FeverPointProgressObservable { get { return _feverPointProgressStream.AsObservable(); } }
        private BehaviorSubject<float> _feverPointProgressStream = new BehaviorSubject<float>(0);

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
        public bool IsOnFever { get; private set; }

        public float CalculateRadius(float originalRadius)
        {
            if (IsOnFever) return originalRadius * 2;
            else return originalRadius;
        }

        public void Initialize(float timeToFinishFever, float pointEarnForEachDelete)
        {
            this.timeToFinishFever = timeToFinishFever;
            this.pointEarnForEachDelete = pointEarnForEachDelete;
        }

        void Start()
        {
            Observable.EveryUpdate()
                      .Where(x => IsOnFever)
                      .Select(x => Time.deltaTime / timeToFinishFever)
                      .Subscribe(x =>
                      {
                          feverProgress -= x;
                          if (feverProgress <= 0)
                          {
                              feverProgress = 0f;
                              IsOnFever = false;
                          }
                      })
                      .AddTo(gameObject);

            FruitsPunchManager.Instance.DeleteFruitsObservable
                                       .Where(x => !IsOnFever && x.Count >= 2)
                                       .Subscribe(x =>
                                       {
                                           feverProgress += pointEarnForEachDelete * x.Count;
                                           if (feverProgress > 1f)
                                           {
                                               feverProgress = 1f;
                                              IsOnFever = true;
                                           }
                                       })
                                       .AddTo(gameObject);
        }
    }
}
