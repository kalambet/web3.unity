const {
  time,
  loadFixture,
} = require("@nomicfoundation/hardhat-toolbox/network-helpers");
const { anyValue } = require("@nomicfoundation/hardhat-chai-matchers/withArgs");
const { expect } = require("chai");

describe("Lock", function () {
    async function deployBasicGameSession() {
        const [minter, defender] = await ethers.getSigners();

        // const ids = [1, 2, 3, 4];
        // const values= [100, 200, 300, 400];

        const MinterDefenderSession = await ethers.getContractFactory("MinterDefenderSession");
        const mfs = await MinterDefenderSession.deploy();

        return { mfs, minter, defender };
    }

    describe("Deployment", function () {
    it("Should set the right players", async function () {
        const { mfs, minter, defender } = await loadFixture(deployOneYearLockFixture);

        expect(await mfs.minter()).to.equal(minter);
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