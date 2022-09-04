using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public AnimationClip AnimationClip;

    // Start is called before the first frame update
    void Start()
    {
        if (AnimationClip != null)
        {
            // switch the current animator with an override with our animation
            var animator = GetComponent<Animator>();
            RuntimeAnimatorController baseController = animator.runtimeAnimatorController;
            var overrideController = new AnimatorOverrideController();
            overrideController.runtimeAnimatorController = baseController;
            animator.runtimeAnimatorController = overrideController;
            var originalClip = overrideController.runtimeAnimatorController.animationClips[0];
            overrideController[originalClip] = AnimationClip;
        }
    }
}
