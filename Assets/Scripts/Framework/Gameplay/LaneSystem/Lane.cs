using UnityEngine;
using UnityEngine.UI;
using ProefExamen.Framework.Gameplay.Values;
using System.Collections.Generic;
using ProefExamen.Framework.Utils.Libraries.LaneUtils;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    [RequireComponent(typeof(Button))]
    public class Lane : MonoBehaviour
    {
        [SerializeField]
        private int _id = -1;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private List<GameObject> _notes = new();

        public Button Button => _button;

        void Start()
        {
            Input.GetKey(KeyCode.Escape);

            _button = GetComponent<Button>();

            if (_id != -1)
                _button.onClick.AddListener(SendButtonPress);
            else
                Debug.LogError("Lane with button " + _button.name + " has no assigned ID!");
        }

        private void SendButtonPress()
        {
            if (!(_notes.Count > 0))
            {
                SessionValues.score -= 1;
                return;
            }

            GameObject nextNote = _notes[0];
            Note nextNoteScript = nextNote.GetComponent<Note>();

            if (nextNoteScript.LerpAlpha > .3f && nextNoteScript.LerpAlpha < .7f)
            {
                float differenceAlpha = Mathf.Abs(nextNoteScript.LerpAlpha - .5f) / .2f;

                HitStatus hitResult = LaneUtils.ReturnHitStatus(differenceAlpha);

                Debug.Log(_button.name + " was pressed and got a " + hitResult + " hit!");

                SessionValues.score += (int)hitResult;

                Destroy(nextNote);
                RemoveNote(nextNote);
            }
        }

        public void SpawnNote(float timeStamp)
        {
            GameObject newNote = Instantiate(SessionValues.note);
            _notes.Add(newNote);

            Vector2 initialPos = new Vector2(-2.1f + ((_id * 0.7f) * 2), 6);
            Vector2 targetPos = new Vector2(initialPos.x, -6);

            newNote.transform.position = new Vector3(initialPos.x, initialPos.y, 0);

            Note newNoteScript = newNote.GetComponent<Note>();

            newNoteScript.SetNoteValues(initialPos, targetPos, _id, SessionValues.currentLevel.levelID, timeStamp);
            newNoteScript.CallNoteRemoval += RemoveNote;
        }

        public void RemoveNote(GameObject note)
        {
            _notes.Remove(note);
        }
    }
}