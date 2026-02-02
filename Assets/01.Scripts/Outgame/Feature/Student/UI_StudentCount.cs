using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class UI_StudentCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI studentCountTextUI;

    private void Start()
    {
        StudentManager.Instance.OnDataChanged += Refresh;
        
        Refresh();
    }
    
    private void Refresh()
    {
        var students = StudentManager.Instance.GetAll();
        int totalStudentCount = students.Count;
        int attendanceCount = students.Count(s => s.IsAttendance == true);
        
        studentCountTextUI.text = $"{attendanceCount}/{totalStudentCount}";
    }
}
