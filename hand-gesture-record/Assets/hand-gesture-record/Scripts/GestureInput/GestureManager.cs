using System.Collections;
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
#if !UNITY_ANDROID && UNITY_EDITOR
        [SerializeField] LeapServiceProvider provider;
#endif
        // TODO : テスト用データ.
        [SerializeField] GestureRecordData gu;
        [SerializeField] GestureRecordData choki;
        [SerializeField] GestureRecordData par;
        bool isInitialized = false;
        
        
        void Awake()
        {
            ManagerProvider.RegisterRuntimeManager(this);
            DontDestroyOnLoad(gameObject);
        }


        void Start()
        {
            StartCoroutine(this.Initialize());
        }


        void Update()
        {
            if (!isInitialized) return;
            
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

            DebugWindow.SetDebugInfo("Gesture - Gu", $"グー {this.CorrectGesture(HandId.RightHand, gu.Data)}");
            DebugWindow.SetDebugInfo("Gesture - Choki", $"チョキ {this.CorrectGesture(HandId.RightHand, choki.Data)}");
            DebugWindow.SetDebugInfo("Gesture - Par", $"パー {this.CorrectGesture(HandId.RightHand, par.Data)}");
        }


        IEnumerator Initialize()
        {
#if UNITY_ANDROID
            Player player;
            yield return new WaitUntil(() =>
            {
                player = ManagerProvider.GetPlayer();
                return player != null;
            });
            // TODO : 今回のプロジェクトではQuestのPlayerはまだ実装していないのでここはコメントアウトしておく.
            leftHand = new QuestHandData(ManagerProvider.GetPlayer().GetSkeleton(Player.SkeletonId.LeftHandSkeleton));
            rightHand = new QuestHandData(ManagerProvider.GetPlayer().GetSkeleton(Player.SkeletonId.RightHandSkeleton));
#elif !UNITY_ANDROID && UNITY_EDITOR
            // TODO : これもPlayerに登録したものから取得するようにする?.
            yiled return null;
            leftHand = new LeapMotionHandData(HandId.LeftHand, provider);
            rightHand = new LeapMotionHandData(HandId.RightHand, provider);
#endif
            isInitialized = true;
        }

        
        /// <summary>
        /// 指定した指が伸びているか否か.
        /// </summary>
        /// <param name="handId"></param>
        /// <param name="fingerId"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public bool IsFingerStraight(
            HandId handId,
            FingerId fingerId,
            float threshold)
        {
            bool ret = false;
            if (handId == HandId.LeftHand)
            {
                ret = leftHand != null && leftHand.IsFingerStraight(threshold, fingerId);
            }
            else
            {
                ret = rightHand != null && rightHand.IsFingerStraight(threshold, fingerId);
            }

            return ret;
        }

        
        /// <summary>
        /// ジェスチャが合致しているか判定.
        /// </summary>
        /// <param name="leftRight"></param>
        /// <param name="targetGesture"></param>
        /// <returns></returns>
        public bool CorrectGesture(
            HandId leftRight,
            HandDataBase.FingerStraightInfo targetGesture, 
            float threshold = 0.8f)
        {
            HandDataBase.FingerStraightInfo fingerStraightInfo = (leftRight == HandId.LeftHand) 
                ? leftHand.GetFingerStraightInfo(threshold) 
                : rightHand.GetFingerStraightInfo(threshold);

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