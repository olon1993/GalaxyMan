using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DropLoot : MonoBehaviour, IDropLoot
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\
        [SerializeField] private bool _showDebugLog;
        [SerializeField] private float _dropChance;
        [SerializeField] private GameObject[] _lootList;
        [SerializeField] private float[] _lootWeightedChance;
        private float[] cumulativeWeight;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void Awake() {
            cumulativeWeight = _lootWeightedChance;
            for (int i = 0; i < _lootWeightedChance.Length; i++) {
                float cumulative = 0; 
                for (int j = 0; j <= i; j++) {
                    cumulative += _lootWeightedChance[j];
				}
                cumulativeWeight[i] = cumulative;
			}
            if (_showDebugLog) {
                for (int i = 0; i < cumulativeWeight.Length; i++) {
                    Debug.Log("Cumulative Weight of Loot: i=[" + i + "]: " + cumulativeWeight[i]) ;
				}
			}
		}

		public virtual void DropRandomLoot() {

            if (WillLootDrop()) {
                SpawnLoot(WhatLootWillDrop());
            }
		}

        protected virtual bool WillLootDrop() {
            float ran = Random.Range(0f,1f);
            if (_dropChance > 1) {
                ran *= 100;
			}
            if (ran < _dropChance) {
                return true;
			} else {
                return false;
			}
		}

        protected virtual GameObject WhatLootWillDrop() {

            float totalWeight = 1;
            foreach (float chance in _lootWeightedChance) {
                totalWeight += chance;
            }
            float ran = Random.Range(0, totalWeight);
            GameObject spawning = _lootList[0];
            for (int i = 0; i < _lootWeightedChance.Length; i++) {
                if (ran > _lootWeightedChance[i]) {
                    continue;
				} else {
                    spawning = _lootList[i];
                    break;
				}
			}
            return spawning;
        }

        protected virtual void SpawnLoot(GameObject loot) {
            Instantiate(loot, gameObject.transform.position, Quaternion.identity, null);
		}

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public float DropChance {
            get { return _dropChance; }
        }

        public GameObject[] LootList {
            get { return _lootList; }
		}

        public float[] LootWeightedChance {
            get { return _lootWeightedChance; }
        }
    }
}
