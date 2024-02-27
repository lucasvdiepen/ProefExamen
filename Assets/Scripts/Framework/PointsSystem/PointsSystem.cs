using System;

namespace ProefExamen.Framework.PointsSystem
{
    /// <summary>
    /// A class responsible for keeping track of the points of the player.
    /// </summary>
    public static class PointsSystem
    {
        /// <summary>
        /// An event that is invoked when the points have changed.
        /// </summary>
        public static Action<int> OnPointsChanged;

        private static int _points;

        /// <summary>
        /// The current points of the player.
        /// </summary>
        public static int Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPointsChanged?.Invoke(_points);
            }
        }
    }
}