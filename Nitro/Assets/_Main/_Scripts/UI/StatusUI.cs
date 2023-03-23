using TMPro;
using UnityEngine;

namespace Synith
{
    public class StatusUI : MonoBehaviour, IObserver<ToySpawn>
    {
        [SerializeField] CatToyTracker catToyTracker;
        [SerializeField] TextMeshProUGUI text;

        void Start()
        {
            catToyTracker.Subscribe(this);
        }

        public void OnNotify(ToySpawn value)
        {
            UpdateToyCountText();
        }

        void OnDestroy()
        {
            catToyTracker.Unsubscribe(this);
        }

        void UpdateToyCountText()
        {
            text.SetText($"{catToyTracker.CatToyCurrent} / {catToyTracker.CatToyMax}");
        }
    }
}
