using UnityEngine;

using Leap;
using Leap.Unity;


namespace HandGestureRecord.GestureInput
{
    public class LeapMotionHandData : HandDataBase
    {

        HandId handId;
        LeapServiceProvider provider;
        Hand hand;
        
        // 各指のデータ.
        Finger thumb;
        Finger index;
        Finger middle;
        Finger ring;
        Finger pinky;
        
        public LeapMotionHandData(
            HandId argHandId,
            LeapServiceProvider argProvider)
        {
            handId = argHandId;
            provider = argProvider;
        }


        // とりあえずの更新処理.
        public void Update()
        {
            hand = null;
            foreach (var frameHand in provider.CurrentFrame.Hands)
            {
                if (frameHand.IsLeft && handId == HandId.LeftHand)
                    hand = frameHand;
                else if (frameHand.IsRight && handId == HandId.RightHand)
                    hand = frameHand;
            }
            if (hand == null) return;
            
            // 各指のデータを取得.
            foreach (var finger in hand.Fingers)
            {
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_THUMB: thumb = finger; break;
                    case Finger.FingerType.TYPE_INDEX: index = finger; break;
                    case Finger.FingerType.TYPE_MIDDLE: middle = finger; break;
                    case Finger.FingerType.TYPE_RING: ring = finger; break;
                    case Finger.FingerType.TYPE_PINKY: pinky = finger; break;
                }
            }
        }


        /// <summary>
        /// 指定した指のIdからVector3の配列を取得.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override Vector3[] CreatePositionFingerPositionArray(
            FingerId id)
        {
            if (hand == null) return null;

            Finger sourceFinger = null;
            switch (id)
            {
                case FingerId.Thumb: sourceFinger = thumb; break;
                case FingerId.Index: sourceFinger = index; break;
                case FingerId.Middle: sourceFinger = middle; break;
                case FingerId.Ring: sourceFinger = ring; break;
                case FingerId.Pinky: sourceFinger = pinky; break;
            }
            if (sourceFinger == null) return null;

            Vector3[] ret = new Vector3[sourceFinger.bones.Length];
            if (id == FingerId.Thumb)
            {
                // 親指の時は付け根を手首の座標に挿げ替える.
                ret[0] = hand.WristPosition.ToVector3();
                for (var i = 1; i < sourceFinger.bones.Length; ++i)
                {
                    ret[i] = sourceFinger.bones[i].Basis.translation.ToVector3();
                }
            }
            else
            {
                for (var i = 0; i < ret.Length; ++i)
                {
                    ret[i] = sourceFinger.bones[i].Basis.translation.ToVector3();
                }
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
            this.Update();
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
            this.Update();
            return this.DotByFingerDirection(this.CreatePositionFingerPositionArray(id));            
        }

        
        /// <summary>
        /// 指の直線の比率をまとめたデータの取得.
        /// </summary>
        /// <returns></returns>
        public override FingerStraightInfo GetFingerStraightInfo(
            float threshold)
        {
            this.Update();
            return new FingerStraightInfo
            {
                thumb = this.IsFingerStraight(threshold, FingerId.Thumb),
                index = this.IsFingerStraight(threshold,FingerId.Index),
                middle = this.IsFingerStraight(threshold,FingerId.Middle),
                ring = this.IsFingerStraight(threshold,FingerId.Ring),
                pinky = this.IsFingerStraight(threshold,FingerId.Pinky)
            };
        }

    }
}