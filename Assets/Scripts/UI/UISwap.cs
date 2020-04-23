using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISwap : MonoBehaviour
{
    [SerializeField] private Image[] _buttonsImg = null;
    [SerializeField] private Image[] _backgroundsImg = null;
    [SerializeField] private Image[] _slidersHandleImg = null;
    [SerializeField] private Image[] _slidersFillImg = null;
    [SerializeField] private Image[] _slidersBackgroundImg = null;
    [SerializeField] private TextMeshProUGUI[] _textMeshProFonts = null;

    [SerializeField] private Sprite[] _buttonsSprite = null;
    [SerializeField] private Sprite[] _backgroudsSprite = null;
    [SerializeField] private Sprite[] _sliderHandleSprite = null;
    [SerializeField] private Sprite[] _sliderFillSprite = null;
    [SerializeField] private Sprite[] _sliderBackgroundSprite = null;
    [SerializeField] private TMP_FontAsset[] _fonts = null;

    private void Start()
    {
        PlayerController playerController = PlayerManager.Instance.Player;

        GameLoopManager.Instance.Pause += IsPaused;

        if(playerController != null)
        {
            if(playerController.WorldTaged == PlayerManager.WorldTag.VERTUMNE)
            {
                for (int i = 0; i < _buttonsImg.Length; i++)
                {
                    if(_buttonsImg[i] != null)
                    {
                        _buttonsImg[i].sprite = _buttonsSprite[0];
                    }
                }

                for (int i = 0; i < _backgroundsImg.Length; i++)
                {
                    if(_backgroundsImg[i] != null)
                    {
                        _backgroundsImg[i].sprite = _backgroudsSprite[0];
                    }
                }

                for (int i = 0; i < _slidersHandleImg.Length; i++)
                {
                    if(_slidersHandleImg[i] != null)
                    {
                        _slidersHandleImg[i].sprite = _sliderHandleSprite[0];
                    }
                }

                for (int i = 0; i < _slidersBackgroundImg.Length; i++)
                {
                    if (_slidersBackgroundImg[i] != null)
                    {
                        _slidersBackgroundImg[i].sprite = _sliderBackgroundSprite[0];
                    }
                }

                for (int i = 0; i < _slidersFillImg.Length; i++)
                {
                    if (_slidersFillImg[i] != null)
                    {
                        _slidersFillImg[i].sprite = _sliderFillSprite[0];
                    }
                }

                for (int i = 0; i < _textMeshProFonts.Length; i++)
                {
                    if(_textMeshProFonts[i] != null)
                    {
                        _textMeshProFonts[i].font = _fonts[0];
                    }
                }
            }
            else if(playerController.WorldTaged == PlayerManager.WorldTag.GCF)
            {
                for (int i = 0; i < _buttonsImg.Length; i++)
                {
                    if (_buttonsImg[i] != null)
                    {
                        _buttonsImg[i].sprite = _buttonsSprite[1];
                    }
                }

                for (int i = 0; i < _backgroundsImg.Length; i++)
                {
                    if (_backgroundsImg[i] != null)
                    {
                        _backgroundsImg[i].sprite = _backgroudsSprite[1];
                    }
                }

                for (int i = 0; i < _slidersHandleImg.Length; i++)
                {
                    if (_slidersHandleImg[i] != null)
                    {
                        _slidersHandleImg[i].sprite = _sliderHandleSprite[1];
                    }
                }

                for (int i = 0; i < _slidersBackgroundImg.Length; i++)
                {
                    if (_slidersBackgroundImg[i] != null)
                    {
                        _slidersBackgroundImg[i].sprite = _sliderBackgroundSprite[1];
                    }
                }

                for (int i = 0; i < _slidersFillImg.Length; i++)
                {
                    if (_slidersFillImg[i] != null)
                    {
                        _slidersFillImg[i].sprite = _sliderFillSprite[1];
                    }
                }

                for (int i = 0; i < _textMeshProFonts.Length; i++)
                {
                    if (_textMeshProFonts[i] != null)
                    {
                        _textMeshProFonts[i].font = _fonts[1];
                    }
                }
            }
        }
    }

    private void IsPaused(bool value)
    {
        PlayerController playerController = PlayerManager.Instance.Player;

        if (value == true)
        {
            if (playerController != null)
            {
                if (playerController.WorldTaged == PlayerManager.WorldTag.VERTUMNE)
                {
                    for (int i = 0; i < _buttonsImg.Length; i++)
                    {
                        if (_buttonsImg[i] != null)
                        {
                            _buttonsImg[i].sprite = _buttonsSprite[0];
                        }
                    }

                    for (int i = 0; i < _backgroundsImg.Length; i++)
                    {
                        if (_backgroundsImg[i] != null)
                        {
                            _backgroundsImg[i].sprite = _backgroudsSprite[0];
                        }
                    }

                    for (int i = 0; i < _slidersHandleImg.Length; i++)
                    {
                        if (_slidersHandleImg[i] != null)
                        {
                            _slidersHandleImg[i].sprite = _sliderHandleSprite[0];
                        }
                    }

                    for (int i = 0; i < _slidersBackgroundImg.Length; i++)
                    {
                        if (_slidersBackgroundImg[i] != null)
                        {
                            _slidersBackgroundImg[i].sprite = _sliderBackgroundSprite[0];
                        }
                    }

                    for (int i = 0; i < _slidersFillImg.Length; i++)
                    {
                        if (_slidersFillImg[i] != null)
                        {
                            _slidersFillImg[i].sprite = _sliderFillSprite[0];
                        }
                    }

                    for (int i = 0; i < _textMeshProFonts.Length; i++)
                    {
                        if (_textMeshProFonts[i] != null)
                        {
                            _textMeshProFonts[i].font = _fonts[0];
                        }
                    }
                }
                else if (playerController.WorldTaged == PlayerManager.WorldTag.GCF)
                {
                    for (int i = 0; i < _buttonsImg.Length; i++)
                    {
                        if (_buttonsImg[i] != null)
                        {
                            _buttonsImg[i].sprite = _buttonsSprite[1];
                        }
                    }

                    for (int i = 0; i < _backgroundsImg.Length; i++)
                    {
                        if (_backgroundsImg[i] != null)
                        {
                            _backgroundsImg[i].sprite = _backgroudsSprite[1];
                        }
                    }

                    for (int i = 0; i < _slidersHandleImg.Length; i++)
                    {
                        if (_slidersHandleImg[i] != null)
                        {
                            _slidersHandleImg[i].sprite = _sliderHandleSprite[1];
                        }
                    }

                    for (int i = 0; i < _slidersBackgroundImg.Length; i++)
                    {
                        if (_slidersBackgroundImg[i] != null)
                        {
                            _slidersBackgroundImg[i].sprite = _sliderBackgroundSprite[1];
                        }
                    }

                    for (int i = 0; i < _slidersFillImg.Length; i++)
                    {
                        if (_slidersFillImg[i] != null)
                        {
                            _slidersFillImg[i].sprite = _sliderFillSprite[1];
                        }
                    }

                    for (int i = 0; i < _textMeshProFonts.Length; i++)
                    {
                        if (_textMeshProFonts[i] != null)
                        {
                            _textMeshProFonts[i].font = _fonts[1];
                        }
                    }
                }
            }
        }
    }
}
