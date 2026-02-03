using UnityEngine;


public class Dog
{
    public string Name;
    public int Age;

    public Dog(string name, int age)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new System.ArgumentNullException("이름은 비어있을 수 없습니다.");
        }

        if (age <= 0)
        {
            throw new System.ArgumentNullException("나이는 0살보다 작을 수 업습니다.");
        }
    }
}
