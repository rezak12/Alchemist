using DG.Tweening;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(fileName = "NewPotionTween", menuName = "StaticData/Tweens/Potion")]
    public class PotionTween : ScriptableObject
    {
        [field: SerializeField, Header("Move Up")] public float MoveUpPoints { get; private set; }
        [field: SerializeField] public float MoveUpDuration { get; private set; }
        [field: SerializeField] public float OneRotateDuration { get; private set; }
        [field: SerializeField] public Ease RotationEase { get; private set; }
        
        [field: SerializeField, Header("Move To Camera")] public float MoveToCameraDuration { get; private set; }
        [field: SerializeField] public float CameraZOffset { get; private set; }
        [field: SerializeField] public Ease MoveToCameraEase { get; private set; }
        
        [field: SerializeField, Header("Scale")] public float ScaleDuration { get; private set; }
        [field: SerializeField] public Ease ScaleEase { get; private set; }
    }
}