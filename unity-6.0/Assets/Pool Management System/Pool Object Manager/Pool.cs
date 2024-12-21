using System.Collections.Generic;
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
        public UnityEvent<Pool, PoolObject[]> OnSpawn = new();
        public UnityEvent<Pool, PoolObject> OnDespawn = new();

        // public List<PoolObject> standbyObjects = new();
        public Queue<PoolObject> standbyObjects = new();
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
                standbyObjects.Enqueue(instance);
            }

            // standbyObjects.AddRange(initializedObjects.ToList());

            OnInitialize?.Invoke(this, initializedObjects);
        }

        [ContextMenu("Terminate")]
        public void Terminate()
        {
            foreach (var obj in AllObjects)
            {
                obj.Terminate();
            }

            standbyObjects.Clear();
            activeObjects.Clear();
        }

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            Spawn(1);
        }

        [ContextMenu("Spawn 5")]
        public void Spawn5()
        {
            Spawn(5);
        }


        public void Spawn(int num)
        {
            var spawnedObjects = new List<PoolObject>(num); // More flexible than arrays

            for (int i = 0; i < num; i++)
            {
                if (standbyObjects.Count <= 0)
                {
                    if (overflowType == OverflowType.Expandable)
                    {
                        Initialize(1);
                    }
                    else if (overflowType == OverflowType.Recycable)
                    {
                        Despawn(activeObjects[0]);
                    }
                }

                PoolObject obj = standbyObjects.Dequeue(); // Cache the object reference

                // PoolObject obj = standbyObjects[0]; // Cache the object reference
                // standbyObjects.RemoveAt(0);         // Remove it from the pool
                Spawn(obj);                         // Perform spawn operation
                spawnedObjects.Add(obj);            // Track spawned object
            }

            OnSpawn?.Invoke(this, spawnedObjects.ToArray());
        }

        private void Spawn(PoolObject poolObject)
        {
            activeObjects.Add(poolObject);
            poolObject.Spawn();
        }

        public void Despawn(PoolObject poolObject)
        {
            standbyObjects.Enqueue(poolObject);
            // standbyObjects.Add(poolObject);
            activeObjects.Remove(poolObject);
            poolObject.Despawn();
            OnDespawn?.Invoke(this, poolObject);
        }
    }
}
