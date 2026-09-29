namespace Notebook.Interfaces;

/// <summary>
/// Интерфейс для сущностей, поддерживающих поиск по ключевому слову.
/// Демонстрирует принцип абстракции и полиморфизма.
/// </summary>
public interface ISearchable
{
    /// <summary>
    /// Проверяет, содержит ли объект указанную подстроку.
    /// </summary>
    bool Matches(string query);
}