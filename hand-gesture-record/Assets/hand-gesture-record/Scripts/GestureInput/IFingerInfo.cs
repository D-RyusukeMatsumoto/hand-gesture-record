namespace HandGestureRecord.GestureInput
{
    /// <summary>
    /// 各指の情報を取得するためのインターフェース.
    /// </summary>
    public interface IFingerInfo
    {

        HandDataBase.FingerStraightRatioInfo GetFingerStraightInfo();

    }
}