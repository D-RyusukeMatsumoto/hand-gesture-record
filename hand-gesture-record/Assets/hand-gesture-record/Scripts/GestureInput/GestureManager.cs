using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HandGestureRecord.Settings;

using AppFw.Core;

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


        // レコードデータのパスをまとめたインデックス.
        [SerializeField] GestureDataIndex dataIndex;

        [SerializeField] LeapServiceProvider provider;
        [SerializeField] List<GestureRecordData> list = new List<GestureRecordData>();
        bool isInitialized = false;
        
        
        void Awake()
        {
            ManagerProvider.RegisterRuntimeManager(this);
            DontDestroyOnLoad(gameObject);
 
            // ここでロードする.
            if (dataIndex != null)
            {
                foreach (var path in dataIndex.PathIndex)
                {
                    var data = Resources.Load<GestureRecordData>(path.ResourcesPath);
                    if (data != null)
                    {
                        list.Add(data);
                    }
                }
            }
            
        }


        void Start()
        {
            StartCoroutine(Initialize());
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


            if (list != null)
            {
                foreach (var element in list)
                {
                    DebugWindow.SetDebugInfo($"Gesture{element.GestureName}", $"{element.GestureName} {CorrectGesture(HandId.RightHand, element.Data)}");
                }
            }
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
            leftHand = new QuestHandData(ManagerProvider.GetPlayer().GetSkeleton(Player.SkeletonId.LeftHandSkeleton));
            rightHand = new QuestHandData(ManagerProvider.GetPlayer().GetSkeleton(Player.SkeletonId.RightHandSkeleton));
#elif !UNITY_ANDROID && UNITY_EDITOR
            // TODO : これもPlayerに登録したものから取得するようにする?.
            yield return null;
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
            if (handId == HandId.LeftHand)
                return leftHand != null && leftHand.IsFingerStraight(threshold, fingerId);
            else
                return rightHand != null && rightHand.IsFingerStraight(threshold, fingerId);

            return false;
        }

        
        /// <summary>
        /// ジェスチャが合致しているか判定.
        /// </summary>
        /// <param name="leftRight"></param>
        /// <param name="targetGesture"></param>
        /// <param name="threshold"></param>
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