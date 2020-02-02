using UnityEngine;

namespace HandGestureRecord.GestureInput
{
    #if false
    /// <summary>
    /// OVRSkeletonを利用した手のデータ.
    /// </summary>
    public class HandData
    {
        readonly OVRSkeleton handSkeleton;
        readonly OVRSkeleton.BoneId[] thumb;
        readonly OVRSkeleton.BoneId[] index;
        readonly OVRSkeleton.BoneId[] middle;
        readonly OVRSkeleton.BoneId[] ring;
        readonly OVRSkeleton.BoneId[] pinky;

        
        public HandData(
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
        /// 指定したすべてのBoneIdが直線状にああるか否か.
        /// </summary>
        /// <param name="threshold">閾値, 1に近いほど判定が厳しい.</param>
        /// <param name="boneIds">BoneIdの配列.</param>
        /// <returns></returns>
        bool IsStraight(
            float threshold,
            params OVRSkeleton.BoneId[] boneIds)
        {
            // 3未満の要素は計算に入れない.
            if (boneIds.Length < 3) return false;

            Vector3? lastVec = null;
            var dot = 1.0f;
            for (var i = 0; i < boneIds.Length - 1; ++i)
            {
                Vector3 v = (handSkeleton.Bones[(int)boneIds[i + 1]].Transform.position - handSkeleton.Bones[(int)boneIds[i]].Transform.position).normalized;
                if (lastVec.HasValue)
                {
                    // 内積の値を総乗する.
                    dot *= Vector3.Dot(v, lastVec.Value);
                }
                lastVec = v;
            }
            return threshold <= dot;
        }


        /// <summary>
        /// 指定したFingerIdの指がまっすぐになっているか判定.
        /// </summary>
        /// <param name="threshold">閾値, 1に近いほど判定が厳しい.</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsFingerStraight(
            float threshold,
            FingerId id)
        {
            bool ret = false;
            switch (id)
            {
                case FingerId.Thumb: ret = IsStraight(threshold, thumb); break;
                case FingerId.Index: ret = IsStraight(threshold, index); break;
                case FingerId.Middle: ret = IsStraight(threshold, middle); break;
                case FingerId.Ring: ret = IsStraight(threshold, ring); break;
                case FingerId.Pinky: ret = IsStraight(threshold, pinky); break;
            }
            return ret;
        }

    }
#endif
}