using System;
using System.Collections;
using Domain.Enum;
using Domain.Interface;
using Services.AudioSystem.Interface;
using Services.UISystem.Interface;
using UnityEngine;
using Zenject;

namespace Services.UISystem.Abstract
{
    public class EachUIPanel<T> : MonoBehaviour, IPoolable<IMemoryPool>, IFactoryObject, IPoolObject, IUIScreen
    {
        [SerializeField] private bool _haveExitAnimation = true;
        private Animator _animator;
        private Transform _transform;
        private IMemoryPool _memoryPool;
        protected Action _onClose;

        protected IUIService UIService;
        protected IAudioService AudioService;

        public virtual UiPanelNames PanelName => UiPanelNames.None;
        public bool IsClosed { get; set; }
        
        public Transform ObjectRoot
        {
            get
            {
                if (_transform == null) _transform = transform;
                return _transform;
            }
        }

        public IMemoryPool MemoryPool => _memoryPool;

        [Inject]
        public virtual void Init(IUIService uiService,
            IAudioService audioService)
        {
            UIService = uiService;
            AudioService = audioService;
            _animator = GetComponent<Animator>();
        }

        public virtual void OnDespawned()
        {
        }

        public virtual void OnSpawned(IMemoryPool memoryPool)
        {
            IsClosed = false;
            _memoryPool = memoryPool;
        }

        public virtual void BackToPool()
        {
            if (_memoryPool == null) return;
            _memoryPool.Despawn(this);
            _memoryPool = null;
        }

        public virtual void Show(Action onClose)
        {
            PlayOnOpenSound();
            _onClose = onClose;
        }

        public virtual void PlayOnOpenSound()
        {
            AudioService.PlayUIAudio(AudioUITypes.OpenPage, _transform);
        }

        public virtual void Close()
        {
            if (IsClosed) return;
            AudioService.PlayUIAudio(AudioUITypes.ClosePage, _transform);
            if (_haveExitAnimation && _animator != null)
            {
                IsClosed = true;
                StartCoroutine(nameof(CloseByAnimation));
            }
            else
            {
                ClosePage();
            }
        }

        IEnumerator CloseByAnimation()
        {
            _animator.Play("CloseUI", 0, 0);
            yield return new WaitForSeconds(0.4f);
            ClosePage();
        }

        private void ClosePage()
        {
            BackToPool();
            IsClosed = true;
            _onClose?.Invoke();
            _onClose = null;
            UIService.ClosePage(PanelName, null);
        }

        public virtual void SetParent(Transform parent)
        {
            ObjectRoot.SetParent(parent);
            ObjectRoot.localPosition = Vector3.zero;
            ObjectRoot.localScale = Vector3.one;
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }

        public class Factory : PlaceholderFactory<T>
        {
        }
    }
}