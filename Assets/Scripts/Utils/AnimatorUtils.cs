using UnityEngine;

namespace Utils
{
    public static class AnimatorUtils
    {
        public static bool HasParameter(string paramName, Animator animator)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                    return true;
            }
            return false;
        }
    }
}