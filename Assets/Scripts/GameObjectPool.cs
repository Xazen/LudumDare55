using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameObjectPool : MonoBehaviour
    {
        [SerializeField]
        private int poolCount = 50;

        [SerializeField]
        private GameObject notePrefab;

        [SerializeField]
        private float autoReturnTime = 2f;

        private readonly Queue<GameObject> _gameObjectPool = new ();

        private readonly List<GameObjectLifeTime> _gameObjectTime = new ();

        private class GameObjectLifeTime
        {
            public GameObject GameObject;
            public float LifeTime;
        }

        private void Start()
        {
            for (int i = 0; i < poolCount; i++)
            {
                var note = Instantiate(notePrefab, transform);
                note.gameObject.SetActive(false);
                _gameObjectPool.Enqueue(note);
            }
        }

        private void Update()
        {
            List<GameObjectLifeTime> objectsToRemove = new ();
            foreach (var gameObjectLifeTime in _gameObjectTime)
            {
                gameObjectLifeTime.LifeTime -= Time.deltaTime;
                if (gameObjectLifeTime.LifeTime <= 0)
                {
                    objectsToRemove.Add(gameObjectLifeTime);
                    ReturnObject(gameObjectLifeTime.GameObject);
                }
            }

            foreach (var objectToRemove in objectsToRemove)
            {
                _gameObjectTime.Remove(objectToRemove);
            }
        }

        public GameObject GetObject()
        {
            if (_gameObjectPool.Count == 0)
            {
                // Optionally, create a new note if the pool is empty.
                // This depends on whether you want a fixed-size pool or a flexible one.
                var newO = Instantiate(notePrefab, transform);
                newO.gameObject.SetActive(false);
                _gameObjectPool.Enqueue(newO);
            }

            var o = _gameObjectPool.Dequeue();
            _gameObjectTime.Add(new GameObjectLifeTime
            {
                GameObject = o,
                LifeTime = autoReturnTime
            });
            return o;
        }

        public void ReturnObject(GameObject gameObject)
        {
            gameObject.gameObject.SetActive(false);
            _gameObjectPool.Enqueue(gameObject);
        }
    }
}