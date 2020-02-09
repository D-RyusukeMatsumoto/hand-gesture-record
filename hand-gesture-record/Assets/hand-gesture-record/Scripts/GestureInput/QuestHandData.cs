using UnityEngine;

namespace HandGestureRecord.GestureInput
{
    /// <summary>
    /// OVRSkeletonを利用した手のデータ.
    /// </summary>
    public class QuestHandData : HandDataBase
    {
        readonly OVRSkeleton handSkeleton;
        readonly OVRSkeleton.BoneId[] thumb;
        readonly OVRSkeleton.BoneId[] index;
        readonly OVRSkeleton.BoneId[] middle;
        readonly OVRSkeleton.BoneId[] ring;
        readonly OVRSkeleton.BoneId[] pinky;

        
        public QuestHandData(
            OVRSkeleton skeleton)
        {
            handSkeleton = skeleton;

            thumb = new []
            {
                OVRSkeleton.BoneId.Hand_Thumb0,
                OVRSkeleton.BoneId.Hand_Thumb1,
                OVRSkeleton.BoneId.Hand_Thumb2,
                OVRSkeleton.BoneId.Hand_Thumb3,
                OVRSkeleton.BoneId.Hand_ThumbTip,
            };
            
            index = new []
            {
                OVRSkeleton.BoneId.Hand_Index1,
                OVRSkeleton.BoneId.Hand_Index2,
                OVRSkeleton.BoneId.Hand_Index3,
                OVRSkeleton.BoneId.Hand_IndexTip,
            };
            
            middle = new []
            {
                OVRSkeleton.BoneId.Hand_Middle1,                
                OVRSkeleton.BoneId.Hand_Middle2,                
                OVRSkeleton.BoneId.Hand_Middle3,                
                OVRSkeleton.BoneId.Hand_MiddleTip,                
            };

            ring = new []
            {
                OVRSkeleton.BoneId.Hand_Ring1,
                OVRSkeleton.BoneId.Hand_Ring2,
                OVRSkeleton.BoneId.Hand_Ring3,
                OVRSkeleton.BoneId.Hand_RingTip,
            };
            
            pinky = new []
            {
                OVRSkeleton.BoneId.Hand_Pinky0,
                OVRSkeleton.BoneId.Hand_Pinky1,
                OVRSkeleton.BoneId.Hand_Pinky2,
                OVRSkeleton.BoneId.Hand_Pinky3,
                OVRSkeleton.BoneId.Hand_PinkyTip,
            };
        }
        
        
        /// <summary>
        /// 指定した指のIdからVector3の配列を取得.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override Vector3[] CreatePositionFingerPositionArray(
            FingerId id)
        {
            OVRSkeleton.BoneId[] sourceFinger = null;
            switch (id)
            {
                case FingerId.Thumb: sourceFinger = thumb; break;
                case FingerId.Index: sourceFinger = index; break;
                case FingerId.Middle: sourceFinger = middle; break;
                case FingerId.Ring: sourceFinger = ring; break;
                case FingerId.Pinky: sourceFinger = pinky; break;
            }
            if (sourceFinger == null) return null;

            Vector3[] ret = new Vector3[sourceFinger.Length];
            for (var i = 0; i < ret.Length; ++i)
            {
                ret[i] = handSkeleton.Bones[(int) sourceFinger[i]].Transform.position;
            }

            return ret;
        }
        

        /// <summary>
        /// 指定したFingerIdの指がまっすぐになっているか判定.
        /// </summary>
        /// <param name="threshold">閾値, 1に近いほど判定が厳しい.</param>
        /// <param name="fingerId"></param>
        /// <returns></returns>
        public override bool IsFingerStraight(
            float threshold,
            FingerId fingerId)
        {
            Vector3[] array = this.CreatePositionFingerPositionArray(fingerId);
            if (array == null) return false;
            
            // 3未満の要素は計算に入れない.
            if (array.Length < 3) return false;

            return threshold <= this.DotByFingerDirection(array);
        }


        /// <summary>
        ///　指定した指のIDからどれくらい指を伸ばしているかを取得.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override float GetDotByFinger(
            FingerId id)
        {
            return this.DotByFingerDirection(this.CreatePositionFingerPositionArray(id));
        }

        
        /// <summary>
        /// 指の直線の比率をまとめたデータの取得.
        /// </summary>
        /// <returns></returns>
        public override FingerStraightInfo GetFingerStraightInfo(
            float threshold)
        {
            return new FingerStraightInfo
            {
                thumb = this.IsFingerStraight(threshold, FingerId.Thumb),
                index = this.IsFingerStraight(threshold, FingerId.Index),
                middle = this.IsFingerStraight(threshold, FingerId.Middle),
                ring = this.IsFingerStraight(threshold, FingerId.Ring),
                pinky = this.IsFingerStraight(threshold, FingerId.Pinky)
            };
        }
        
    }
}