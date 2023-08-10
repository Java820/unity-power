using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Sprite playerSprite;
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] Joystick movementJoystick;
    [SerializeField] float movementSpeed = 60;
    float horizontal;
    float vertical;
    float walkingAnimFloat;

    Animator playerAnim;
    Rigidbody2D body;

    public int skinNr;

    public Skins[] skins;
    SpriteRenderer spriteRenderer;

    public int spriteNr;



    void Awake()
    {
        playerAnim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementJoystick.gameObject.activeSelf)
        {
            horizontal = movementJoystick.Horizontal;
            vertical = movementJoystick.Vertical;
            walkingAnimFloat = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            if (movementJoystick.Horizontal > 0 || movementJoystick.Horizontal < 0 || movementJoystick.Vertical > 0 || movementJoystick.Vertical < 0)
            {
                playerAnim.SetBool("isWalking", true);
                playerAnim.SetFloat("walkingFloat", walkingAnimFloat);
            }
            else
            {
                playerAnim.SetBool("isWalking", false);
            }

            if (walkingAnimFloat > 0.8)
            {
                if (!movementParticle.isPlaying)
                {
                    movementParticle.Play();
                }
            }
            else
            {
                if (movementParticle.isPlaying)
                {
                    movementParticle.Stop();
                }
            }

            if (movementJoystick.Vertical > 0.2)
            {
                playerAnim.SetBool("isBack", true);
            }
            else if (movementJoystick.Vertical < -0.2)
            {
                playerAnim.SetBool("isBack", false);
            }
        }
        else
        {
            horizontal = 0;
            vertical = 0;
        }

        if (skinNr > skins.Length - 1) skinNr = 0;
        else if (skinNr < 0) skinNr = skins.Length - 1;

    }
    void FixedUpdate()
    {
        body.velocity += new Vector2(horizontal * (movementSpeed), vertical * (movementSpeed));

    }

    public void SetupSkin(string id)
    {
        string[] idString = id.Split(';');
        skinNr = int.Parse(idString[1]);
    }

    void LateUpdate()
    {
        SkinChoice();
    }

    void SkinChoice()
    {
        if (spriteRenderer.sprite.name.Contains("Test Walk"))
        {
            string spriteName = spriteRenderer.sprite.name;
            spriteName = spriteName.Replace("Test Walk_", "");
            spriteNr = int.Parse(spriteName);

            spriteRenderer.sprite = skins[skinNr].sprites[spriteNr];
        }
    }

}

[System.Serializable]
public struct Skins
{
    public Sprite[] sprites;
}