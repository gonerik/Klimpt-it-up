using System.Collections;
using System.Collections.Generic;
using Intertables;
using Unity.VisualScripting;
using UnityEngine;

public class PaintingFinal : Interactable
{
   private ParticleSystem[] particleSystem;
   public Sprite Sprite;
   [TextArea] public string Title;
   [TextArea] public string Description;
   private static bool isOn;
   private void Start()
   {
      Storage.addPainting();
      particleSystem = GetComponentsInChildren<ParticleSystem>();
   }
   public override void Interact()
   {
      if(isOn)
      {
         CharacterController2D.Instance.setCanMove(true);
         
      }
      else
      {
         base.Interact();
         FinalCanva.Instance.Display(this);
         CharacterController2D.Instance.setCanMove(false);
      }

      isOn = !isOn;
      FinalCanva.Instance.PlayAnimation();
   }
   
}
