using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Views
{
    public delegate void OnClick(int level);  
    public class LevelChooseButton : MonoBehaviour
    {
        public Button Button;
        public TextMeshProUGUI Text;
        public int Level;
        public event OnClick OnClick;

        private void OnEnable()
        {
            //Button.onClick.AddListener(OnClickPressed);
        }

        public void OnClickPressed()
        {
            Debug.Log("Button click");
            OnClick?.Invoke(Level);
        }

        public void SetLevelIndex(int level)
        {
            Text.text = (level+1).ToString();
            this.Level = level;
        }
    }
}