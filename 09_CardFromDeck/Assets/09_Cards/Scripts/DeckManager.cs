using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace EGD
{
    public class DeckManager : MonoBehaviour
    {
        [Header("Genreal")]
        public Card cardPrefab;
        public Transform playerCardCenter;
        [SerializeField] int maxCardsinHand = 7;

        [Header("Organization")]
        [SerializeField] float distanceBetweenCards = 0.3f;
        [SerializeField] float degreeBetweenCards = 5;
        [SerializeField] float distanceBetweenZCards = 0.01f;
        [SerializeField] float distanceBetweenYCards = 0.05f;

        [Header("Hover")]
        [SerializeField] float pushDistanceX = 0.1f;



        private void OnMouseDown()
        {
            DrawCard();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DrawCard();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ClearHand();
                ClearBoard();
            }
        }

        public void DrawCard()
        {
            if (playerCardCenter.transform.childCount >= maxCardsinHand) return;
            Card go = Instantiate(cardPrefab);
            go.InitCard(playerCardCenter.transform.childCount, this);
            go.transform.SetParent(playerCardCenter.transform);
            go.transform.position = transform.position;
            // go.transform.localPosition = Vector3.zero;
            // go.transform.localRotation = Quaternion.identity;
            OrganizeCards();
        }

        public void OrganizeCards()
        {
            //                  *
            //              *       *
            //           *      *      *
            // First one, child count is 1 so magic factor is zero making the card be in the center
            // Second one, magic factor is just half. This help find the offset from zero based on the cards. Each loop, the offset increase by the distance

            int childCount = playerCardCenter.transform.childCount;
            float magicFactor = 0.5f * (float)(childCount - 1f);
            float offsetX = -distanceBetweenCards * magicFactor;
            float degreeOffset = -degreeBetweenCards * magicFactor;
            float offsetZ = -distanceBetweenZCards * magicFactor;
            float offsetY = -distanceBetweenYCards * magicFactor;
            for (int i = 0; i < childCount; i++,
            offsetX += distanceBetweenCards, offsetZ += distanceBetweenZCards,
            offsetY += distanceBetweenYCards,
            degreeOffset += degreeBetweenCards)
            {
                playerCardCenter.transform.GetChild(i).GetComponent<Card>().RefreshIndex(i); //Bad GetComponent but blame the time xD

                var pos = Vector3.zero;
                pos.x = offsetX;
                pos.z = -offsetZ;
                pos.y = -Mathf.Abs(offsetY);
                playerCardCenter.transform.GetChild(i).transform.DOLocalMove(pos, 0.1f);
                // playerCardCenter.transform.GetChild(i).transform.localPosition = pos;

                var rotEuler = Vector3.zero;
                rotEuler.z = -degreeOffset;
                playerCardCenter.transform.GetChild(i).transform.DOLocalRotate(rotEuler, 0.1f);
                // playerCardCenter.transform.GetChild(i).transform.localEulerAngles = rotEuler;
            }
        }

        public void HoverCard(int hoverIndex)
        {
            int childCount = playerCardCenter.transform.childCount;
            float magicFactor = 0.5f * (float)(childCount - 1f);
            float offsetX = -distanceBetweenCards * magicFactor;
            float degreeOffset = -degreeBetweenCards * magicFactor;
            float offsetZ = -distanceBetweenZCards * magicFactor;
            float offsetY = -distanceBetweenYCards * magicFactor;
            for (int i = 0; i < childCount; i++,
            offsetX += distanceBetweenCards, offsetZ += distanceBetweenZCards,
            offsetY += distanceBetweenYCards,
            degreeOffset += degreeBetweenCards)
            {
                var pos = Vector3.zero;

                if (i != hoverIndex)
                {
                    pos.x = offsetX + (1f / ((float)i - (float)hoverIndex) * pushDistanceX);
                    pos.z = -offsetZ;
                    pos.y = -Mathf.Abs(offsetY);
                }
                else
                {
                    pos.x = offsetX;
                    pos.z = -0.4f;
                    pos.y = 0.2f;
                }
                playerCardCenter.transform.GetChild(i).transform.DOLocalMove(pos, 0.1f);
                // playerCardCenter.transform.GetChild(i).transform.localPosition = pos;

                var rotEuler = Vector3.zero;
                if (i != hoverIndex)
                    rotEuler.z = -degreeOffset;
                // playerCardCenter.transform.GetChild(i).transform.localEulerAngles = rotEuler;
                playerCardCenter.transform.GetChild(i).transform.DOLocalRotate(rotEuler, 0.1f);
            }
        }


        [Header("Table Placement")]
        public Transform[] cardsTransform;
        public bool PlaceCardOnTable(int index)
        {

            int emptyIndex = GetFirstEmpty();
            if (emptyIndex == -1) return false;

            var card = playerCardCenter.transform.GetChild(index);
            card.SetParent(cardsTransform[emptyIndex]);
            card.DOLocalMove(Vector3.zero, 0.1f);
            card.DOLocalRotate(Vector3.zero, 0.1f);

            OrganizeCards();
            return true;
        }

        int GetFirstEmpty()
        {
            for (int i = 0; i < cardsTransform.Length; i++)
            {
                if (cardsTransform[i].childCount == 0) return i;
            }
            return -1;
        }



        void ClearHand()
        {
            for (int i = playerCardCenter.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(playerCardCenter.transform.GetChild(i).gameObject);
            }
        }

        void ClearBoard()
        {
            for (int i = 0; i < cardsTransform.Length; i++)
            {
                if (cardsTransform[i].childCount != 0)
                    Destroy(cardsTransform[i].GetChild(0).gameObject);
            }
        }

    }
}
