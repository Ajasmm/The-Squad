public class PhotonCustomAnimatorEvents
{
    public int propertyHash;
    public CustomAnimatorEventType Type;
    public object value;

}
public enum CustomAnimatorEventType
{
    Trigger,
    Float,
    Int,
    Bool
}