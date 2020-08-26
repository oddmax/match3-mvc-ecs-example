using System.Collections.Generic;
using Features.Data;
using Features.Data.Components;
using Features.Models;
using Features.Signals;
using Features.Views;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Features.Presenters
{
    /// <summary>
    /// Manages player input on the board and triggers swap via signal
    /// </summary>
    public class InputPresenter : MonoBehaviour
    {
        private Entity firstSwapEntity = Entity.Null;
        private Entity secondSwapEntity = Entity.Null;

        private GemView currentGemUnderInput;
        private GemView selectedGem;

        private readonly Dictionary<Entity, GemView> gemViews = new Dictionary<Entity, GemView>();
        private EntityManager entityManager;
        private SignalBus signalBus;
        private GameStateModel gameStateModel;
        private BoardModel boardModel;
        private Camera cameraMain;
        
        [Inject]
        public void Inject(EntityManager entityManager, SignalBus signalBus, DiContainer diContainer, GameStateModel gameStateModel, BoardModel boardModel)
        {
            this.gameStateModel = gameStateModel;
            this.entityManager = entityManager;
            this.signalBus = signalBus;
            this.boardModel = boardModel;
        }

        private void Start()
        {
            cameraMain = Camera.main;
        }

        void Update()
        {
            if(gameStateModel.State != Match3State.PlayerTurn)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                currentGemUnderInput = null;
                var gemUnderMouse = GetGemUnderMouse();
                if (gemUnderMouse != null)
                {
                    currentGemUnderInput = gemUnderMouse;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                var gem = GetGemUnderMouse();
                
                if (currentGemUnderInput == gem)
                {
                    // Same gem as mouse down
                    if (gem != null)
                    {
                        if (gem != selectedGem)
                            SelectGem(gem);
                        else
                            Deselect();
                    }
                }
                else
                {
                    if(currentGemUnderInput != null)
                        CheckSwipe(currentGemUnderInput);
                }

                currentGemUnderInput = null;
            }

        }

        private void CheckSwipe(GemView fromGem)
        {
            var touchPosWorld = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            
            Vector2 swipe = new Vector2(touchPosWorld2D.x - fromGem.transform.position.x, touchPosWorld2D.y - fromGem.transform.position.y);
            if (swipe.magnitude < 0.05f) // Too short swipe
                return;
            
            var fromPosition = entityManager.GetComponentData<BoardPositionComponent>(fromGem.Entity);
            int2 nextGemCoord = fromPosition.Position;
            if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
            {
                if (swipe.x > 0)
                    nextGemCoord.x += 1;
                else {
                    nextGemCoord.x -= 1;
                }
            }
            else
            {
                if (swipe.y > 0)
                    nextGemCoord.y += 1;
                else {
                    nextGemCoord.y -= 1;
                }
            }

            if (boardModel.IsPositionInBorders(nextGemCoord) && boardModel.HasEntityAt(nextGemCoord))
            {
                StartSwap(fromGem.Entity, boardModel.GetEntityAt(nextGemCoord));
            }
        }

        private void Deselect()
        {
            if(selectedGem != null)
                selectedGem.Deselect();
            selectedGem = null;
        }

        private void Select(GemView gem)
        {
            Deselect();
            selectedGem = gem;
            selectedGem.Select();
            Debug.Log("Select gem " + gem.Entity.Index);
        }

        private void CheckSwipe(GemView gem1, GemView gem2)
        {
            Debug.Log("Swipe attempt " + gem1.Entity.Index + " - " + gem1.Entity.Index);
            var gem1Position = entityManager.GetComponentData<BoardPositionComponent>(gem1.Entity);
            var gem2Position = entityManager.GetComponentData<BoardPositionComponent>(gem2.Entity);

            if (BoardCalculator.IsNextToEachOther(gem1Position.Position, gem2Position.Position))
            {
                StartSwap(gem1.Entity, gem2.Entity);
            }
        }
        
        private void SelectGem(GemView gem)
        {
            if (selectedGem == null)
            {
                Select(gem);
            }
            else
            {
                var gemPosition = entityManager.GetComponentData<BoardPositionComponent>(gem.Entity);
                var selectedGemPosition = entityManager.GetComponentData<BoardPositionComponent>(selectedGem.Entity);
                Debug.Log("Select second gem " + gem.Entity.Index);
                if (BoardCalculator.IsNextToEachOther(gemPosition.Position, selectedGemPosition.Position))
                {
                    StartSwap(gem.Entity, selectedGem.Entity);
                }
                else
                {
                    Select(gem);
                }
            }
        }

        private GemView GetGemUnderMouse()
        {
           return GetGemUnderPoint(cameraMain.ScreenToWorldPoint(Input.mousePosition)); 
        }

        private GemView GetGemUnderPoint(Vector3 touchPosWorld)
        {
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, cameraMain.transform.forward);
            if (hitInformation.collider != null) {
                GameObject touchedObject = hitInformation.transform.gameObject;
                var gemView = touchedObject.GetComponent<GemView>();
                if (gemView != null)
                {
                    return gemView;
                }
            }

            return null;
        }

        private void StartSwap(Entity gem1, Entity gem2)
        {
            if(gameStateModel.State != Match3State.PlayerTurn)
                return;
            
            if (gem1 != Entity.Null && gem2 != Entity.Null)
            {
                var gem1BoardPosition = entityManager.GetComponentData<BoardPositionComponent>(gem1);
                var gem2BoardPosition = entityManager.GetComponentData<BoardPositionComponent>(gem2);
                signalBus.Fire(new Match3Signals.StartSwapSignal(gem1BoardPosition.Position, gem2BoardPosition.Position));
                
                Deselect();
                currentGemUnderInput = null;
            }
        }
    }
}