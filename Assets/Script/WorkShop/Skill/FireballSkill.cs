using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireballSkill", menuName = "Skills/FireballSkill")]
public class FireballSkill : Skill
{
    public float searchRadius = 5;

    public FireballSkill()
    {
        this.skillName = "FireballSkill";
        this.cooldownTime = 5;
    }
    public override void Activate(Character character)
    {
        Debug.Log(character.Name +" Casting Fireball! Deals 50 damage.");

        Enemy[] target = GetEnemysInRange(character);
        if (target.Length>0)
        {
            foreach (var enemy in target)
            {
                enemy.TakeDamage(50);
                Debug.Log($"{character.Name} casts {skillName} on {enemy.Name}, dealing 50 damage!");
            }
        }
        else
        {
            Debug.Log("No enemies in range to target with Fireball.");
        }
    }
    public override void Deactivate(Character character)
    {

    }
    public override void UpdateSkill(Character character)
    {

    }
    private Enemy[] GetEnemysInRange(Character caster)
    {
        Collider[] hitColliders = Physics.OverlapSphere(caster.transform.position, searchRadius);
        List<Enemy> Enemys = new List<Enemy>();
        foreach (var hitCollider in hitColliders)
        {
            Enemy targetCharacter = hitCollider.GetComponent<Enemy>();
            if (targetCharacter != null && targetCharacter != caster)
            {
                Enemys.Add(targetCharacter);
            }
        }
        return Enemys.ToArray();
    }
}
