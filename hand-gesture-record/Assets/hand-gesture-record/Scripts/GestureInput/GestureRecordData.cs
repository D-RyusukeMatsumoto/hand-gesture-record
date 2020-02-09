using UnityEngine;

namespace HandGestureRecord.GestureInput
{
    /// <summary>
    /// 記録したジェスチャーデータ.
    /// </summary>
    public class GestureRecordData : ScriptableObject
    {
        [SerializeField] HandDataBase.FingerStraightRatioInfo fingerStraightRatioInfo;
        public HandDataBase.FingerStraightRatioInfo FingerStraightRatio => fingerStraightRatioInfo;

        public GestureRecordData(
            HandDataBase.FingerStraightRatioInfo argFingerStraightRatioInfo)
        {
            fingerStraightRatioInfo = argFingerStraightRatioInfo;
        }

    }
}