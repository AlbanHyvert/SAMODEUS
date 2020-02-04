using UnityEngine;

namespace Engine.Timer
{
    public class Timer
    {
        private float _timeStamp = 0;
        private bool _isStopped = false;

        public float TimeLeft { get { return (_timeStamp - Time.time); } }
        public bool IsStopped { get { return _isStopped; } set { _isStopped = value; } }

        public void ReduceRemainingTime(float value)
        {
            _timeStamp -= value;
        }

        public void ResetTimer(float time)
        {
            _timeStamp = Time.time + time;
            _isStopped = false;
        }

        public bool IsTimerEnd()
        {
            if (_isStopped)
            {
                return false;
            }

            return (TimeLeft <= 0);
        }

        public void Stop()
        {
            _timeStamp = Time.time;
            _isStopped = true;
        }
    }
}

