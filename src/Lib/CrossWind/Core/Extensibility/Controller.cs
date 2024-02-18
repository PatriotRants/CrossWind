namespace ForgeWorks.CrossWind.Core;

public abstract class Controller : IController
{
    private bool disposedValue;

    public ControllerId Id { get; }

    public event OnControllerDisposing OnDisposing;

    public string Name => Id.Name;
    public int Handle => Id.GetHashCode();
    public Type Type => Id.Type;

    protected Controller(string name)
    {
        Id = new(GetType())
        {
            Name = name
        };
    }

    public void Dispose()
    {
        OnDisposing?.Invoke(Id);

        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~Controller()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }
}
