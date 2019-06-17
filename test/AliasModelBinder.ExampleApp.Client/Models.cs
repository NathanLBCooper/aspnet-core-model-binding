using AliasModelBinder.Client;

namespace AliasModelBinder.ExampleApp.Client
{
    public class EchoRequest
    {
        [BindingAlias("num")]
        [BindingAlias("n")]
        public int Number { get; set; }
    }

    public class EchoCollectionRequest
    {
        [BindingAlias("int")]
        [BindingAlias("i")]
        public int[] Integers { get; set; }
    }

    public class AddRequest
    {
        [BindingAlias("l")]
        [BindingAlias("term")]
        public int LeftSummand { get; set; }

        [BindingAlias("r")]
        [BindingAlias("term")]
        public int RightSummand { get; set; }
    }
}
