namespace Notebook.Interfaces;

/// <summary>
/// Интерфейс хранилища данных.
/// Позволяет менять способ сохранения (JSON, XML, база данных)
/// без изменения основной логики приложения.
/// </summary>
public interface IStorage<T>
{
    void Save(IEnumerable<T> items, string path);
    List<T> Load(string path);
}