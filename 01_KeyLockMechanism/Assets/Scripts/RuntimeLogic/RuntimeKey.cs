using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace EGD
{
    public class RuntimeKey : MonoBehaviour
    {
        Key currentKey;
        GameManager gameManager;

        public SpriteRenderer rend;

        public Key key => currentKey;
        public void SetKey(Key k, GameManager gm)
        {
            gameManager = gm;
            currentKey = k;
            k.OnKeyDestroyed += OnKeyDestroyed;

            UpdateKeyColor();
        }

        void UpdateKeyColor()
        {
            //Update Key Color
            rend.color = gameManager.GetColor(key.id);

        }

        void OnKeyDestroyed()
        {
            Destroy(this.gameObject);
        }
    }
}
