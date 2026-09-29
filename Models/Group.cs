namespace Notebook.Models;

/// <summary>
/// Группа контактов (например, "Работа", "Друзья", "Семья").
/// </summary>
public class Group
{
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;

    public Group(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название группы не может быть пустым");
        Name = name.Trim();
    }

    public override string ToString() => Name;
}