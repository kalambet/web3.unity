const {
    time,
    loadFixture,
  } = require("@nomicfoundation/hardhat-toolbox/network-helpers");
  const { anyValue } = require("@nomicfoundation/hardhat-chai-matchers/withArgs");
  const { expect } = require("chai");
  
  describe("Lock", function () {
      async function deployBasicGameSession() {
          const [miner, defender] = await ethers.getSigners();
  
          // const ids = [1, 2, 3, 4];
          // const values= [100, 200, 300, 400];
  
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
  
      describe("Deployment", function () {
      it("`mint` should be onlly callable by the miner", async function () {
          const { mfs, miner, defender } = await loadFixture(deployBasicGameSession);
  
          expect(await mfs.miner()).to.equal(miner);
          expect(await mfs.defender()).to.equal(defender);
      });
  
      // it("Should set the right owner", async function () {
      //   const { lock, owner } = await loadFixture(deployOneYearLockFixture);
  
      //   expect(await lock.owner()).to.equal(owner.address);
      // });
  
      // it("Should receive and store the funds to lock", async function () {
      //   const { lock, lockedAmount } = await loadFixture(
      //     deployOneYearLockFixture
      //   );
  
      //   expect(await ethers.provider.getBalance(lock.target)).to.equal(
      //     lockedAmount
      //   );
      });
  
  });