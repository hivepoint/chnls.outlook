namespace chnls.Model
{
    // ReSharper disable InconsistentNaming
    internal class NameValuePair
    {
        public string name;
        public string value;

        public override string ToString()
        {
            return name + ":" + value;
        }
    }

    // ReSharper restore InconsistentNaming
}