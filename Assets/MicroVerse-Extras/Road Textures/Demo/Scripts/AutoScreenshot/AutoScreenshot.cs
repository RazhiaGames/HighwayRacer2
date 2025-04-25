#if USING_PHOTOSESSION
using JBooth.MicroVerseCore;
using UnityEngine;

namespace Rowlan.AutoScreenshot
{
    /// <summary>
    /// Automatically create screenshots using PhotoSession while performing an editor-task in a loop (change camera, switch material, etc).
    /// 
    /// This is proprietary to MicroVerse Roads, but can be used in other cases.
    /// 
    /// Usage:
    /// 
    ///     * install PhotoSession
    ///     * add USING_PHOTOSESSION scripting define symbol
    ///     * add this script to an empty gameobject
    ///     * assign parameters, ie photosession (set resolution, eg 4K) and the road system
    ///     * hit the action button
    ///     => the action will be performed, ie in this case materials assigned in a loop and screenshots taken
    ///     
    /// </summary>
    public class AutoScreenshot : MonoBehaviour
    {
        public PhotoSession.PhotoSession photoSession;
        public RoadSystem roadSystem;
    }
}
#endif