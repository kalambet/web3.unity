// SPDX-License-Identifier: MIT
pragma solidity ^0.8.19;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "./SessionManager.sol";

contract MinerDefenceGame is ERC1155 {
    using Arrays for uint256[];
    using Arrays for address[];

    // SessionManager Contract
    SessionManager private _sessionManager;

    error InvalidSession(uint256);
    error UnauthorizedSubmit(address sender);
    error IllegalClaim(address sender);

    event RollupStateSubmit(
        uint256 indexed sessionId,
        address indexed player,
        address indexed from,
        uint256[] ids,
        uint256[] values
    );

    constructor(address sessionManager) ERC1155("") {
        _sessionManager = SessionManager(sessionManager);
    }

    // Mapping from session id => submitter view of a rollup => on what address => token ids => balancaes
    mapping(uint256 sessionId => mapping(address => mapping(address => mapping(uint256 => uint256)))) private _settelemntView;

    function submitRollupState(
        uint256 sessionId, 
        address player, 
        uint256[] memory ids, 
        uint256[] memory values) public {

            address sender = _msgSender();
            SessionManager.Session memory s = _sessionManager.getSessionInfo(sessionId);
            if (s.miner == address(0) || s.defender == address(0)) {
                revert InvalidSession(sessionId);
            }

            if (s.miner != sender && s.defender != sender) {
                revert UnauthorizedSubmit(sender);
            }

            for (uint256 i = 0; i <= ids.length; ++i) {
                uint256 id = ids.unsafeMemoryAccess(i);
                uint256 value = values.unsafeMemoryAccess(i);

                _settelemntView[sessionId][sender][player][id] = value;
            }

            emit RollupStateSubmit(sessionId, sender, player, ids, values);
    }

    function claim(uint256 sessionId, address player, uint256[] memory ids, bytes memory data) public {
        address sender = _msgSender();

        if (player == address(0)) {
            revert IllegalClaim(sender);
        }

        for (uint256 i = 0; i <= ids.length; ++i) {
                uint256 id = ids.unsafeMemoryAccess(i);

                if (_settelemntView[sessionId][sender][player][id] == _settelemntView[sessionId][player][sender][id]) {
                    _mint(sender, id, _settelemntView[sessionId][sender][player][id], data);
                }
        }
    }
}