using System.Collections.Generic;
using UnityEngine;


namespace Common
{
    /// <summary>
    /// プレイヤークラス.
    /// 主に各種トラッキング物への参照の受け渡しを行う.
    /// </summary>
    public class Player : MonoBehaviour
    {
        readonly string HandPrefab = "OVRHandPrefab";
        readonly string HeadObjName = "CenterEyeAnchor";
        readonly string LeftHandObjName = "LeftHandAnchor";
        readonly string RightHandObjName = "RightHandAnchor";

        public enum TrackingId : int
        {
            Head,
            LeftHand,
            RightHand,
        }


        public enum SkeletonId : int
        {
            LeftHandSkeleton,
            RightHandSkeleton,
        }


        [SerializeField] Transform trackingSpace;
        Dictionary<TrackingId, Transform> trackingObjDic;
        Dictionary<SkeletonId, OVRSkeleton> skeletonDic;

        // 左右の手のスケルトン.
        OVRSkeleton leftSkeleton;
        OVRSkeleton rightSkeleton;

        void Start()
        {
            ManagerProvider.RegisterPlayer(this);
            trackingObjDic = new Dictionary<TrackingId, Transform>();
            trackingObjDic.Add(TrackingId.Head, trackingSpace.Find(HeadObjName));
            trackingObjDic.Add(TrackingId.LeftHand, trackingSpace.Find(LeftHandObjName));
            trackingObjDic.Add(TrackingId.RightHand, trackingSpace.Find(RightHandObjName));

            skeletonDic = new Dictionary<SkeletonId, OVRSkeleton>();
#if UNITY_ANDROID && !UNITY_EDITOR
            skeletonDic.Add(SkeletonId.LeftHandSkeleton, trackingObjDic[TrackingId.LeftHand].Find(HandPrefab).GetComponent<OVRSkeleton>());
            skeletonDic.Add(SkeletonId.RightHandSkeleton, trackingObjDic[TrackingId.RightHand].Find(HandPrefab).GetComponent<OVRSkeleton>());
#elif UNITY_EDITOR
            skeletonDic.Add(SkeletonId.LeftHandSkeleton, null);
            skeletonDic.Add(SkeletonId.RightHandSkeleton, null);
#endif
        }


        /// <summary>
        /// トラッキングしているオブジェクトを取得.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Transform GetTrackingObject(TrackingId id) => trackingObjDic[id];


        /// <summary>
        /// 指定したSkeletonの取得.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OVRSkeleton GetSkeleton(SkeletonId id) => skeletonDic[id];


    }
}