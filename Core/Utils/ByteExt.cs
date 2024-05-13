namespace Core.Utils
{
    public static class ByteExt
    {
        public static void ToConsole(this byte[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + ", ");
            }
            Console.WriteLine();
        }

        public static byte[] Slice(this byte[] arr, int start, int end)
        {
            byte[] res = new byte[end - start];
            for (int i = start, j = 0; i < end; i++, j++)
            {
                res[j] = arr[i];
            }
            return res;
        }

        public static byte[] Slice(this byte[] arr, int start)
        {
            return arr.Slice(start, arr.Length);
        }
    }
}
