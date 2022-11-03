
using System;
using UnityEngine;

namespace TheFrozenBanana
{
    public class EventBroker
    {
        public static event Action LevelExitReached;
        public static event Action ShipPartFound;
        public static event Action AllShipPartsFound;
        public static event Action LevelCompleted;
        public static event Action PlayerDeath;
        public static event Action EnemyKilled;

        public static void CallLevelExitReached()
        {
            LevelExitReached?.Invoke();
        }

        public static void CallShipPartFound()
        {
            ShipPartFound?.Invoke();
        }

        public static void CallAllShipPartsFound()
        {
            AllShipPartsFound?.Invoke();
        }

        public static void CallLevelCompleted()
        {
            LevelCompleted?.Invoke();
        }

        public static void CallPlayerDeath()
        {
            PlayerDeath?.Invoke();

        }

        public static void CallEnemyKilled()
        {
            EnemyKilled?.Invoke();
        }
    }
}
