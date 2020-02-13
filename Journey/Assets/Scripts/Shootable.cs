using UnityEngine;

public interface Shootable
{
    void Damage(int amount, GameObject sender); //Reviece the amount of damage along with the info of who sent the bullet
}
