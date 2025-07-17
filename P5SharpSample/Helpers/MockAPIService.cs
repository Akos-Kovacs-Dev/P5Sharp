using P5SharpSample.Interfaces;

namespace P5SharpSample.Helpers
{


    public class MockAPIService : IMockAPIService
    {
        // Simulated method to fetch mock data asynchronously
        public async Task<List<string>> GetDataAsync()
        {
            // Simulate a delay to represent an API call
            await Task.Delay(3000);

            // Return mock data
            return new List<string>
            {
                "Item 1",
                "Item 2",
                "Item 3"
            };
        }
    }


}
