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

    public class AddInheritanceBaseRequest
    {
        [BindingAlias("OverriddenBaseAlias")]
        public virtual int Overridden { get; set; }

        [BindingAlias("HiddenBaseAlias")]
        public int Hidden { get; set; }

        public virtual int AliasInDerived { get; set; }

        [BindingAlias("AliasInBaseBaseAlias")]
        public virtual int AliasInBase { get; set; }

        [BindingAlias("NotOverridenWithAliasBaseAlias")]
        public int NotOverridenWithAlias { get; set; }

        public int NotOverriden { get; set; }
    }

    public class AddInheritanceDerivedRequest : AddInheritanceBaseRequest
    {
        [BindingAlias("OverriddenDerivedAlias")]
        public override int Overridden { get; set; }

        [BindingAlias("HiddenDerivedAlias")]
        public new int Hidden { get; set; }

        [BindingAlias("AliasInDerivedDerivedAlias")]
        public override int AliasInDerived { get; set; }

        public override int AliasInBase { get; set; }
    }
}
