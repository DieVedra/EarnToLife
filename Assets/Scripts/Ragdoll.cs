using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ragdoll : MonoBehaviour
{
    private const float MIN_SPRITE_ALPHA = 0.05f;
    protected float _timeRagdollActive = 3f;
    protected List<SpriteRenderer> _spriteRenderersToFade;

    public event Action OnDestroy;
    public virtual void ActiveRagdoll() { }

    protected virtual void InitSpritesToFade() { }
    protected IEnumerator FadeCorutine()
    {
        yield return new WaitForSeconds(_timeRagdollActive);
        Color color;
        while (_spriteRenderersToFade[_spriteRenderersToFade.Count - 1].color.a > MIN_SPRITE_ALPHA)
        {
            for (int i = 0; i < _spriteRenderersToFade.Count; i++)
            {
                color = _spriteRenderersToFade[i].color;
                color.a -= Time.deltaTime;
                _spriteRenderersToFade[i].color = color;
            }
            yield return null;
        }
        OnDestroy?.Invoke();
    }

}
