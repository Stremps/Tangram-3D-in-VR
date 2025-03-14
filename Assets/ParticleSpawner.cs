using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public ParticleSystem particleEffect; // Arraste o sistema de partículas aqui no Inspector
    public bool destroyAfterPlay = false; // Se ativado, destrói o objeto após a execução das partículas

    void Start()
    {
        // Garante que o sistema de partículas está desativado no início
        if (particleEffect != null)
        {
            particleEffect.Stop();
        }
    }

    public void PlayParticles()
    {
        if (particleEffect != null)
        {
            particleEffect.Play(); // Dispara as partículas

            // Se marcado, destrói o GameObject após o tempo de vida das partículas
            if (destroyAfterPlay)
            {
                Destroy(gameObject, particleEffect.main.duration);
            }
        }
    }

    void Update()
    {
        // Se quiser ativar as partículas ao apertar espaço, por exemplo:
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayParticles();
        }
    }
}
