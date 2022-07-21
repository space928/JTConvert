namespace JTConvert.JTCodec
{
    public interface IJTSegmentLoader
    {
        // TODO: Currently abstract static methods are not supported in C#9.
        // Since this is a purely optional bit, to add a little more rigour to
        // the API, we can ommit it for now.
        //public abstract static JTSegment LoadSegment(ref BinaryJTReader reader);
    }
}