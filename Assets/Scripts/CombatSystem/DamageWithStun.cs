using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DamageWithStun : Damage
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] private float _stunTime;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        #region PublicMethods

        #endregion


        #region UnityMethods

        void Awake()
        {
        
        }

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        #endregion


        #region PrivateMethods

        #endregion


        //**************************************************\\
        //****************** Properties ********************\\
        //**************************************************\\


        // NEW
        public float StunTime
        {
            get { return _stunTime; }
            set { _stunTime = value; }
        }

    }
}
