using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSpriteChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;

    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>();

        if (buttonImage == null)
        {
            Debug.LogError("No Image component found on " + gameObject.name);
            return;
        }

        // Set default sprite
        buttonImage.sprite = defaultSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = defaultSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.sprite = clickSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }
}
