using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
using System.Collections.Generic;

public class Footprint : MonoBehaviour
{
    [Header("消失效果")]
    [SerializeField] private float fadeOutDuration = 2f; // 淡出持续时间
    
    private float lifetime;
    private float currentTime = 0f;
    private bool isFading = false;
    private Collider footprintCollider;
    private Renderer footprintRenderer;
    private List<VisualEffect> visualEffects = new List<VisualEffect>(); // 存储所有VFX组件
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>(); // 存储所有粒子系统组件
    
    public void Initialize(float lifetime)
    {
        this.lifetime = lifetime;
        currentTime = 0f;
        isFading = false;
        
        footprintCollider = GetComponent<Collider>();
        footprintRenderer = GetComponent<Renderer>();
        
        
        // 如果有关联的Renderer，确保初始透明度为1
        if (footprintRenderer != null && footprintRenderer.material != null)
        {
            Color color = footprintRenderer.material.color;
            color.a = 1f;
            footprintRenderer.material.color = color;
        }
    }

    private void Update()
    {
        if (lifetime <= 0) return;

        currentTime += Time.deltaTime;

        // 开始淡出
        if (!isFading && currentTime >= lifetime - fadeOutDuration)
        {
            StartFadeOut();
        }

        // 完全消失后销毁
        if (currentTime >= lifetime)
        {
            DestroyFootprint();
        }
    }

    private void StartFadeOut()
    {
        isFading = true;
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsed = 0f;
        float startAlpha = 1f;

        if (footprintRenderer != null && footprintRenderer.material != null)
        {
            Material mat = footprintRenderer.material;

            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeOutDuration);

                Color color = mat.color;
                color.a = alpha;
                mat.color = color;

                yield return null;
            }
        }

        // 淡出完成后禁用碰撞器，但保持对象直到完全销毁
        if (footprintCollider != null)
        {
            footprintCollider.enabled = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Killed");
            SceneLoader.Instance.ReloadCurrentScene();
        }
    }
    
    private void DestroyFootprint()
    {
 
        
        // 销毁游戏对象（会自动销毁所有子对象）
        Destroy(gameObject);
    }
    
    // 如果需要在外部手动销毁
    public void ForceDestroy()
    {
        DestroyFootprint();
    }
}

