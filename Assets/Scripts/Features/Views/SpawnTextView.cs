using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Features.View
{
    public class SpawnTextView: MonoBehaviour
    {
        public float time;
        public int bubbleNumber;
        public GameObject textGameObject;
        public TextMeshPro textMeshPro;
        
        public void Show()
        {
            var currentPosition = textGameObject.transform.localPosition;
            currentPosition.y += 0.7f;
            textGameObject.transform.DOLocalMove(currentPosition, time);
            textMeshPro.text = SetBubbleNumberText(bubbleNumber);
            DOTween.ToAlpha(() => textMeshPro.color, x => textMeshPro.color = x, 0, time);
            Destroy(gameObject, time);
        }
        
        private string SetBubbleNumberText(int bubbleNumber)
        {
            if (bubbleNumber < 1024)
            {
                return bubbleNumber.ToString();
            }
        
            return bubbleNumber / 1024 + "K";
        }
    }
}