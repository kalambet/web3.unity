// SPDX-License-Identifier: MIT
pragma solidity ^0.8.19;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";

contract MinerDefenceGame is ERC1155 {
    using Arrays for uint256[];
    using Arrays for address[];

    // SessionManager Contract
    address private _sessionManager;

    constructor(address sessionManager) ERC1155("") {
        _sessionManager = sessionManager;
    }

    // Mapping from session id => submitter view of a rollup => on what address => token ids => balancaes
    mapping(uint256 sessionId => mapping(address => mapping(address => mapping(uint256 => uint256)))) private _settelemntView;

    function submitRollupState(
        uint256 sessionId, 
        address player, 
        uint256[] memory ids, 
        uint256[] memory values) public {

    }
}