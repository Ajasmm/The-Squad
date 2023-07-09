using System;

public struct SyncValue<T>
{
    public T Value
    {
        get { return value; }
        set
        {
            this.value = value;
            onValueChange?.Invoke(value);
        }
    }
    private T value;

    Action<T> onValueChange;

    public void AddListener(Action<T> listener)
    {
        listener?.Invoke(value);
        onValueChange += listener;
    }
    public void RemoveListener(Action<T> listener)
    {
        onValueChange -= listener;
    }
    public void Invoke()
    {
        onValueChange?.Invoke(value);
    }
}