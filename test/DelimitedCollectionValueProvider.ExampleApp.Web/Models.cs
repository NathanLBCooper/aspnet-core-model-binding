
namespace DelimitedCollectionValueProvider.ExampleApp.Web
{
    public class EchoCollectionRequest
    {
        public int[] Integers { get; set; }
    }

    public class EchoMultipleCollectionsRequest
    {
        public int[] One { get; set; }
        public int[] Two { get; set; }
        public int[] Three { get; set; }
    }
}
