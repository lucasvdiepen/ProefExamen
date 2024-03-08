using System;
using System.Collections;
using ProefExamen.Framework.Gameplay.Level;
using ProefExamen.Framework.Utils;
using UnityEngine;

namespace ProefExamen.Framework.UI
{
    public class MenuStateUpdater : AbstractSingleton<MenuStateUpdater>
    {
        public Action<Difficulty> OnDifficultyChanged;

        public Action<int> OnHighScoreChanged;

        public Action<int> OnSelectedLevelIDChanged;

        private void Start()
        {
            StartCoroutine(test());
        }

        public IEnumerator test()
        {
            Debug.Log("waiting for 20 seconds");
            yield return new WaitForSeconds(20f);
            OnDifficultyChanged?.Invoke(Difficulty.Normal);

            Debug.Log("waiting completed.");
        }
    }
}
