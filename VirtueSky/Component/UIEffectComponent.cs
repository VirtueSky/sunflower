using DG.Tweening;
using UnityEngine;
using VirtueSky.Attributes;

namespace VirtueSky.Component
{
    public class UIEffectComponent : MonoBehaviour
    {
        [Header("Data config")] public AnimType animType;

        [SerializeField] private bool playOnAwake = true;
        [SerializeField] private float animTime = .5f;
        [SerializeField] private float delayAnimTime;

        [SerializeField] private Vector3 fromScale = Vector3.one;
        [SerializeField] private Vector3 saveLocalScale = Vector3.one;

        [Header("Shake Effect")] [SerializeField]
        private float strength = 3f;

        [ShowIf(nameof(animType), AnimType.Move)] [Header("Move Effect")] [SerializeField]
        private MoveType _moveType;

        [ShowIf(nameof(IsShowAttributeFromPosition), true)] [SerializeField]
        private Vector3 fromPosition;

        [ShowIf(nameof(IsShowAttributesMoveDirection), true)] [SerializeField]
        private DirectionType directionType;

        [ShowIf(nameof(IsShowAttributesMoveDirection), true)] [SerializeField]
        private float offset;

        [ShowIf(nameof(animType), AnimType.Move)] [ReadOnly]
        private Vector3 _saveAnchorPosition;

        private RectTransform _rectTransform;
        private Sequence _sequence;

        public bool IsShowAttributeFromPosition => animType == AnimType.Move && _moveType == MoveType.Vector3;
        public bool IsShowAttributesMoveDirection => animType == AnimType.Move && _moveType == MoveType.Direction;

        public void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _saveAnchorPosition = _rectTransform.anchoredPosition;
            saveLocalScale = _rectTransform.localScale;
        }

        public void OnEnable()
        {
            if (playOnAwake)
            {
                PlayAnim();
            }
        }

        public void PlayAnim()
        {
            switch (animType)
            {
                case AnimType.OutBack:
                    transform.localScale = fromScale;
                    _sequence = DOTween.Sequence().OnStart(() => transform.localScale = fromScale).SetDelay(delayAnimTime)
                        .Append(transform.DOScale(Vector3.one, animTime).OnKill(() => transform.localScale = saveLocalScale)
                            .SetEase(Ease.OutBack));
                    break;
                case AnimType.Shake:
                    _sequence = DOTween.Sequence().SetDelay(delayAnimTime)
                        .Append(transform.DOShakeRotation(animTime, strength).SetEase(Ease.Linear));
                    break;
                case AnimType.Move:
                    _rectTransform.anchoredPosition = _saveAnchorPosition;
                    switch (_moveType)
                    {
                        case MoveType.Vector3:
                            transform.DOLocalMove(_saveAnchorPosition, animTime).SetDelay(delayAnimTime)
                                .SetEase(Ease.Linear);
                            break;
                        case MoveType.Direction:
                            switch (directionType)
                            {
                                case DirectionType.Up:
                                    _sequence = DOTween.Sequence().SetDelay(delayAnimTime).Append(transform
                                        .DOLocalMoveY(transform.localPosition.y + offset, animTime).SetEase(Ease.InBack));
                                    break;
                                case DirectionType.Down:
                                    _sequence = DOTween.Sequence().SetDelay(delayAnimTime).Append(transform
                                        .DOLocalMoveY(transform.localPosition.y - offset, animTime).SetEase(Ease.InBack));
                                    break;
                                case DirectionType.Left:
                                    _sequence = DOTween.Sequence().SetDelay(delayAnimTime).Append(transform
                                        .DOLocalMoveX(transform.localPosition.x - offset, animTime).SetEase(Ease.InBack));
                                    break;
                                case DirectionType.Right:
                                    _sequence = DOTween.Sequence().SetDelay(delayAnimTime).Append(transform
                                        .DOLocalMoveX(transform.localPosition.x + offset, animTime).SetEase(Ease.InBack));
                                    break;
                            }

                            break;
                    }

                    break;
            }
        }

        public void OnDisable()
        {
            Reset();
            _sequence?.Kill();
        }


        public void Reset()
        {
            if (!Application.isPlaying) return;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = _saveAnchorPosition;
            _rectTransform.localScale = saveLocalScale;
        }
    }

    public enum AnimType
    {
        OutBack,
        Shake,
        Move,
    }

    public enum MoveType
    {
        Vector3,
        Direction,
    }

    public enum DirectionType
    {
        Up,
        Down,
        Left,
        Right,
    }
}