using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color originalColor;
    private Color hoverColor = new Color(1.0f, 0.5f, 0.0f, 1.0f); // ÁÖÈ²»ö

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = originalColor;
    }
}
