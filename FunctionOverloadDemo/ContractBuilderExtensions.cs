namespace FunctionOverloadDemo
{
    using Nethereum.ABI.Model;
    using Nethereum.Contracts;
    using Nethereum.Hex.HexConvertors.Extensions;
    using System;
    using System.Linq;

    public static class ContractBuilderExtensions
    {
        public static FunctionBuilder GetFunctionBuilderBySignature(this ContractBuilder cb, string signature)
        {
            return new FunctionBuilder(cb.Address, cb.GetFunctionAbiBySignature(signature));
        }

        public static FunctionABI GetFunctionAbiBySignature(this ContractBuilder cb, string signature)
        {
            if (cb.ContractABI == null) throw new Exception("Contract abi not initialised");
            var functionAbi = cb.ContractABI.Functions.FirstOrDefault(x => x.Sha3Signature.ToLowerInvariant().EnsureHexPrefix() == signature.ToLowerInvariant().EnsureHexPrefix());
            if (functionAbi == null) throw new Exception("Function not found for signature:" + signature);
            return functionAbi;
        }
    }
}
