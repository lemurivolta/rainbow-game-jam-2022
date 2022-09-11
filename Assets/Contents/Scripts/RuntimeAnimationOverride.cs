using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RuntimeAnimationOverride : MonoBehaviour
{
    [System.Serializable]
    public class AnimationOverride
    {
        public AnimationClip OriginalClip;
        public AnimationClip OverrideClip;
    }

    public List<AnimationOverride> AnimationOverrides;

    // Start is called before the first frame update
    void Start()
    {
        // create an override only if there's at least on override clip
        if(AnimationOverrides != null &&
            AnimationOverrides.Find(ao => ao.OverrideClip != null) != null)
        {
            // switch the current animator with an override controller
            var animator = GetComponent<Animator>();
            var baseController = animator.runtimeAnimatorController;
            var overrideController = new AnimatorOverrideController();
            overrideController.runtimeAnimatorController = baseController;
            animator.runtimeAnimatorController = overrideController;
            // map the clips
            foreach(var animationOverride in AnimationOverrides) {
                if (animationOverride.OverrideClip != null)
                {
                    overrideController[animationOverride.OriginalClip] = animationOverride.OverrideClip;
                }
            }
        }
    }

    private void OnValidate()
    {
        var animator = GetComponent<Animator>();
        var baseController = animator.runtimeAnimatorController;
        if(AnimationOverrides == null)
        {
            AnimationOverrides = new();
        }
        // add override clips for each original one
        foreach(var clip in baseController.animationClips)
        {
            bool found = false;
            foreach(var animationOverride in AnimationOverrides)
            {
                if(animationOverride.OriginalClip == clip)
                {
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                AnimationOverrides.Add(new AnimationOverride()
                {
                    OriginalClip = clip,
                    OverrideClip = null
                });
            }
        }
    }

}
