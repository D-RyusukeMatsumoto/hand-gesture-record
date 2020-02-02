using HandGestureRecord.Common;

namespace HandGestureRecord.GestureInput
{
    /// <summary>
    /// ジェスチャーの入力.
    /// </summary>
    public class GestureManager : RuntimeManagerBase
    {

        /*
        HandData leftHand;
        HandData rightHand;
        */
        
        
        void Awake()
        {
            ManagerProvider.RegisterRuntimeManager(this);
            DontDestroyOnLoad(gameObject);
        }


        void Start()
        {
            /*
            leftHand = new HandData(GameManager.GetPlayer().GetSkeleton(Player.SkeletonId.LeftHandSkeleton));
            rightHand = new HandData(GameManager.GetPlayer().GetSkeleton(Player.SkeletonId.RightHandSkeleton));
        */
        }


        void Update()
        {
#if !UNITY_EDITOR
            // こちらはQuestで利用するデータ.

            /*
            DebugWindow.SetDebugInfo("RThumb", $"RThumb : {rightHand.IsFingerStraight(0.8f, FingerId.Thumb)}");
            DebugWindow.SetDebugInfo("RIndex", $"RIndex : {rightHand.IsFingerStraight(0.8f, FingerId.Index)}");
            DebugWindow.SetDebugInfo("RMiddle", $"RMiddle : {rightHand.IsFingerStraight(0.8f, FingerId.Middle)}");
            DebugWindow.SetDebugInfo("RRing", $"RRing : {rightHand.IsFingerStraight(0.8f, FingerId.Ring)}");
            DebugWindow.SetDebugInfo("RPinky", $"RPinky : {rightHand.IsFingerStraight(0.8f, FingerId.Pinky)}");

            DebugWindow.SetDebugInfo("LThumb", $"LThumb : {leftHand.IsFingerStraight(0.8f, FingerId.Thumb)}");
            DebugWindow.SetDebugInfo("LIndex", $"LIndex : {leftHand.IsFingerStraight(0.8f, FingerId.Index)}");
            DebugWindow.SetDebugInfo("LMiddle", $"LMiddle : {leftHand.IsFingerStraight(0.8f, FingerId.Middle)}");
            DebugWindow.SetDebugInfo("LRing", $"LRing : {leftHand.IsFingerStraight(0.8f, FingerId.Ring)}");
            DebugWindow.SetDebugInfo("LPinky", $"LPinky : {leftHand.IsFingerStraight(0.8f, FingerId.Pinky)}");
            */

            /*
            bool thumb = rightHand.IsFingerStraight(0.8f, FingerId.Thumb);
            bool index = rightHand.IsFingerStraight(0.8f, FingerId.Index);
            bool middle = rightHand.IsFingerStraight(0.8f, FingerId.Middle);
            bool ring = rightHand.IsFingerStraight(0.8f, FingerId.Ring);
            bool pinky = rightHand.IsFingerStraight(0.8f, FingerId.Pinky);
*/
#elif UNITY_EDITOR
            // こちらはEditorで利用するLeapMotionのデータ.
#endif

        }



    }
}