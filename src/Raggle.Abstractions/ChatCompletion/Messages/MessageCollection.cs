﻿using System.Collections;

namespace Raggle.Abstractions.ChatCompletion.Messages;

public sealed class MessageCollection : ICollection<IMessage>
{
    private readonly List<IMessage> _items;

    public MessageCollection()
    {
        _items = new();
    }

    public MessageCollection(IEnumerable<IMessage> messages)
    {
        _items = new(messages);
    }

    public void AddUserMessage(IMessageContent content)
    {
        Add<UserMessage>(content);
    }

    public void AddAssistantMessage(IMessageContent content)
    {
        Add<AssistantMessage>(content);
    }

    public void Add<T>(IMessageContent content)
        where T : IMessage
    {
        if (_items.Last() is T)
        {
            _items.Last().Content.Add(content);
        }
        else
        {
            var message = Activator.CreateInstance<T>();
            message.Content.Add(content);
            _items.Add(message);
        }
    }

    public void Add<T>(MessageContentCollection content)
        where T : IMessage
    {
        if (_items.Last() is T)
        {
            _items.Last().Content.AddRange(content);
        }
        else
        {
            var message = Activator.CreateInstance<T>();
            message.Content.AddRange(content);
            Add(message);
        }
    }

    #region ICollection Implementations

    public int Count => _items.Count;

    public bool IsReadOnly => false;

    public IMessage this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    public void Add(IMessage message) => _items.Add(message);

    public bool Remove(IMessage message) => _items.Remove(message);

    public bool Contains(IMessage message) => _items.Contains(message);

    public void CopyTo(IMessage[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public void Clear() => _items.Clear();

    public IEnumerator<IMessage> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}