using System.Collections;
using UnityEngine;

public class CustomerVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject customerVisuals;
    [SerializeField] private Animator customerAnimator;
    [SerializeField] private Animator customerMover;

    [SerializeField] private Renderer customerRenderer;

    [SerializeField] private Material[] customerMaterials;

    [SerializeField] private GameObject thoughBubbleVisuals;
    [SerializeField] private GameObject[] thoughtBubbleDisablables;

    private void Start()
    {
        HideBubble();
    }
    public void SpawnCustomerVisuals()
    {
        RandomizeVisuals();
        customerVisuals.SetActive(false);
        customerVisuals.SetActive(true);
        StartCoroutine(nameof(CustomerIdleBehaviour));
        AudioManager.Instance.PlaySoundEffect("Swoosh1");
        StartCoroutine(ThoughBubbleSpawnLogic());
    }

    public void RemoveCustomerVisuals()
    {
        StopCoroutine(nameof(CustomerIdleBehaviour));
        StartCoroutine(CustomerDespawner());
    }

    private void HideBubble()
    {
        thoughBubbleVisuals.SetActive(false);
        foreach (var element in thoughtBubbleDisablables)
        {
            element.SetActive(false);
        }
    }

    public void CustomerHappy()
    {
        AudioManager.Instance.PlaySoundEffect("HappyCustomer");
        customerAnimator.SetTrigger("Excited");
    }

    public void CustomerDisappointed()
    {
        AudioManager.Instance.PlaySoundEffect("DisappointedCustomer");
        customerAnimator.SetTrigger("Disappointed");
    }

    private IEnumerator CustomerDespawner()
    {
        customerMover.Play("LeaveCustomer", 0, 0);
        AudioManager.Instance.PlaySoundEffect("Swoosh1");
        yield return new WaitForSeconds(0.3f);
        customerVisuals.SetActive(false);
        HideBubble();
    }

    public IEnumerator CustomerIdleBehaviour()
    {
        yield return new WaitForSeconds(Random.Range(5f, 30f));
        int behaviour = Random.Range(0, 2);
        switch (behaviour)
        {
            case 0: customerAnimator.SetTrigger("Sweep"); break;
            case 1: customerAnimator.SetTrigger("Smile"); break;
        }

        StartCoroutine(nameof(CustomerIdleBehaviour));
    }

    private void RandomizeVisuals()
    {
        Material selectedMaterial = customerMaterials[Random.Range(0, customerMaterials.Length)];
        customerRenderer.material = selectedMaterial;
    }

    private IEnumerator ThoughBubbleSpawnLogic()
    {
        yield return new WaitForSeconds(0.3f);
        var animator = thoughBubbleVisuals.GetComponent<Animator>();
        thoughBubbleVisuals.SetActive(true);
        animator.Play("SpawnBubble");
        yield return new WaitForSeconds(0.5f);

        foreach(var element in thoughtBubbleDisablables)
        {
            element.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
