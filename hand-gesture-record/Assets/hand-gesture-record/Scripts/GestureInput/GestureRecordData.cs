using UnityEngine;

namespace HandGestureRecord.GestureInput
{
    /// <summary>
    /// 記録したジェスチャーデータ.
    /// </summary>
    [System.Serializable]
    public class GestureRecordData : ScriptableObject
    {
        [SerializeField] HandDataBase.FingerStraightInfo data;

        public HandDataBase.FingerStraightInfo Data => data;

        public GestureRecordData(
            HandDataBase.FingerStraightInfo argData)
        {
            data = argData;
        }

        public void SetData(
            HandDataBase.FingerStraightInfo argData)
        {
            data = argData;
        }

    }
}