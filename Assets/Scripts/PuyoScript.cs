using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoScript : MonoBehaviour
{
    [SerializeField]
    private int posX;
    [SerializeField]
    private int posY;
    [SerializeField]
    private float verticalSpeed;
    [SerializeField]
    private Color red;
    [SerializeField]
    private Color green;
    [SerializeField]
    private Color blue;
    [SerializeField]
    private Color yellow;
    [SerializeField]
    private PuyoScript puyoScriptOwn;
    [SerializeField]
    private Material redMaterial;
    [SerializeField]
    private Material blueMaterial;
    [SerializeField]
    private Material greenMaterial;
    [SerializeField]
    private Material yellowMaterial;
    [SerializeField]
    private Sprite redSprite;
    [SerializeField]
    private Sprite blueSprite;
    [SerializeField]
    private Sprite greenSprite;
    [SerializeField]
    private Sprite yellowSprite;

    private bool isFree = false;
    public bool willFall = true;
    private bool isLocked = false;
    private float futureVerticalPosition = 0;
    //0 = red, 1 = green, 2= blue, 3 = yellow
    private int colorSelected;
    private SpriteRenderer spriteComponent;

    private void Awake()
    {
        GridManager.instance.PuyoUnlocked();
    }
    void Start()
    {
        
        spriteComponent = GetComponent<SpriteRenderer>();
        colorSelected = Random.Range(0, 4);
        if (colorSelected == 0)
        {
            spriteComponent.material = redMaterial;
            spriteComponent.sprite = redSprite;
            GetComponentInChildren<ParticleSystemRenderer>().material = redMaterial;
        }
        else if (colorSelected == 1)
        {
            spriteComponent.material = greenMaterial;
            spriteComponent.sprite = greenSprite;
            GetComponentInChildren<ParticleSystemRenderer>().material = greenMaterial;
        }
        else if (colorSelected == 2)
        {
            spriteComponent.material = blueMaterial;
            spriteComponent.sprite = blueSprite;
            GetComponentInChildren<ParticleSystemRenderer>().material = blueMaterial;
        }
        else if (colorSelected == 3)
        {
            spriteComponent.material = yellowMaterial;
            spriteComponent.sprite = yellowSprite;
            GetComponentInChildren<ParticleSystemRenderer>().material = yellowMaterial;
        }
    }



    void Update()
    {
        if (GridManager.instance.GetGameStarted())
        {
            Fall();
        }
    }

    public int GetColor()
    {
        return colorSelected;
    }

    public void Fall()
    {
        if (isFree && !isLocked)
        {
            if (transform.position.y > futureVerticalPosition)
            {
                transform.Translate(Vector3.down * Time.deltaTime * verticalSpeed, Space.World);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, futureVerticalPosition, transform.position.z);
                LockIn();
            }
        }
    }

    private void SetNewAdvanceTarget(int fallMultiplyer)
    {
        futureVerticalPosition = transform.position.y - (.5f * fallMultiplyer);
        //UpdatePosY(fallMultiplyer);
        GridManager.instance.LinkPuyoToGrid(posX, posY, this);
    }

    public void SetFutureVerticalPosition(int futurePositionMultiplyer)
    {
        futureVerticalPosition = transform.position.y - (.5f * futurePositionMultiplyer);
    }

    public void Settle()
    {
        if (willFall)
        {
            isFree = true;
            int fallMultiplyer = CellToFallTo();
            //Debug.Log(fallMultiplyer);
            fallMultiplyer = fallMultiplyer - posY;
            if (fallMultiplyer < 0)
            {
                fallMultiplyer = 0;
            }
            UpdatePosY(fallMultiplyer);
            //Debug.Log(fallMultiplyer);
            if (fallMultiplyer > 0)
            {
                SetNewAdvanceTarget(fallMultiplyer);
            }
            else
            {
                GridManager.instance.LinkPuyoToGrid(posX, posY, this);
                LockIn();
            }
        }
    }

    public void Unlock()
    {
        if (CanMoveVertical())
        {
            GridManager.instance.UnlinkPuyoFromGrid(posX, posY);
            willFall = true;
            isLocked = false;
            GridManager.instance.PuyoUnlocked();
        }
    }

    public void LockIn()
    {
        SFXManager.instance.PlayClip(1);
        willFall = false;
        isLocked = true;
        GridManager.instance.PuyoLocked();
        GridManager.instance.SettlePuyo(posX, posY, puyoScriptOwn);
    }

    public int GetPosX()
    {
        return posX;
    }

    public int GetPosY()
    {
        return posY;
    }

    public bool CanMoveHorizontal(int direction)
    {
        return GridManager.instance.CanMoveHorizontal(posX + direction, posY);
    }

    public bool CanMoveVertical()
    {
        return GridManager.instance.CanMoveVertical(posY + 1, posX);
    }

    public int CellToFallTo()
    {
        return GridManager.instance.CellToFallTo(posY, posX);
    }

    public bool CanRotateLeft(int directionX, int directionY)
    {
        return GridManager.instance.CanRotateLeft(posX + directionX, posY + directionY);
    }

    public bool CanRotateRight(int directionX, int directionY)
    {
        return GridManager.instance.CanRotateRight(posX + directionX, posY + directionY);
    }

    public void InitializePuyo(int initialPosX, int initialPosY)
    {
        posX = initialPosX;
        posY = initialPosY;
    }

    public void UpdatePosX(int steps)
    {
        posX += steps;
    }

    public void UpdatePosY(int steps)
    {
        posY += steps;
    }

    public void Die()
    {
        spriteComponent.sprite = null;
        GetComponentInChildren<ParticleSystem>().Play();
        StartCoroutine(WaitForDeath());
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(.55f);
        Destroy(gameObject);
    }
}
