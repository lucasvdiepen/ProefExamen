using ProefExamen.Framework.Gameplay.LaneSystem;
using Unity.VisualScripting;

namespace ProefExamen.Framework.Utils.Libraries.LaneUtils
{
    public static class LaneUtils
    {
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