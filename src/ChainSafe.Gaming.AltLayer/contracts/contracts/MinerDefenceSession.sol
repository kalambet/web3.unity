// SPDX-License-Identifier: MIT
pragma solidity ^0.8.19;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import {Arrays} from "@openzeppelin/contracts/utils/Arrays.sol";

contract MinerDefenceSession is ERC1155 {
    using Arrays for uint256[];
    using Arrays for address[];

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

    modifier onlyDefender() {
        if(_msgSender() != defender()) {
            revert DefenderUnauthorizedAccess(_msgSender());
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

    function mineBatch(
        uint256[] memory ids,
        uint256[] memory values,
        bytes memory data) stillActive onlyMiner public {
        for (uint256 i = 0; i < ids.length; i++) {
            uint256 id = ids.unsafeMemoryAccess(i);
            uint256 value = values.unsafeMemoryAccess(i);
            
            _mint(_msgSender(), id, value, data);
        }
    }

    function burnBatch(
        uint256[] memory ids,
        uint256[] memory values) stillActive onlyDefender public {    
        _burnBatch(defender(), ids, values);
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