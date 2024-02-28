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
        /// <param name="differenceAlpha">A 0-1 alpha value that represents the accuracy of a note hit. 0 being getting the best score and 1 the lowest.</param>
        /// <returns>The decided HitStatus based on the accuracy</returns>
        public static HitStatus CalculateHitStatus(float differenceAlpha)
        {
            if (differenceAlpha > SessionValuesShortcut.Instance._okThreshold)
                return HitStatus.Ok;
            else if (differenceAlpha > SessionValuesShortcut.Instance._alrightThreshold)
                return HitStatus.Alright;
            else if (differenceAlpha > SessionValuesShortcut.Instance._niceThreshold)
                return HitStatus.Nice;
            else
                return HitStatus.Perfect;
        }
    }
}