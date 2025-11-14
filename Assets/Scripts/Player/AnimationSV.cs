using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSV : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Animator animator;

    private bool IsShooting;

    private bool IsWalking;
    private bool OldWalking;

    public void FireShootTrigger()
    {
        if (!photonView.IsMine) return;

        IsShooting = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            IsWalking = animator.GetBool("IsWalking");

            if (!HasValuesToSend()) return;

            stream.SendNext(IsWalking);
            stream.SendNext(IsShooting);

            OldWalking = IsWalking;
            IsShooting = false;
        }
        else
        {
            OldWalking = (bool)stream.ReceiveNext();
            animator.SetBool("IsWalking", OldWalking);

            bool receivedTrigger = (bool)stream.ReceiveNext();

            if (receivedTrigger)
            {
                animator.SetTrigger("IsShooting");
            }
        }
    }

    private bool HasValuesToSend()
    {
        if (IsShooting && OldWalking != IsWalking) return true;

        return false;
    }
}
