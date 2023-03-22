using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Synith
{
    public class StatusUI : MonoBehaviour
    {
        [SerializeField] CatToyTracker catToyTracker;
        [SerializeField] TextMeshProUGUI text;

        void Start()
        {
            catToyTracker.OnToyDestroyedCount += CatToyTracker_OnToyDestroyedCount;
            catToyTracker.OnGameWon += CatToyTracker_OnGameWon;
        }

        void CatToyTracker_OnGameWon(object sender, System.EventArgs e)
        {
            Invoke(nameof(SetGameWonText), 1.5f);
        }

        void CatToyTracker_OnToyDestroyedCount(object sender, int e)
        {
            text.SetText($"{e} / {catToyTracker.CatToyMax}");
        }

        void SetGameWonText() => text.SetText("Bad Cat!");
    }
}
