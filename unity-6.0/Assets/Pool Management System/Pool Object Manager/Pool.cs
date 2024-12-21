using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement
{
    public class Pool : MonoBehaviour
    {
        public PoolObject poolObject;

        [Header("Configuration")]
        public int numToInitialize;
        public bool initializeOnStart;

        public enum OverflowType { Expandable, Recycable }
        public OverflowType overflowType;

        [Header("Events")]
        public UnityEvent<Pool, PoolObject[]> OnInitialize = new();
        public UnityEvent<Pool, PoolObject> OnSpawn = new();
        public UnityEvent<Pool, PoolObject> OnDespawn = new();

        public List<PoolObject> standbyObjects = new();

        public List<PoolObject> activeObjects = new();

        public IEnumerable<PoolObject> AllObjects
        {
            get
            {
                foreach (var obj in standbyObjects) yield return obj;
                foreach (var obj in activeObjects) yield return obj;
            }
        }

        private void Start()
        {
            if (initializeOnStart) Initialize();
        }

        [ContextMenu("Initialize")]
        public void Initialize()
        {
            Initialize(numToInitialize);
        }

        public void Initialize(int initializeCount)
        {
            PoolObject[] initializedObjects = new PoolObject[initializeCount];


            for (int i = 0; i < initializeCount; i++)
            {
                var instance = Instantiate(poolObject);
                instance.Initialize(this);
                initializedObjects[i] = instance;
            }

            standbyObjects.AddRange(initializedObjects.ToList());

            OnInitialize?.Invoke(this, initializedObjects);
        }

        [ContextMenu("Terminate")]
        public void Terminate()
        {
            foreach (var obj in AllObjects)
            {
                obj.Terminate();
            }

            activeObjects.Clear();
        }

        [ContextMenu("Spawn 1")] private void Spawn1() { Spawn(1); }
        [ContextMenu("Spawn 5")] private void Spawn5() { Spawn(5); }
        [ContextMenu("Spawn 10")] private void Spawn10() { Spawn(10); }

        public void Spawn(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (standbyObjects.Count <= 0)
                {
                    if (overflowType == OverflowType.Expandable)
                    {
                        Initialize(1);
                    }
                    else if (overflowType == OverflowType.Recycable)
                    {
                        standbyObjects.Add(activeObjects[0]);
                        activeObjects.RemoveAt(0);
                    }
                }

                Spawn(standbyObjects[0]);
            }
        }

        public void Spawn(PoolObject poolObject)
        {
            standbyObjects.Remove(poolObject);
            activeObjects.Add(poolObject);
            poolObject.Spawn();
            OnSpawn?.Invoke(this, poolObject);
        }

        // public void Despawn(PoolObject poolObject)
        // {
        //     if (storageType == StorageType.Queue)
        //         standbyObjectsQueue.Enqueue(poolObject);

        //     if (storageType == StorageType.List)
        //         standbyObjectsList.Add(poolObject);

        //     activeObjects.Remove(poolObject);
        //     poolObject.Despawn();
        //     OnDespawn?.Invoke(this, poolObject);
        // }
    }
}
