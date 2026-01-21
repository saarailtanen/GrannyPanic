using UnityEngine;
using System.Collections;

public class SoundFxManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] catSounds;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomCatSound());
    }
    
    IEnumerator PlayRandomCatSound()
    {
        while (true)
        {
            // Wait for a random time interval
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            
            // Select and play a random cat sound
            int randomIndex = Random.Range(0, catSounds.Length);
            audioSource.PlayOneShot(catSounds[randomIndex]);
        }
    }
}
