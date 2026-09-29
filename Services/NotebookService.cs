using Notebook.Interfaces;
using Notebook.Models;

namespace Notebook.Services;

/// <summary>
/// Основной сервис записной книжки.
/// Управляет коллекцией контактов и групп, выполняет поиск.
/// Демонстрирует инкапсуляцию и разделение ответственности.
/// </summary>
public class NotebookService
{
    private readonly List<Contact> _contacts = new();
    private readonly List<Group> _groups = new();
    private readonly IStorage<Contact> _contactStorage;
    private readonly IStorage<Group> _groupStorage;

    private const string ContactsFile = "contacts.json";
    private const string GroupsFile = "groups.json";

    public IReadOnlyList<Contact> Contacts => _contacts;
    public IReadOnlyList<Group> Groups => _groups;

    public NotebookService(IStorage<Contact> contactStorage, IStorage<Group> groupStorage)
    {
        _contactStorage = contactStorage;
        _groupStorage = groupStorage;
    }

    public void AddContact(Contact contact)
    {
        if (_contacts.Any(c => c.FullName.Equals(contact.FullName, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Контакт '{contact.FullName}' уже существует");

        _contacts.Add(contact);
    }

    public bool RemoveContact(string fullName)
    {
        var contact = _contacts.FirstOrDefault(c =>
            c.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase));
        if (contact == null) return false;

        _contacts.Remove(contact);
        return true;
    }

    public void AddGroup(Group group)
    {
        if (_groups.Any(g => g.Name.Equals(group.Name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Группа '{group.Name}' уже существует");

        _groups.Add(group);
    }

    /// <summary>
    /// Поиск контактов. Использует интерфейс ISearchable - полиморфизм.
    /// </summary>
    public List<Contact> Search(string query)
    {
        return _contacts.Where(c => c.Matches(query)).ToList();
    }

    public List<Contact> GetByGroup(string groupName)
    {
        return _contacts
            .Where(c => c.Group.Equals(groupName, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void Save()
    {
        _contactStorage.Save(_contacts, ContactsFile);
        _groupStorage.Save(_groups, GroupsFile);
    }

    public void Load()
    {
        _contacts.Clear();
        _contacts.AddRange(_contactStorage.Load(ContactsFile));

        _groups.Clear();
        _groups.AddRange(_groupStorage.Load(GroupsFile));
    }
}