    internal class JsonEdgesParser2 : IJsonEdgesParser
    {

        // Declare an array to store the data elements.
        private T[] arr = new T[100];
        int nextIndex = 0;


        public F this[int i]
            => arr[i];


    }


