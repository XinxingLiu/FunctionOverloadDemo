using System;
using System.IO;
using Nethereum.Hex.HexConvertors.Extensions;

namespace FunctionOverloadDemo
{
    class Program
    {
        static public void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var contractHandler = new ContractHandler("http://127.0.0.1:8545");

            string abiString;
            using (StreamReader reader = new StreamReader(File.OpenRead(@"ContractWithOverloading.abi")))
            {
                abiString = reader.ReadToEnd();
            }

            string byteCodeString;
            using (StreamReader reader = new StreamReader(File.OpenRead(@"ContractWithOverloading.bin")))
            {
                byteCodeString = reader.ReadToEnd();
            }

            var contractAddress = contractHandler.Deploy(abiString, byteCodeString).GetAwaiter().GetResult();

            contractHandler.VerifyFunctionCall(abiString, contractAddress, "getString", "0x150b7a02".HexToBigInteger(false)).GetAwaiter().GetResult();
            contractHandler.VerifyFunctionCall(abiString, contractAddress, "getString", 1005, 1005).GetAwaiter().GetResult();

            Console.WriteLine("Success!");
            Console.ReadKey();
        }
    }
}
