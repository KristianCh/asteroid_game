using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Scenes.Store
{
    public class InfoTab : MonoBehaviour
    {
        public static InfoTab Instance;

        [SerializeField]
        public TMP_Text Title;
        [SerializeField]
        public TMP_Text Description;

        private Vector3 _TargetPosition = Vector3.zero;
        private Vector2 _TargetPivot = Vector2.zero;
        private RectTransform _RectTransform;
        public float _Visibility = 0;
        private float _TitleAlpha;
        private float _DescriptionAlpha;
        private float _ThisAlpha;
        private Image _Image;
        public void Start()
        {
            Instance = this;
            _TargetPosition = transform.position;
            _TargetPivot = new Vector2(1, 0);
            _RectTransform = GetComponent<RectTransform>();
            SetEnabled(false);

            _TitleAlpha = Title.alpha;
            _DescriptionAlpha = Description.alpha;
            _Image = GetComponent<Image>();
            _ThisAlpha = _Image.color.a;
            
            Title.alpha = 0;
            Description.alpha = 0;

            var color = _Image.color;
            color.a = 0;
            _Image.color = color;
        }

        public void Update()
        {
            _TargetPosition = Input.mousePosition;

            _TargetPosition.x = (_TargetPosition.x / (Screen.width / 2) - 1) * Camera.main.orthographicSize * Camera.main.aspect;
            _TargetPosition.y = (_TargetPosition.y / (Screen.height / 2) - 1) * Camera.main.orthographicSize;

            transform.position = Vector3.Lerp(transform.position, _TargetPosition, Time.deltaTime * 10f);

            _TargetPivot.x = _TargetPosition.x < 0 ? 0 : 1;
            _TargetPivot.y = _TargetPosition.y < 0 ? 0 : 1;
            
            _RectTransform.pivot = Vector2.Lerp(_RectTransform.pivot, _TargetPivot, Time.deltaTime * 10f);

            Title.alpha = Mathf.Lerp(Title.alpha, _TitleAlpha * _Visibility, Time.deltaTime * 10f);
            Description.alpha = Mathf.Lerp(Description.alpha, _DescriptionAlpha * _Visibility, Time.deltaTime * 10f);

            var color = _Image.color;
            color.a = Mathf.Lerp(color.a, _ThisAlpha * _Visibility, Time.deltaTime * 10f);
            _Image.color = color;
        }

        public void SetTitle(string title)
        {
            Title.text = title;
        }

        public void SetDescription(string description)
        {
            Description.text = description;
        }

        public void SetEnabled(bool enable)
        {
            _Visibility = enable ? 1 : 0;
        }
    }
}
