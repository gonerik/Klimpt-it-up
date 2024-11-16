using System;
using UnityEngine;

namespace Intertables
{
    public class HidingSpot : Interactable
    {
        [SerializeField]private Sprite hidingSpotSpriteEmpty;
        [SerializeField]private Sprite hidingSpotSpritePainting;
        private SpriteRenderer _currentSprite;
        private bool _containsPainting = false;

        private PickUpObjects _painting = null;

        void Start()
        {
            _currentSprite = GetComponent<SpriteRenderer>();
            _currentSprite.sprite = hidingSpotSpriteEmpty;
        }
        // Start is called before the first frame update
        public override void Interact()
        {
            if (CharacterController2D.Instance.GetIsHoldingPainting() && !_containsPainting)
            {
                _containsPainting = true;
                _painting = CharacterController2D.Instance.currentPickup;
                _painting.transform.localScale = new Vector3(0, 0, 0);
                CharacterController2D.Instance.SetIsHoldingPainting(false);
                CharacterController2D.Instance.currentPickup = null;
                CharacterController2D.Instance.settoMaxSpeed();
                _currentSprite.sprite = hidingSpotSpritePainting;
            }
            else if (!CharacterController2D.Instance.GetIsHoldingPainting() && _containsPainting)
            {
                _containsPainting = false;
                _painting.transform.localScale = new Vector3(1, 1, 1);
                CharacterController2D.Instance.currentPickup = _painting;
                CharacterController2D.Instance.SetIsHoldingPainting(true);
                CharacterController2D.Instance.settoMinSpeed();
                _painting = null;
                _currentSprite.sprite = hidingSpotSpriteEmpty;
            }
        }
    }
}
