pragma solidity ^0.5.2;

contract ContractWithOverloading {

    function getString() public returns (uint)
    {
        return 0x150b7a02;
    }

	function getString(uint i) public returns (uint)
    {
        return i;
    }
}