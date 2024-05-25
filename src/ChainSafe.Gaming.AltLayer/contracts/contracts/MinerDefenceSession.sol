// SPDX-License-Identifier: MIT
pragma solidity ^0.8.19;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";

contract MinerDefenceSession is ERC1155 {
    address private _miner;
    address private _defender;

    bool private _minerResolved = false;
    bool private _defenderResolved = false;

    error MinerUnauthorizedAccess(address miner);
    error DefenderUnauthorizedAccess(address sender);

    error InactiveAccess(address sender);
    error UnauthorizedResolution(address sender);

    modifier stillActive() {
        if(_minerResolved || _defenderResolved) {
            revert InactiveAccess(_msgSender());
        }
        _;
    }

    modifier onlyMiner() {
        if(_msgSender() != miner()) {
            revert MinerUnauthorizedAccess(_msgSender());
        }
        _;
    }

    constructor(address new_miner, address new_defender) ERC1155(""){
        _miner = new_miner;
        _defender = new_defender;
    }

    function miner() public view returns (address) {
        return _miner;
    }

    function defender() public view returns (address) {
        return _defender;
    }

    function moveResources(
        address from_miner, 
        address to_defender, 
        uint256[] memory ids, 
        uint256[] memory values, 
        bytes memory data) stillActive onlyMiner public {
        address sender = _msgSender();
        if (from_miner != sender && !isApprovedForAll(from_miner, sender)) {
            revert ERC1155MissingApprovalForAll(sender, from_miner);
        }
        _safeBatchTransferFrom(from_miner, to_defender, ids, values, data);
    }

    function mine(address to_miner, uint256 id, uint256 value, bytes memory data) stillActive onlyMiner public {
        _mint(to_miner, id, value, data);  
    }

    function resolve() public {
        address sender = _msgSender();
        if (sender == miner()) {
            _minerResolved = true;
            return;
        } else if (sender == defender()) {
            _defenderResolved = true;
            return;
        }
        revert UnauthorizedResolution(sender);
    }
}