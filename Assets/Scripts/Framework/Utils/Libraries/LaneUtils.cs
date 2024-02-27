using ProefExamen.Framework.Gameplay.LaneSystem;

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
        public static HitStatus ReturnHitStatus(float differenceAlpha)
        {
            if (differenceAlpha > .75f)
                return HitStatus.OK;
            else if (differenceAlpha > .5f)
                return HitStatus.ALRIGHT;
            else if (differenceAlpha > .25f)
                return HitStatus.NICE;
            else
                return HitStatus.PERFECT;
        }
    }
}