using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiTest
{
    [TestClass]
    public class Main
    {
        [TestMethod]
        public void StartGamePlaySimulation()
        {
            // =========== CONFIG ===============================================================================
            // Note that it might not always be necessary to execute this simulation when running all unit tests
            const bool enableSimulation = false;
            const string gameId = "1lzg54";
            const int howManySeasons = 3;
            // ==================================================================================================

            if (!enableSimulation)
                return;

            var simulator = new Simulator(gameId, howManySeasons);
            simulator.Start();
        }
    }
}
