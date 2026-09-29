using Notebook.Interfaces;

namespace Notebook.Models;

/// <summary>
/// Абстрактный базовый класс, описывающий человека.
/// Демонстрирует наследование и инкапсуляцию.
/// </summary>
public abstract class Person : ISearchable
{
    private string _fullName = string.Empty;

    public string FullName
    {
        get => _fullName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя не может быть пустым");
            _fullName = value.Trim();
        }
    }

    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    protected Person(string fullName)
    {
        FullName = fullName;
    }

    /// <summary>
    /// Абстрактный метод - каждая конкретная реализация возвращает своё описание.
    /// Демонстрирует полиморфизм.
    /// </summary>
    public abstract string GetInfo();

    public virtual bool Matches(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return false;
        query = query.ToLower();
        return FullName.ToLower().Contains(query)
            || Phone.Contains(query)
            || Email.ToLower().Contains(query);
    }
}