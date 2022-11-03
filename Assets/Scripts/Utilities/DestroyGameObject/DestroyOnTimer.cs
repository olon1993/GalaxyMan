using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DestroyOnTimer : MonoBehaviour
    {
        [SerializeField] private float _timer = 5.2f;
        private float _startTime;

        private void Start()
        {
            Destroy(gameObject, _timer);
        }
    }
}
