using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EGD
{
    public class GameLogic
    {
        string seed = "LKJAWD@3D";

        List<Key> keys = new List<Key>();
        List<Lock> locks = new List<Lock>();
        int maxKeyCount = 25;

        public Action<Lock> OnLockCreated;
        public Action<Key> OnKeyCreated;

        public Action OnGameEnded;

        public void Init(int numOfLocks, int minKeyRequired, int maxKeyRequired)
        {
            UnityEngine.Random.InitState(seed.GetHashCode());

            for (int i = 0; i < numOfLocks; i++)
            {
                CreateLock(numOfLocks, minKeyRequired, maxKeyRequired);
            }
            CreateKey();
            CreateKey();

            time = 0;
        }

        float time = 0;
        float spawnRate = 0.4f;
        float lastTimeSpawned = 0;
        public void UpdateLogic(float deltaTime)
        {
            if (locks.Count <= 0) { OnGameEnded?.Invoke(); return; }
            time += deltaTime;

            if (time - lastTimeSpawned < spawnRate) return;
            if (keys.Count >= maxKeyCount) return;
            lastTimeSpawned = time;
            CreateKey();

        }

        public void CreateLock(int randomMax, int minKeyRequired, int maxKeyRequired)
        {
            Lock newLock = new Lock(UnityEngine.Random.Range(0, randomMax),
                                    UnityEngine.Random.Range(minKeyRequired, maxKeyRequired + 1));
            locks.Add(newLock);
            OnLockCreated?.Invoke(newLock);
        }

        public void CreateKey()
        {
            HashSet<int> ids = new HashSet<int>();
            foreach (var l in locks)
            {
                if (ids.Contains(l.id)) continue;
                ids.Add(l.id);
            }

            Key newKey = new Key(ids.ElementAt(UnityEngine.Random.Range(0, ids.Count)));
            keys.Add(newKey);
            OnKeyCreated?.Invoke(newKey);
        }

        public bool UseKey(Key k, Lock l)
        {
            if (!l.UseKey(k)) return false;

            k.OnKeyDestroyed?.Invoke();
            keys.Remove(k);
            return true;
        }

        public bool IsLocked(Lock l)
        {
            if (l.isLocked) return false;

            l.OnLockDestroyed?.Invoke();
            locks.Remove(l);
            return true;
        }


    }
}
