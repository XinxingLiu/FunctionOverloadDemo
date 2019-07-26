namespace FunctionOverloadDemo
{
    using FluentAssertions;
    using Nethereum.ABI.JsonDeserialisation;
    using Nethereum.Hex.HexConvertors.Extensions;
    using Nethereum.Signer;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;
    using System.Linq;
    using System.Threading.Tasks;

    public class ContractHandler
    {
        private string blockchainNetworkUrl;
        private Account account;
        private Web3 web3;

        public ContractHandler(string blockchainNetworkUrl)
        {
            this.blockchainNetworkUrl = blockchainNetworkUrl; var ecKey = EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            this.account = new Account(privateKey);
            this.web3 = new Web3(account, this.blockchainNetworkUrl);

            web3.TransactionManager.DefaultGas = 6721975;
            web3.TransactionManager.DefaultGasPrice = 0;
        }

        public async Task<string> Deploy(string abi, string contractByteCode, params object[] values)
        {
            var receipt = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(abi, contractByteCode, account.Address, new Nethereum.Hex.HexTypes.HexBigInteger(1000000), null, values);
            return receipt.ContractAddress;
        }

        public async Task VerifyProperty<T>(string abi, string contractAddress, string propertyName, T expectedResult)
        {
            var contract = web3.Eth.GetContract(abi, contractAddress);
            T result = await contract.GetFunction(propertyName).CallAsync<T>();
            result.Should().BeEquivalentTo(expectedResult, $"The value of property {propertyName} is not expected");
        }

        public async Task VerifyFunctionCall<T>(string abi, string contractAddress, string functionName, T expectedResult, params object[] values)
        {
            var contractABI = new ABIDeserialiser().DeserialiseContract(abi);
            var functionABI = contractABI.Functions.FirstOrDefault(x => x.Name == functionName && x.InputParameters.Length == values.Length);

            var contract = web3.Eth.GetContract(abi, contractAddress);
            T result = await contract.GetFunctionBySignature(functionABI.Sha3Signature).CallAsync<T>(values);
            result.Should().BeEquivalentTo(expectedResult, $"The result of function call {functionName} is not expected");
        }
    }
}