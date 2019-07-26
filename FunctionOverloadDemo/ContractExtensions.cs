namespace FunctionOverloadDemo
{
    using Nethereum.Contracts;

    public static class ContractExtensions
    {
        public static Function GetFunctionBySignature(this Contract contract, string signature)
        {
            return new Function(contract, contract.GetFunctionBuilderBySignature(signature));
        }

        public static FunctionBuilder GetFunctionBuilderBySignature(this Contract contract, string signature)
        {
            return contract.ContractBuilder.GetFunctionBuilderBySignature(signature);
        }
    }
}
