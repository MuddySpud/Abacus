



































































































































































































#region Test regions

namespace Nested
{
    /// <summary>
    /// Retrieve or manipulate individual nodes
    /// </summary>
    public class PreProcessorTest
    {
#pragma warning disable CA1056 // Uri properties should not be strings
        public string Url { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="name">Name</param>
        public void method1(string name)
        {
            Func<(string, bool)> fred = () =>
            {
                string h = "";
                bool intime = true;

                return (h, intime);
            };

#if DEBUG
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} - ProcessID = {_processState.ID}");
#endif
        }
    }


}

#endregion
