const {
    time,
    loadFixture,
  } = require("@nomicfoundation/hardhat-toolbox/network-helpers");
  const { anyValue } = require("@nomicfoundation/hardhat-chai-matchers/withArgs");
  const { expect } = require("chai");
  
  describe("MinerDefenceSession", function () {
      async function deployBasicGameSession() {
          const [miner, defender] = await ethers.getSigners();
          const MinerDefenceSession = await ethers.getContractFactory("MinerDefenceSession");
          const mfs = await MinerDefenceSession.deploy(miner, defender);
  
          return { mfs, miner, defender };
      }
  
      describe("Deployment", function () {
        it("Should set the right players", async function () {
            const { mfs, miner, defender } = await loadFixture(deployBasicGameSession);
    
            expect(await mfs.miner()).to.equal(miner);
            expect(await mfs.defender()).to.equal(defender);
      });
  
      describe("Mining", function () {
        it("`mine` should be onlly callable by the miner", async function () {
            const { mfs, miner, defender } = await loadFixture(deployBasicGameSession);
            await mfs.mineBatch([1], [100], "0x");
            expect(await mfs.balanceOf(miner, 1)).to.equal(100);
            await expect(mfs.connect(defender).mineBatch([1], [100], "0x")).to.be.reverted;

            await mfs.mineBatch([1, 2], [100, 300], "0x");
            expect(await mfs.balanceOf(miner, 1)).to.equal(200);
            expect(await mfs.balanceOf(miner, 2)).to.equal(300);

        });
      });

      describe("Move", function () {
        it("`move` is working and onlly callable by the miner", async function () {
            const { mfs, miner, defender } = await loadFixture(deployBasicGameSession);
            await mfs.mineBatch([1, 2], [100, 300], "0x");
            await mfs.moveResources(miner, defender, [1, 2], [50, 50], "0x");

            expect(await mfs.balanceOf(miner, 1)).to.equal(50);
            expect(await mfs.balanceOf(miner, 2)).to.equal(250);

            expect(await mfs.balanceOf(defender, 1)).to.equal(50);
            expect(await mfs.balanceOf(defender, 2)).to.equal(50);

            await expect(mfs.moveResources(miner, defender, [1], [100], "0x")).to.be.reverted;

            await expect(mfs.connect(defender).moveResources(miner, defender, [1], [100], "0x")).to.be.reverted;
        });
      });

      describe("Burn", function () {
        it("`burn` is working and onlly callable by the defender", async function () {
            const { mfs, miner, defender } = await loadFixture(deployBasicGameSession);
            await mfs.mineBatch([1, 2], [100, 300], "0x");
            await mfs.moveResources(miner, defender, [1, 2], [50, 50], "0x");

            await mfs.connect(defender).burnBatch([1, 2], [30, 50]);
            expect(await mfs.balanceOf(defender, 1)).to.equal(20);
            expect(await mfs.balanceOf(defender, 2)).to.equal(0);

            await expect(mfs.moveResources(miner, defender, [1], [100], "0x")).to.be.reverted;
        });
      });
    });
  });