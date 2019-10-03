using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    AudioSource source = null;
    [SerializeField] AudioClip hover = null;
    [SerializeField] AudioClip click = null;
    Button button = null;
    Slider slider = null;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<AudioSource>())
            source = GetComponent<AudioSource>();
        if (GetComponent<Button>())
        {
            button = GetComponentInParent<Button>();
            button.onClick.AddListener(Clicked);
        }
        else if(GetComponent<Slider>())
        {
            slider = GetComponent<Slider>();
        }
        hover = Resources.Load<AudioClip>("SFX/hover");
        click = Resources.Load<AudioClip>("SFX/click");
    }

    private void Clicked()
    {
        if(source != null && source.isActiveAndEnabled)
            source.PlayOneShot(click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button != null)
            EventSystem.current.SetSelectedGameObject(button.gameObject, eventData);
        else if (slider != null)
            EventSystem.current.SetSelectedGameObject(slider.gameObject, eventData);
        source.PlayOneShot(hover);
    }
}
