using Core.Config;
using DG.Tweening;
using Features.Data.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Zenject;

namespace Features.Views
{
    public delegate void OnTouchDown(Entity entity);  
    public delegate void OnTouchUp(Entity entity);  
    public delegate void OnDestructionAnimationComplete(GemView view);  
    
    public class GemView : MonoBehaviour
    {
        [SerializeField] 
        private SpriteRenderer spriteRenderer;
        
        [SerializeField] 
        public float destroyDuration = 0.3f;

        public event OnTouchDown OnTouchDown;
        public event OnTouchUp OnTouchUp;
        public event OnDestructionAnimationComplete OnDestructionAnimationComplete;
        
        private int entityId;
        private EntityManager entityManager;
        private Entity entity;
        private float3 originPosition;
        private string prefabName;
        
        private GameObject gemGameObject;
        private Transform gemTransform;
        private Vector3 defaultScale;
        private AssetsCatalogue catalogue;
        public Entity Entity
        {
            get => entity;
        }

        [Inject]
        private void Inject(AssetsCatalogue catalogue)
        {
            this.catalogue = catalogue;
        }
        
        private void Awake()
        {
            gemTransform = transform;
            defaultScale = gameObject.transform.localScale;
        }

        private void OnMouseDown()
        {
            OnTouchDown?.Invoke(entity);
        }

        private void OnMouseUp()
        {
            OnTouchUp?.Invoke(entity);
        }

        public void DestroyAnimation()
        {
            gameObject.transform
                .DOScale(0.1f, destroyDuration)
                .OnComplete(() =>
                {
                    gameObject.transform.localScale = defaultScale;
                    OnDestructionAnimationComplete?.Invoke(this);
                });
        }

        public void Init(EntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public void SetData(Entity entity, float3 originPosition)
        {
            this.entity = entity;
            this.originPosition = originPosition;

            var gemComponent = entityManager.GetComponentData<GemComponent>(entity);
            spriteRenderer.sprite = catalogue.GetSpriteConfig(gemComponent.Color);
        }
        
        public void UpdatePosition()
        {
            var localTransform = entityManager.GetComponentData<LocalTransform>(entity);
            gemTransform.localPosition = localTransform.Position;
        }

        public void Select()
        {
            spriteRenderer.color = Color.gray;
        }

        public void Deselect()
        {
            spriteRenderer.color = Color.white;
        }
    }
}