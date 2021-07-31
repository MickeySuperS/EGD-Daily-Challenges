using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public class GameManager : MonoBehaviour
    {
        GrabKeyController grabKeyController;
        [SerializeField] LayerMask keyMask;

        [SerializeField] RuntimeLock lockObject;
        [SerializeField] RuntimeKey keyObject;

        GameLogic logic;
        [SerializeField] [Range(1, 9)] int numOfLocks = 1;

        public AudioClip[] useKeyClip;
        public AudioClip lockIsUnlockedClip;
        AudioSource src;

        bool gameEnded = false;

        Transform keyParent, lockParent;
        int lockMinRange = -6, lockMaxRange = 4;
        float increment; //the increment value of lockPosition based on lock count (to cover the -6 to 4 evenly)
        float currentIncrement = 0;

        private void Start()
        {
            keyParent = new GameObject("KeyParent").transform;
            lockParent = new GameObject("LockParent").transform;
            src = GetComponent<AudioSource>();
            grabKeyController = new GrabKeyController();
            grabKeyController.Init(Camera.main, keyMask);

            Restart();
        }

        void Restart()
        {
            increment = (float)(lockMaxRange - lockMinRange) / (float)Mathf.Max(1, (numOfLocks - 1));
            currentIncrement = 0;
            gameEnded = false;

            for (int i = 0; i < keyParent.childCount; i++)
            {
                Destroy(keyParent.GetChild(i).gameObject);
            }
            for (int i = 0; i < lockParent.childCount; i++)
            {
                Destroy(lockParent.GetChild(i).gameObject);
            }

            logic = new GameLogic();
            logic.OnLockCreated += OnLockCreated;
            logic.OnKeyCreated += OnKeyCreated;
            logic.OnGameEnded += OnGameEnded;
            logic.Init(numOfLocks, 5, 20);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                Restart();
            grabKeyController.GrabKeyWithMouse();
            if (gameEnded) return;
            logic.UpdateLogic(Time.deltaTime);
        }
        private void FixedUpdate()
        {
            grabKeyController.MoveGrabbedObjectIfExist();
        }

        void OnLockCreated(Lock l)
        {
            var lockRuntime = Instantiate(lockObject, lockParent);
            lockRuntime.SetLock(l, this);

            lockRuntime.gameObject.SetActive(true);
            lockRuntime.transform.position += Vector3.right * currentIncrement * increment;
            currentIncrement++;
        }

        void OnKeyCreated(Key k)
        {
            var keyRuntime = Instantiate(keyObject, keyParent);
            keyRuntime.SetKey(k, this);

            keyRuntime.gameObject.SetActive(true);
            keyRuntime.transform.position += Vector3.right * Random.Range(0f, (lockMaxRange - lockMinRange));
        }

        void OnGameEnded()
        {
            gameEnded = true;
        }

        public void UseKey(Key k, Lock l)
        {
            if (logic.UseKey(k, l))
            {
                src.PlayOneShot(useKeyClip[Random.Range(0, useKeyClip.Length)]);
                if (logic.IsLocked(l))
                {
                    src.PlayOneShot(lockIsUnlockedClip);
                }
            }
        }

        public Color GetColor(int id)
        {
            float h = (float)(id) / (float)(numOfLocks);
            return Random.ColorHSV(h, h, 0.5f, 0.5f, 1, 1);
        }
    }
}
