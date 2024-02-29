using UnityEngine;

using ProefExamen.Framework.Gameplay.LaneSystem;
using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.Utils.Libraries.LaneUtils
{
    /// <summary>
    /// A class holding LaneSystem calculations.
    /// </summary>
    public static class LaneUtils
    {
        /// <summary>
        /// Returns a HitStatus enum that can be cast to an int for a score.
        /// </summary>
        /// <param name="lerpAlpha">The current lerpAlpha from the note that is being hit, 
        /// will be used to calculate if the note falls in the threshold of being hit.</param>
        /// <returns>The decided HitStatus based on the accuracy.</returns>
        public static HitStatus CalculateHitStatus(float lerpAlpha)
        {
            float threshold = Mathf.Abs((lerpAlpha - .5f) / SessionValues.Instance.lerpAlphaHitThreshold);

            if (threshold > 1)
                return HitStatus.MissClick;
            else if (threshold > SessionValues.Instance.okThreshold)
                return HitStatus.Ok;
            else if (threshold > SessionValues.Instance.alrightThreshold)
                return HitStatus.Alright;
            else if (threshold > SessionValues.Instance.niceThreshold)
                return HitStatus.Nice;
            else
                return HitStatus.Perfect;
        }
    }
}