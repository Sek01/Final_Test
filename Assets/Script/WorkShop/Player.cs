using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Hand setting")]
    public Transform RightHand;
    public Transform LeftHand;
    public List<Item> inventory = new List<Item>();

    Vector3 _inputDirection;
    bool _isAttacking = false;
    bool _isInteract = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    public void FixedUpdate()
    {
        Move(_inputDirection);
        Turn(_inputDirection);
        Attack(_isAttacking);
        Interact(_isInteract);
    }
    public void Update()
    {
        HandleInput();
    }
    public void AddItem(Item item) 
    {
        inventory.Add(item);
    }
    
    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _inputDirection = new Vector3(x, 0, y);
        if (Input.GetMouseButtonDown(0)) {
            _isAttacking = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player pressed E ==================");
            _isInteract = true;
        }

    }
    public void Attack(bool isAttacking) 
    {
        if (isAttacking) {
            animator.SetTrigger("Attack");
            var e = InFront as Idestoryable;
            if (e != null)
            {
                e.TakeDamage(Damage);
                Debug.Log($"{gameObject.name} attacks for {Damage} damage.");
            }
            _isAttacking = false;
        }
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        // ⭐ อัปเดตเฉพาะ HPBar ของผู้เล่นเท่านั้น
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthBar(health, maxHealth);
        }
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);

        // ⭐อัปเดต UI เฉพาะ Player เท่านั้น
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthBar(health, maxHealth);
        }
    }

    private void Interact(bool interactable)
    {
        if (interactable)
        {
            IInteractable e = InFront as IInteractable;

            if (e != null) {
                e.Interact(this);
            }
            _isInteract = false;

        }
    }
}
