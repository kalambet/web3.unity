// SPDX-License-Identifier: MIT
pragma solidity >=0.6.12 <0.9.0;

contract SessionManager {
    // Event to be emitted when a session is started
    event SessionStarted(
        uint256 id,
        address indexed miner,
        address indexed defender,
        string rpcUrl
    );

    // Struct to hold session details
    struct Session {
        uint256 id;
        address miner;
        address defender;
        string rpcUrl;
    }

    // Mapping to store sessions with a unique ID
    mapping(uint256 => Session) public sessions;

    function getSessionInfo(uint256 _sid) public view returns (Session memory) {
        return sessions[_sid];
    }

    // Counter for session IDs
    uint256 public sessionCounter;

    // Function to start a session
    function startSession(address defender, string memory rpcUrl) public {
        // Create a new session
        sessions[sessionCounter] = Session({
            id: sessionCounter,
            miner: msg.sender,
            defender: defender,
            rpcUrl: rpcUrl
        });

        // Emit the SessionStarted event with the miner, defender, and RPC URL
        emit SessionStarted(sessionCounter, msg.sender, defender, rpcUrl);

        // Increment the session counter to get a unique session ID
        sessionCounter++;
    }
}
