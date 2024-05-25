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

    // Event to be emitted when a session is joined
    event SessionJoined(
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

    // Mapping to store session IDs by defender address
    mapping(address => uint256) public defenderToSessionId;

    // Counter for session IDs
    uint256 public sessionCounter;
    
    function getSessionInfo(uint256 _sid) public view returns (Session memory) {
        return sessions[_sid];
    }

    // Function to start a session
    function startSession(address defender, string memory rpcUrl) public {
        // Create a new session
        sessions[sessionCounter] = Session({
            id: sessionCounter,
            miner: msg.sender,
            defender: defender,
            rpcUrl: rpcUrl
        });

        // Map defender address to session ID
        defenderToSessionId[defender] = sessionCounter;

        // Emit the SessionStarted event with the miner, defender, and RPC URL
        emit SessionStarted(sessionCounter, msg.sender, defender, rpcUrl);

        // Increment the session counter to get a unique session ID
        sessionCounter++;
    }

    // Function for the defender to join a session
    function joinSession(address miner) public {
        uint256 sessionId = defenderToSessionId[msg.sender];

        // Ensure the session exists
        require(sessionId < sessionCounter, "Session does not exist");

        // Get the session details
        Session memory session = sessions[sessionId];

        // Ensure the miner matches
        require(miner == session.miner, "Miner address does not match");

        // Emit the SessionJoined event with the session details
        emit SessionJoined(session.id, session.miner, session.defender, session.rpcUrl);
    }
}