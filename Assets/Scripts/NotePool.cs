using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class NotePool : MonoBehaviour
    {
        private const int PoolCount = 100;

        [SerializeField]
        private NoteView notePrefab;

        private readonly Queue<NoteView> _notePrefabPool = new ();

        private void Start()
        {
            for (int i = 0; i < PoolCount; i++)
            {
                var note = Instantiate(notePrefab, transform);
                note.gameObject.SetActive(false);
                _notePrefabPool.Enqueue(note);
            }
        }

        public NoteView GetNote()
        {
            if (_notePrefabPool.Count == 0)
            {
                // Optionally, create a new note if the pool is empty.
                // This depends on whether you want a fixed-size pool or a flexible one.
                var newNote = Instantiate(notePrefab, transform);
                newNote.gameObject.SetActive(false);
                _notePrefabPool.Enqueue(newNote);
            }

            var note = _notePrefabPool.Dequeue();
            return note;
        }

        public void ReturnNote(NoteView note)
        {
            note.gameObject.SetActive(false);
            _notePrefabPool.Enqueue(note);
        }
    }
}