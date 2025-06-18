namespace Arguments
{
    public class CollectionHandlerEventArgs : EventArgs
    {
        public string ChangeType { get; }
        public object ChangedObject { get; }
        public string Name { get; }

        public CollectionHandlerEventArgs(string name, string changeType, object changedObject)
        {
            Name = name;
            ChangeType = changeType;
            ChangedObject = changedObject;
        }
    }
}
