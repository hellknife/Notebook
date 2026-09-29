namespace Notebook.Models;

/// <summary>
/// Контакт - конкретная реализация Person.
/// Демонстрирует наследование и переопределение методов.
/// </summary>
public class Contact : Person
{
    public string Group { get; set; } = "Без группы";
    public string Note { get; set; } = string.Empty;

    public Contact(string fullName) : base(fullName) { }

    public override string GetInfo()
    {
        return $"Имя: {FullName} | Тел: {Phone} | Email: {Email} | Группа: {Group}";
    }

    public override bool Matches(string query)
    {
        return base.Matches(query)
            || Group.ToLower().Contains(query.ToLower())
            || Note.ToLower().Contains(query.ToLower());
    }
}