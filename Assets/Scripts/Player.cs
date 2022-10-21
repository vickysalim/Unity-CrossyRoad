using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;

    [SerializeField] AudioSource collisionAudio;
    [SerializeField] AudioSource walkAudio;

    private float moveDuration = 0.2f;
    private float jumpHeight = 0.2f;

    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;

    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }

    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel; }

    public bool IsDie { get => !this.enabled; }

    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }

    private void Update()
    {
        var moveDir = Vector3.zero;

        // keycode (bisa hold button utk ttp gerak); keycodedown (mesti di klik berulang-ulang)

        if(Input.GetKey(KeyCode.UpArrow))
        {
            moveDir += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDir += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir += new Vector3(-1, 0, 0);
        }

        if (moveDir == Vector3.zero)
        {
            return;
        }

        if(IsJumping() == false)
        {
            Jump(moveDir);
        }
    }

    private void Jump(Vector3 targetDirection)
    {
        var targetPosition = transform.position + targetDirection;

        transform.LookAt(targetPosition);

        var moveSeq = DOTween.Sequence(transform);

        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration / 2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration / 2));

        if (targetPosition.z <= backBoundary || targetPosition.x <= leftBoundary || targetPosition.x >= rightBoundary)
            return;

        if (Tree.AllPositions.Contains(targetPosition))
            return;
        if (Rock.AllPositions.Contains(targetPosition))
            return;

        transform.DOMoveX(targetPosition.x, moveDuration);
        transform.DOMoveZ(targetPosition.z, moveDuration).OnComplete(UpdateTravel);

        walkAudio.Play(0);
    }

    private void UpdateTravel()
    {
        currentTravel = (int) this.transform.position.z;

        if (currentTravel > maxTravel)
            maxTravel = currentTravel;

        stepText.text = maxTravel.ToString();
    }

    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            AnimateDie();
        }
    }

    private void AnimateDie()
    {
        collisionAudio.Play(0);

        // supaya ayam tidak floating
        transform.DOMoveY(-0.325f, 0);

        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(2, 0.1f);
        transform.DOScaleZ(1, 0.1f);

        this.enabled = false;
    }
}
