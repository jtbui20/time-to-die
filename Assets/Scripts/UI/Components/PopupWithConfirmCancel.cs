using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class PopupWithConfirmCancel : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public Button ConfirmButton;
        public Button CancelButton;

        public void Setup(
            string Text, Action OnConfirm)
        {
            this.Text.text = Text;
            ConfirmButton.onClick.AddListener(() =>
            {
                OnConfirm();
                Destroy(gameObject);
            });
            CancelButton.onClick.AddListener(OnCancel);
        }
        
        void OnCancel()
        {
            Text.text = "";
            ConfirmButton.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}