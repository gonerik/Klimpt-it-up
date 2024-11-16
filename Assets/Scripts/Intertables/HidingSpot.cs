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
            _currentSprite.sprite = hidingSpotSpritePainting;
        }
        // Start is called before the first frame update
        public override void Interact()
        {
            base.Interact();
            if (CharacterController2D.Instance.GetIsHoldingPainting() && !_containsPainting)
            {
                _containsPainting = true;
                _painting = CharacterController2D.Instance.currentPickup;
                CharacterController2D.Instance.SetIsHoldingPainting(false);
                CharacterController2D.Instance.currentPickup = null;
                _currentSprite.sprite = hidingSpotSpritePainting;
            }
            else if (!CharacterController2D.Instance.GetIsHoldingPainting() && _containsPainting)
            {
                _containsPainting = false;
                CharacterController2D.Instance.currentPickup = _painting;
                CharacterController2D.Instance.SetIsHoldingPainting(true);
                _painting = null;
                _currentSprite.sprite = hidingSpotSpriteEmpty;
            }
        }
    }
}
