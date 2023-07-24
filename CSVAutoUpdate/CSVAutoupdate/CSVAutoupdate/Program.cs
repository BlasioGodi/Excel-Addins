class Program
{
    static void Main()
    {
        // File paths
        string sourceFilePath = @"C:\Users\user\Desktop\MT5_TERMINALS\PATH_1\Original.csv";
        string destinationFolder1 = @"C:\Users\user\Desktop\MT5_TERMINALS\PATH_2\";
        string destinationFolder2 = @"C:\Users\user\Desktop\MT5_TERMINALS\PATH_3\";

        // Start a continuous update loop
        while (true)
        {
            // Check if the source CSV file has been updated
            if (File.GetLastWriteTime(sourceFilePath) > GetLastUpdateTime())
            {
                // Read data from the source CSV file
                string[] lines = File.ReadAllLines(sourceFilePath);

                // Process and transfer data to other CSV files
                foreach (string line in lines)
                {
                    // Split the CSV data assuming comma-separated values
                    string[] data = line.Split(',');

                    // Process the data if needed
                    string processedData = ProcessData(data);

                    // Transfer data to the first destination CSV file
                    string destinationFile1 = Path.Combine(destinationFolder1, "destination1.csv");
                    TransferData(destinationFile1, processedData);

                    // Transfer data to the second destination CSV file
                    string destinationFile2 = Path.Combine(destinationFolder2, "destination2.csv");
                    TransferData(destinationFile2, processedData);
                }

                // Update the last update time to avoid processing the same data multiple times
                SetLastUpdateTime(DateTime.Now);
            }

            // Wait for a specified time before checking for updates again (e.g., 1 minute)
            Thread.Sleep(60000);
        }
    }

    // Process data (you can customize this based on your needs)
    static string ProcessData(string[] data)
    {
        // For simplicity, just concatenate the first and second columns
        return data[0] + "," + data[1];
    }

    // Write data to the destination CSV file
    static void TransferData(string destinationFile, string data)
    {
        // Append data to the destination CSV file
        using (StreamWriter writer = new StreamWriter(destinationFile, true))
        {
            writer.WriteLine(data);
        }
    }

    // Store and retrieve the last update time in a file or database
    static DateTime GetLastUpdateTime()
    {
        // Implement your logic to retrieve the last update time
        // For simplicity, we will return DateTime.MinValue for the initial check
        return DateTime.MinValue;
    }

    static void SetLastUpdateTime(DateTime updateTime)
    {
        // Implement your logic to store the last update time
        // For simplicity, we won't store the time in this example
    }
}
