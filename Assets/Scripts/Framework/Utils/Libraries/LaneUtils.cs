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
            float threshold = Mathf.Abs((lerpAlpha - .5f) / SessionValues.lerpAlphaHitThreshold);

            if (threshold > 1)
                return HitStatus.Miss;
            else if (threshold > SessionValuesEditor.Instance._okThreshold)
                return HitStatus.Ok;
            else if (threshold > SessionValuesEditor.Instance._alrightThreshold)
                return HitStatus.Alright;
            else if (threshold > SessionValuesEditor.Instance._niceThreshold)
                return HitStatus.Nice;
            else
                return HitStatus.Perfect;
        }
    }
}