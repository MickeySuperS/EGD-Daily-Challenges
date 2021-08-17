using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public class Card : MonoBehaviour
    {

        int index;
        DeckManager deckManager;

        public void InitCard(int index, DeckManager deckManager)
        {
            this.index = index;
            this.deckManager = deckManager;
        }

        public void RefreshIndex(int index)
        {
            this.index = index;
        }

        private void OnMouseEnter()
        {
            if (index == -1) return;
            deckManager.HoverCard(index);
        }

        private void OnMouseExit()
        {
            if (index == -1) return;
            deckManager.OrganizeCards();
        }

        private void OnMouseDown()
        {
            if (index == -1)
            {
                Destroy(this.gameObject);
                return;
            }
            if (deckManager.PlaceCardOnTable(index))
                index = -1;
        }

    }
}
