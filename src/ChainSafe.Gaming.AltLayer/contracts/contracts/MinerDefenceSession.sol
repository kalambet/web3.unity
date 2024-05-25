// SPDX-License-Identifier: MIT
pragma solidity ^0.8.19;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";

contract MinerDefenceSession is ERC1155 {
    address private _minter;
    address private _defender;

    bool private _minterResolved = false;
    bool private _defenderResolved = false;

    error MinterUnauthorizedAccess(address minter);
    error DefenderUnauthorizedAccess(address minter);

    error InactiveAccess(address sender);
    error UnauthorizedResolution(address sender);

    modifier stillActive() {
        if(_minterResolved || _defenderResolved) {
            revert InactiveAccess(_msgSender());
        }
        _;
    }

    modifier onlyMinter() {
        if(_msgSender() != minter()) {
            revert MinterUnauthorizedAccess(_msgSender());
        }
        _;
    }

    constructor(address new_minter, address new_defender) ERC1155(""){
        _minter = new_minter;
        _defender = new_defender;
    }

    function minter() public view returns (address) {
        return _minter;
    }

    function defender() public view returns (address) {
        return _defender;
    }

    function moveResources(address from_minter, address to_defender, uint256[] memory ids, uint256[] memory values, bytes memory data) stillActive onlyMinter public {
        address sender = _msgSender();
        if (from_minter != sender && !isApprovedForAll(from_minter, sender)) {
            revert ERC1155MissingApprovalForAll(sender, from_minter);
        }
        _safeBatchTransferFrom(from_minter, to_defender, ids, values, data);
    }

    function mint(address to_minter, uint256 id, uint256 value, bytes memory data) stillActive onlyMinter public {
        _mint(to_minter, id, value, data);  
    }

    function resolve() public {
        address sender = _msgSender();
        if (sender == minter()) {
            _minterResolved = true;
            return;
        } else if (sender == defender()) {
            _defenderResolved = true;
            return;
        }
        revert UnauthorizedResolution(sender);
    }
}