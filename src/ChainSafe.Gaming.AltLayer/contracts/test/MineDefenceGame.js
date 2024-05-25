const {
    time,
    loadFixture,
  } = require("@nomicfoundation/hardhat-toolbox/network-helpers");
  const { anyValue } = require("@nomicfoundation/hardhat-chai-matchers/withArgs");
  const { expect } = require("chai");

  describe("MinerDefenceGame", function () {
    async function deployBasicGame() {
        const [miner, defender] = await ethers.getSigners();
        const MinerDefenceGame = await ethers.getContractFactory("MinerDefenceGame");
        const SessionManager = await ethers.getContractFactory("SessionManager");

        const sm = await SessionManager.deploy();
        await sm.waitForDeployment();
        const mfg = await MinerDefenceGame.deploy(sm.getAddress());

        return { mfg, sm, miner, defender };
    }

    describe("Settlement", function () {
        it("Successful claim", async function () {
            const { mfg, sm, miner, defender } = await loadFixture(deployBasicGame);

            await sm.startSession(defender, "");
            expect(await sm.sessionCounter()).to.equal(1);
            await expect(sm.connect(defender).joinSession(miner)).to.emit(sm, "SessionJoined");

            const sessionId = 0;
            const sifo = await sm.getSessionInfo(sessionId);
            expect(sifo.miner).to.equal(miner);
            expect(sifo.defender).to.equal(defender);

            await expect(
                mfg.submitRollupState(sessionId, miner, [1, 2], [100, 200])).to.emit(
                    mfg, "RollupStateSubmit").withArgs(sessionId, miner, miner, [1, 2], [100, 200]);
            expect(await mfg.getSettlementView(sessionId, miner, miner, 1)).to.equal(100);
            expect(await mfg.getSettlementView(sessionId, miner, miner, 2)).to.equal(200);

            await mfg.submitRollupState(sessionId, defender, [1, 2, 3], [150, 250, 350]);
            expect(await mfg.getSettlementView(sessionId, miner, defender, 1)).to.equal(150);
            expect(await mfg.getSettlementView(sessionId, miner, defender, 2)).to.equal(250);
            expect(await mfg.getSettlementView(sessionId, miner, defender, 3)).to.equal(350); 

            await mfg.connect(defender).submitRollupState(sessionId, miner, [1, 2], [100, 200]);
            expect(await mfg.getSettlementView(sessionId, defender, miner, 1)).to.equal(100);
            expect(await mfg.getSettlementView(sessionId, defender, miner, 2)).to.equal(200);

            await mfg.connect(defender).submitRollupState(sessionId, defender, [1, 2, 3], [150, 250, 350]);
            expect(await mfg.getSettlementView(sessionId, defender, defender, 1)).to.equal(150);
            expect(await mfg.getSettlementView(sessionId, defender, defender, 2)).to.equal(250);
            expect(await mfg.getSettlementView(sessionId, defender, defender, 3)).to.equal(350); 


            await expect(mfg.claim(sessionId, [1, 2], "0x")).to.emit(mfg, "TransferSingle");
            expect(await mfg.balanceOf(miner, 1)).to.equal(100);
            expect(await mfg.balanceOf(miner, 2)).to.equal(200);

            await mfg.connect(defender).claim(sessionId, [1, 2, 3], "0x");
            expect(await mfg.balanceOf(defender, 1)).to.equal(150);
            expect(await mfg.balanceOf(defender, 2)).to.equal(250);
            expect(await mfg.balanceOf(defender, 3)).to.equal(350);

        });

        it("Unuccessful claim", async function () {
            const { mfg, sm, miner, defender } = await loadFixture(deployBasicGame);

            await sm.startSession(defender, "");
            expect(await sm.sessionCounter()).to.equal(1);
            await expect(sm.connect(defender).joinSession(miner)).to.emit(sm, "SessionJoined");

            const sessionId = 0;
            const sifo = await sm.getSessionInfo(sessionId);
            expect(sifo.miner).to.equal(miner);
            expect(sifo.defender).to.equal(defender);

            await expect(
                mfg.submitRollupState(sessionId, miner, [1, 2], [110, 210])).to.emit(
                    mfg, "RollupStateSubmit").withArgs(sessionId, miner, miner, [1, 2], [110, 210]);
            expect(await mfg.getSettlementView(sessionId, miner, miner, 1)).to.equal(110);
            expect(await mfg.getSettlementView(sessionId, miner, miner, 2)).to.equal(210);

            await mfg.submitRollupState(sessionId, defender, [1, 2, 3], [150, 250, 350]);
            expect(await mfg.getSettlementView(sessionId, miner, defender, 1)).to.equal(150);
            expect(await mfg.getSettlementView(sessionId, miner, defender, 2)).to.equal(250);
            expect(await mfg.getSettlementView(sessionId, miner, defender, 3)).to.equal(350); 

            await mfg.connect(defender).submitRollupState(sessionId, miner, [1, 2], [100, 200]);
            expect(await mfg.getSettlementView(sessionId, defender, miner, 1)).to.equal(100);
            expect(await mfg.getSettlementView(sessionId, defender, miner, 2)).to.equal(200);

            await mfg.connect(defender).submitRollupState(sessionId, defender, [1, 2, 3], [150, 250, 350]);
            expect(await mfg.getSettlementView(sessionId, defender, defender, 1)).to.equal(150);
            expect(await mfg.getSettlementView(sessionId, defender, defender, 2)).to.equal(250);
            expect(await mfg.getSettlementView(sessionId, defender, defender, 3)).to.equal(350); 


            await mfg.claim(sessionId, [1, 2], "0x");
            expect(await mfg.balanceOf(miner, 1)).to.equal(0);
            expect(await mfg.balanceOf(miner, 2)).to.equal(0);

            await mfg.connect(defender).claim(sessionId, [1, 2, 3], "0x");
            expect(await mfg.balanceOf(defender, 1)).to.equal(150);
            expect(await mfg.balanceOf(defender, 2)).to.equal(250);
            expect(await mfg.balanceOf(defender, 3)).to.equal(350);

        });
    });

  });