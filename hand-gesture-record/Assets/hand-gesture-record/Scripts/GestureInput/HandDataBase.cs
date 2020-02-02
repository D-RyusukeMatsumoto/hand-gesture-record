using UnityEngine;


namespace HandGestureRecord.GestureInput
{
    public abstract class HandDataBase
    {
        
        
        public float DotByFingerDirection(
            params Vector3[] positions)
        {
            if (positions == null) return 0f;
            
            Vector3? lastVec = null;
            var dot = 1.0f;
            for (var i = 0; i < positions.Length - 1; ++i)
            {
                Vector3 v = (positions[i + 1] - positions[i]).normalized;
                if (lastVec.HasValue)
                {
                    dot *= Vector3.Dot(v, lastVec.Value);
                }
                lastVec = v;
            }
            return dot;
        }

        protected abstract Vector3[] CreatePositionFingerPositionArray(FingerId id);
        public abstract bool IsFingerStraight(float threshold, FingerId id);
        public abstract float GetDotByFinger(FingerId id);
    }
}