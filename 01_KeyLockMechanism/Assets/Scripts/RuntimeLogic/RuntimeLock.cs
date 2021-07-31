using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EGD
{
    public class RuntimeLock : MonoBehaviour
    {

        GameManager gameManager;
        Lock curentLock;
        public SpriteRenderer rend;
        public void SetLock(Lock l, GameManager gm)
        {
            curentLock = l;

            l.OnLockDestroyed += OnLockDestroyed;
            l.OnKeyUsed += OnKeyUsed;
            gameManager = gm;

            SetRemainingText();
            UpdateKeyColor();
        }
        void UpdateKeyColor()
        {
            //Update Key Color
            rend.color = gameManager.GetColor(curentLock.id);

        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            RuntimeKey k = other.collider.GetComponent<RuntimeKey>();
            if (k == null) return;

            gameManager.UseKey(k.key, curentLock);
        }

        public TextMeshPro remainingText;
        void OnKeyUsed()
        {
            SetRemainingText();
        }
        void SetRemainingText()
        {
            var remText = curentLock.numberOfKeysRequired.ToString("00");
            string outputText = "";
            foreach (var c in remText)
            {
                outputText += $"<sprite={c}>";
            }
            remainingText.text = outputText;
        }

        void OnLockDestroyed()
        {
            Destroy(gameObject);
        }


    }
}