using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChanger : MonoBehaviour
{
    [SerializeField]private CharacterTemplate template;
    public void OnPunch(GameObject player){
        Playermove move = player.GetComponent<Playermove>();
        player.GetComponent<SpriteRenderer>().sprite = template.sprite;
        move.dash.textureSheetAnimation.RemoveSprite(0);
        move.dash.textureSheetAnimation.AddSprite(template.sprite);
        move.dash.transform.localScale = new Vector3(template.dashParticleSize, template.dashParticleSize);
        player.GetComponent<BoxCollider2D>().size = template.hitBox;
        move.colliderOffset = template.rayCastOffset;

        move.howLongYouAreStunned = template.stunTime;
        move.staticStunTime = template.stunTime;
        move.jumpHeight = template.jumpHeight;
        move.dashSpeed = template.dashAmount;
        player.GetComponent<Punch>().overallPower = template.punchAmount;
        player.GetComponent<Punch>().punchRadius = template.punchRadius;
    }
}
