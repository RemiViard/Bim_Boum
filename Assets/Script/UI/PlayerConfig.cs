using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConfig : MonoBehaviour
{
    [SerializeField] int Id;
    [SerializeField] TMPro.TMP_Dropdown dropdown;
    [SerializeField] Sprite sprite;
    [SerializeField] public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Config(List<TMPro.TMP_Dropdown.OptionData> _optionDatas)
    {
        dropdown.AddOptions(_optionDatas);
    }
    public int getDropdownIndex()
    {
        return dropdown.value;
    }
    public string getOption()
    {
        return dropdown.options[dropdown.value].text;
    }
    public void ChangeSprite()
    {

    }
    
}
