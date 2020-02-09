using System.Linq;
using HandGestureRecord.Common;
using UnityEngine;

#if UNITY_EDITOR
using Leap.Unity;
#endif


namespace HandGestureRecord.GestureInput
{
    /// <summary>
    /// ジェスチャーの入力.
    /// </summary>
    public class GestureManager : RuntimeManagerBase
    {
        HandDataBase leftHand;
        HandDataBase rightHand;
        
        // とりあえずインスペクターでセットする.
        [SerializeField] LeapServiceProvider provider;

        GestureRecordData testData;
        
        
        void Awake()
        {
            ManagerProvider.RegisterRuntimeManager(this);
            DontDestroyOnLoad(gameObject);

            testData = ScriptableObject.CreateInstance<GestureRecordData>();
        }


        void Start()
        {
#if !UNITY_EDITOR
            // TODO : 今回のプロジェクトではQuestのPlayerはまだ実装していないのでここはコメントアウトしておく.
            leftHand = new QuestHandData(GameManager.GetPlayer().GetSkeleton(Player.SkeletonId.LeftHandSkeleton));
            rightHand = new QuestHandData(GameManager.GetPlayer().GetSkeleton(Player.SkeletonId.RightHandSkeleton));
#elif UNITY_EDITOR
            // TODO : これもPlayerに登録したものから取得するようにする?.
            leftHand = new LeapMotionHandData(HandId.LeftHand, provider);
            rightHand = new LeapMotionHandData(HandId.RightHand, provider);
#endif
        }


        void Update()
        {
            bool thumb = rightHand.IsFingerStraight(0.8f, FingerId.Thumb);
            bool index = rightHand.IsFingerStraight(0.8f, FingerId.Index);
            bool middle = rightHand.IsFingerStraight(0.8f, FingerId.Middle);
            bool ring = rightHand.IsFingerStraight(0.8f, FingerId.Ring);
            bool pinky = rightHand.IsFingerStraight(0.8f, FingerId.Pinky);
            
            DebugWindow.SetDebugInfo("Thumb", $"Thumb {thumb} : {rightHand.GetDotByFinger(FingerId.Thumb)}");
            DebugWindow.SetDebugInfo("Index", $"Index {index} : {rightHand.GetDotByFinger(FingerId.Index)}");
            DebugWindow.SetDebugInfo("Middle", $"Middle {middle} : {rightHand.GetDotByFinger(FingerId.Middle)}");
            DebugWindow.SetDebugInfo("Ring", $"Ring {ring} : {rightHand.GetDotByFinger(FingerId.Ring)}");
            DebugWindow.SetDebugInfo("Pinky", $"Pinky {pinky} : {rightHand.GetDotByFinger(FingerId.Pinky)}");

            DebugWindow.SetDebugInfo("Gesture", $"Gu {this.CorrectGesture(HandId.RightHand, testData.FingerStraightRatio)}");
        }

        
        /// <summary>
        /// ジェスチャが合致しているか判定.
        /// </summary>
        /// <param name="leftRight"></param>
        /// <param name="targetGesture"></param>
        /// <returns></returns>
        public bool CorrectGesture(
            HandId leftRight,
            HandDataBase.FingerStraightRatioInfo targetGesture, 
            float threshold = 0.8f)
        {
            HandDataBase.FingerStraightRatioInfo fingerStraightInfo = (leftRight == HandId.LeftHand) ? 
                leftHand.GetFingerStraightInfo(threshold) : rightHand.GetFingerStraightInfo(threshold);

            bool[] fingerStraightRatios = new[]
            {
                targetGesture.thumb == fingerStraightInfo.thumb,
                targetGesture.index == fingerStraightInfo.index,
                targetGesture.middle == fingerStraightInfo.middle,
                targetGesture.ring == fingerStraightInfo.ring,
                targetGesture.pinky == fingerStraightInfo.pinky
            };
            return !fingerStraightRatios.Contains(false);
        }

    }
}