using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Utils
{
    /// <summary>
    /// Very simple basic pool for more efficient GC and memory management
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> where T : Component
    {
        private readonly GameObject prefab;
        private readonly DiContainer diContainer;
        private readonly Transform parent;
        
        private readonly Dictionary<int, T> activeObjects;
        private Queue<T> poolQueue;
        
        public Pool(DiContainer diContainer, GameObject prefab, Transform parent, int minimumItemAmount = 0) 
        {
            this.prefab = prefab;
            this.diContainer = diContainer;
            this.parent = parent;
            
            activeObjects = new Dictionary<int, T>(minimumItemAmount);
        }

        public T Rent()
        {
            if (poolQueue == null) 
                poolQueue = new Queue<T>();

            var instance = (poolQueue.Count > 0)
                ? poolQueue.Dequeue()
                : CreatePooledObject();

            instance.gameObject.SetActive(true);
            return instance;
        }
        
        public void Return(T instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");

            if (poolQueue == null) 
                poolQueue= new Queue<T>();
            
            instance.gameObject.SetActive(false);
            poolQueue.Enqueue(instance);
        }

        private T CreatePooledObject()
        {
            return diContainer.InstantiatePrefabForComponent<T>(prefab, parent);
        }

        public void ReturnAll()
        {
           // throw new NotImplementedException();
        }

        public void Clear()
        {
        }
    }
}