using UnityEngine;
using System.Collections;
using UniRx;

namespace FruitsPunchInGameScripts
{

    public class FruitsPunchWaitTime : MonoBehaviour, IWaitTimeProgressObservable
    {
        public IObservable<float> WaitTimeProgressObservable { get { return _waitTimeProgressStream.AsObservable(); } }
        private BehaviorSubject<float> _waitTimeProgressStream = new BehaviorSubject<float>(0);

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
